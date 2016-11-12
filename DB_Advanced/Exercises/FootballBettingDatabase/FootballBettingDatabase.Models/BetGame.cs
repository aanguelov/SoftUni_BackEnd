namespace FootballBettingDatabase.Models
{
    using System.ComponentModel.DataAnnotations;

    public class BetGame
    {
        public int GameId { get; set; }

        public int BetId { get; set; }

        [Required]
        public ResultPrediction Prediction { get; set; }

        public virtual Game Game { get; set; }

        public virtual Bet Bet { get; set; }
    }
}
