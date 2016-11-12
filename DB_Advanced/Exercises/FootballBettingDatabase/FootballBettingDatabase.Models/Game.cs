namespace FootballBettingDatabase.Models
{
    using System;
    using System.Collections.Generic;

    public class Game
    {
        public Game()
        {
            this.PlayerStatistics = new HashSet<PlayerStatistic>();
            this.BetGames = new HashSet<BetGame>();
        }

        public int Id { get; set; }

        public int HomeTeamGoals { get; set; }

        public int AwayTeamGoals { get; set; }

        public DateTime MatchDate { get; set; }

        public double HomeOdds { get; set; }

        public double AwayOdds { get; set; }

        public double DrawOdds { get; set; }

        public virtual Team HomeTeam { get; set; }

        public virtual Team AwayTeam { get; set; }

        public virtual Round Round { get; set; }

        public virtual Competition Competition { get; set; }

        public virtual ICollection<PlayerStatistic> PlayerStatistics { get; set; }

        public virtual ICollection<BetGame> BetGames { get; set; }
    }
}