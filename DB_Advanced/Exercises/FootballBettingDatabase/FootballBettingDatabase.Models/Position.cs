namespace FootballBettingDatabase.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Position
    {
        public Position()
        {
            this.Players = new HashSet<Player>();
        }

        [StringLength(2)]
        public string PositionId { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Player> Players { get; set; }
    }
}