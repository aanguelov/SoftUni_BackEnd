namespace FootballBettingDatabase.Models
{
    public class PlayerStatistic
    {
        public int PlayerId { get; set; }

        public int GameId { get; set; }

        public int ScoredGoals { get; set; }

        public int Assists { get; set; }

        public int PlayedMinutes { get; set; }

        public virtual Player Player { get; set; }

        public virtual Game Game { get; set; }
    }
}
