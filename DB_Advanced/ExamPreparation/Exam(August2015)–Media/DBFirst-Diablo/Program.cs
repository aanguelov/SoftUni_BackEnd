namespace DBFirst_Diablo
{
    using Newtonsoft.Json;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Xml.Linq;

    public class Program
    {
        static void Main()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            var ctx = new DiabloEntities();

            //ListAllCharactersNames(ctx);

            //ExportCharactersAndPlayers(ctx);

            //ExportFinishedGames(ctx);

            ImportUsersAndTheirGames(ctx);
        }

        private static void ImportUsersAndTheirGames(DiabloEntities ctx)
        {
            var file = XDocument.Load("../../data/users-and-games.xml");
            var usersToImport = file.Root.Elements();

            foreach (var user in usersToImport)
            {
                var username = user.Attribute("username").Value;
                if (ctx.Users.FirstOrDefault(u => u.Username == username) == null)
                {
                    var firstName = user.Attribute("first-name") != null ? user.Attribute("first-name").Value : null;
                    var lastName = user.Attribute("last-name") != null ? user.Attribute("last-name").Value : null;
                    var email = user.Attribute("email") != null ? user.Attribute("email").Value : null;
                    var isDeleted = user.Attribute("is-deleted").Value != "0";
                    var ipAddress = user.Attribute("ip-address").Value;
                    var regDate = DateTime.ParseExact(user.Attribute("registration-date").Value, "dd/mm/yyyy", CultureInfo.InvariantCulture);

                    var newUser = new User
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Username = username,
                        Email = email,
                        IsDeleted = isDeleted,
                        IpAddress = ipAddress,
                        RegistrationDate = regDate
                    };

                    foreach (var game in user.Element("games").Elements())
                    {
                        var gameName = game.Element("game-name").Value;
                        var characterName = game.Element("character").Attribute("name").Value;
                        var cash = decimal.Parse(game.Element("character").Attribute("cash").Value);
                        var level = int.Parse(game.Element("character").Attribute("level").Value);
                        var joinedOn = DateTime.ParseExact(game.Element("joined-on").Value, "dd/mm/yyyy", CultureInfo.InvariantCulture);

                        var newUserGame = new UsersGame
                        {
                            Game = ctx.Games.First(g => g.Name == gameName),
                            User = newUser,
                            Character = ctx.Characters.FirstOrDefault(ch => ch.Name == characterName),
                            Cash = cash,
                            Level = level,
                            JoinedOn = joinedOn
                        };
                        ctx.UsersGames.Add(newUserGame);
                        Console.WriteLine($"User {username} successfully added to game {gameName}");
                    }
                    ctx.SaveChanges();
                }
                else
                {
                    Console.WriteLine($"User {username} already exists");
                }
            }
        }

        private static void ExportFinishedGames(DiabloEntities ctx)
        {
            var gamesAndUsers = ctx.Games
                .Where(g => g.IsFinished == true)
                .Select(g => new
                {
                    name = g.Name,
                    duration = g.Duration,
                    users = g.UsersGames.Select(ug => new
                    {
                        username = ug.User.Username,
                        ipAddress = ug.User.IpAddress
                    })
                })
                .OrderBy(g => g.name)
                .ThenBy(g => g.duration)
                .ToList();

            var xml = new XDocument();
            var root = new XElement("games");
            xml.Add(root);

            gamesAndUsers.ForEach(g =>
            {
                var gameElement = new XElement("game");
                var nameAttr = new XAttribute("name", g.name);
                gameElement.Add(nameAttr);
                if (g.duration != null)
                {
                    var durationAttr = new XAttribute("duration", g.duration);
                    gameElement.Add(durationAttr);
                }

                var usersElement = new XElement("users");
                foreach (var user in g.users)
                {
                    var userElement = new XElement("user");
                    var usernameAttr = new XAttribute("username", user.username);
                    var ipAttr = new XAttribute("ip-address", user.ipAddress);
                    userElement.Add(usernameAttr);
                    userElement.Add(ipAttr);
                    usersElement.Add(userElement);
                }

                gameElement.Add(usersElement);
                root.Add(gameElement);
            });

            xml.Save("../../extracted/finished-games.xml");
        }

        private static void ExportCharactersAndPlayers(DiabloEntities ctx)
        {
            var characters = ctx.Characters.Select(ch => new
            {
                name = ch.Name,
                playedBy = ch.UsersGames.Select(ug => ug.User.Username)
            })
            .OrderBy(ch => ch.name);

            var charactersJson = JsonConvert.SerializeObject(characters, Formatting.Indented);

            File.WriteAllText("../../extracted/characters.json", charactersJson);
        }

        private static void ListAllCharactersNames(DiabloEntities ctx)
        {
            var charcterNames = ctx.Characters.Select(ch => ch.Name).ToList();

            charcterNames.ForEach(ch =>
            {
                Console.WriteLine(ch);
            });
        }
    }
}
