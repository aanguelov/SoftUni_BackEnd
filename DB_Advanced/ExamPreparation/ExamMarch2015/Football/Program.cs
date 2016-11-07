using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Football
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            var ctx = new FootballEntities();

            //ListAllTeamNames(ctx);

            //ExportLeaguesAndTeamsToJson(ctx);

            //ExportInternationalMatches(ctx);

            //ImportLeaguesAndTeamsFromXml(ctx);

            //GenerateRandomMatches(ctx);
        }

        private static void GenerateRandomMatches(FootballEntities ctx)
        {
            var document = XDocument.Load("../../data/generate-matches.xml");
            var generators = document.Root.Elements();

            var requestCount = 1;

            foreach (var generator in generators)
            {
                Console.WriteLine($"Processing request #{requestCount} ...");

                var generateCount = generator.Attribute("generate-count") != null ? int.Parse(generator.Attribute("generate-count").Value) : 10;

                var maxGoals = generator.Attribute("max-goals") != null ? int.Parse(generator.Attribute("max-goals").Value) : 5;

                var leagueName = generator.Element("league") != null ? generator.Element("league").Value : "no league";

                var startDate = generator.Element("start-date") != null ? DateTime.Parse(generator.Element("start-date").Value) : new DateTime(2000, 1, 1);

                var endDate = generator.Element("end-date") != null ? DateTime.Parse(generator.Element("end-date").Value) : new DateTime(2015, 12, 31);
                var daysRange = (endDate - startDate).Days;

                var teams = ctx.Teams.Select(t => t.TeamName).ToArray();

                if (leagueName != "no league")
                {
                    teams = ctx.Leagues
                        .First(l => l.LeagueName == leagueName)
                        .Teams
                        .Select(t => t.TeamName)
                        .ToArray();
                }

                var teamsCount = teams.Count();
                var rand = new Random();

                for (int i = 0; i < generateCount; i++)
                {
                    var matchDate = startDate.AddDays(rand.Next(daysRange));
                    var homeIndex = rand.Next(0, teamsCount/2);
                    var awayIndex = rand.Next(teamsCount / 2, teamsCount);
                    var homeGoals = rand.Next(0, maxGoals + 1);
                    var awayGoals = rand.Next(0, maxGoals + 1);

                    var homeTeamName = teams[homeIndex];
                    var awayTeamName = teams[awayIndex];

                    var newTeamMatch = new TeamMatch
                    {
                        HomeTeamId = ctx.Teams.First(t => t.TeamName == homeTeamName).Id,
                        AwayTeamId = ctx.Teams.First(t => t.TeamName == awayTeamName).Id,
                        HomeGoals = homeGoals,
                        AwayGoals = awayGoals,
                        MatchDate = matchDate
                    };
                    if (ctx.Leagues.FirstOrDefault(l => l.LeagueName == leagueName) != null)
                    {
                        newTeamMatch.LeagueId = ctx.Leagues.FirstOrDefault(l => l.LeagueName == leagueName).Id;
                    }

                    ctx.TeamMatches.Add(newTeamMatch);
                    ctx.SaveChanges();

                    Console.WriteLine($"{matchDate}: {teams[homeIndex]} - {teams[awayIndex]}: {homeGoals}-{awayGoals} ({leagueName})");
                }

                requestCount++;
            }
        }

        private static void ImportLeaguesAndTeamsFromXml(FootballEntities ctx)
        {
            var document = XDocument.Load(@"../../data/leagues-and-teams.xml");
            var leagues = document.Root.Elements();

            var leaguesCount = 1;
            foreach (var league in leagues)
            {
                Console.WriteLine($"Processing league #{leaguesCount} ...");
                League xmlLeague = null;

                if (league.Element("league-name") != null)
                {
                    var leagueName = league.Element("league-name").Value;
                    var leagueFromDB = ctx.Leagues.FirstOrDefault(l => l.LeagueName == leagueName);

                    if (leagueFromDB != null)
                    {
                        xmlLeague = leagueFromDB;
                        Console.WriteLine($"Existing league: {leagueName}");
                    }
                    else
                    {
                        xmlLeague = new League { LeagueName = leagueName };
                        Console.WriteLine($"Created league: {leagueName}");
                    }
                }

                var teams = league.Element("teams");
                if (teams != null)
                {
                    foreach (var team in teams.Elements())
                    {
                        string teamName = team.Attribute("name").Value;
                        string teamCountry = team.Attribute("country") != null ? team.Attribute("country").Value : null;

                        var teamFromDB = ctx.Teams
                            .FirstOrDefault(t => t.TeamName == teamName && t.Country.CountryName == teamCountry);
                        if (teamFromDB != null)
                        {
                            var country = teamFromDB.Country != null ? teamFromDB.Country.CountryName : "no country";
                            Console.WriteLine($"Existing team: {teamFromDB.TeamName} ({country})");

                            if (xmlLeague != null)
                            {
                                if (teamFromDB.Leagues.FirstOrDefault(l => l.LeagueName == xmlLeague.LeagueName) != null)
                                {
                                    Console.WriteLine($"Existing team in league: {teamFromDB.TeamName} belongs to {xmlLeague.LeagueName}");
                                }
                                else
                                {
                                    teamFromDB.Leagues.Add(xmlLeague);
                                    xmlLeague.Teams.Add(teamFromDB);
                                    Console.WriteLine($"Added team to league: {teamFromDB.TeamName} to league {xmlLeague.LeagueName}");
                                }
                            }
                        }
                        else
                        {
                            var teamCountryCode = ctx.Countries
                                .FirstOrDefault(c => c.CountryName == teamCountry) != null 
                                ? ctx.Countries.FirstOrDefault(c => c.CountryName == teamCountry).CountryCode 
                                : null;
                            var xmlTeam = new Team { TeamName = teamName, CountryCode = teamCountryCode };
                            ctx.Teams.Add(xmlTeam);
                            string country = teamCountry != null ? teamCountry : "no country";
                            Console.WriteLine($"Created team: {teamName} ({country})");

                            if (xmlLeague != null)
                            {
                                xmlLeague.Teams.Add(xmlTeam);
                                xmlTeam.Leagues.Add(xmlLeague);
                                Console.WriteLine($"Added team to league: {teamName} to league {xmlLeague.LeagueName}");
                            }
                        }
                    }
                }

                if (xmlLeague != null)
                {
                    if (ctx.Leagues.FirstOrDefault(l => l.LeagueName == xmlLeague.LeagueName) == null)
                    {
                        ctx.Leagues.Add(xmlLeague);
                    }                 
                }
                ctx.SaveChanges();

                leaguesCount++;
            }
        }

        private static void ExportInternationalMatches(FootballEntities ctx)
        {
            var internationMatches = ctx.InternationalMatches.Select(m => new
            {
                matchDate = m.MatchDate,
                homeCountry = m.HomeCountry.CountryName,
                homeCountryCode = m.HomeCountryCode,
                awayCountry = m.AwayCountry.CountryName,
                awayCountryCode = m.AwayCountryCode,
                league = m.League.LeagueName,
                score = m.HomeGoals + "-" + m.AwayGoals
            })
            .OrderBy(m => m.matchDate)
            .ThenBy(m => m.homeCountry)
            .ThenBy(m => m.awayCountry)
            .ToList();

            var xml = new XDocument();
            var root = new XElement("matches");
            xml.Add(root);

            internationMatches.ForEach(m =>
            {
                var matchNode = new XElement("match");
                if (m.matchDate != null)
                {
                    if (m.matchDate.Value.TimeOfDay.TotalSeconds != 0)
                    {
                        matchNode.SetAttributeValue("date-time", m.matchDate.Value.ToString());
                    }
                    else
                    {
                        matchNode.SetAttributeValue("date", m.matchDate.Value.ToShortDateString());
                    }
                }

                var homeMatchNode = new XElement("home-country");
                homeMatchNode.SetAttributeValue("code", m.homeCountryCode);
                homeMatchNode.SetValue(m.homeCountry);

                var awayMatchNode = new XElement("away-country");
                awayMatchNode.SetAttributeValue("code", m.awayCountryCode);
                awayMatchNode.SetValue(m.awayCountry);

                matchNode.Add(homeMatchNode);
                matchNode.Add(awayMatchNode);

                if (m.score.Length > 1)
                {
                    var matchScore = new XElement("score");
                    matchScore.SetValue(m.score);
                    matchNode.Add(matchScore);
                }

                if (m.league != null)
                {
                    var matchLeague = new XElement("league");
                    matchLeague.SetValue(m.league);
                    matchNode.Add(matchLeague);
                }

                root.Add(matchNode);
            });

            xml.Save(@"../../international-matches.xml");
        }

        private static void ExportLeaguesAndTeamsToJson(FootballEntities ctx)
        {
            var leaguesAndTeams = ctx.Leagues.Select(l => new
            {
                leagueName = l.LeagueName,
                teams = l.Teams.Select(t => t.TeamName).OrderBy(s => s)
            })
            .OrderBy(l => l.leagueName);

            var leaguesAndTeamsJson = JsonConvert.SerializeObject(leaguesAndTeams, Formatting.Indented);
            File.WriteAllText(@"../../leagues-and-teams.json", leaguesAndTeamsJson);
        }

        private static void ListAllTeamNames(FootballEntities ctx)
        {
            var teamNames = ctx.Teams.Select(t => t.TeamName).ToList();

            teamNames.ForEach(Console.WriteLine);
        }
    }
}
