namespace FootballBettingDatabase.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Player
    {
        public Player()
        {
            this.PlayerGames = new HashSet<PlayerStatistic>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int SquadNumber { get; set; }

        public bool IsCurrentlyInjured { get; set; }

        public virtual Team Team { get; set; }

        public virtual Position Position { get; set; }

        public virtual ICollection<PlayerStatistic> PlayerGames { get; set; }
    }
}
