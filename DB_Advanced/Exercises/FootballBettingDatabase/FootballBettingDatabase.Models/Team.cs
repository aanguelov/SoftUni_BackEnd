namespace FootballBettingDatabase.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Team
    {
        public Team()
        {
            this.Players = new HashSet<Player>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public byte[] Logo { get; set; }

        [StringLength(3)]
        public string Initials { get; set; }

        public decimal? Budget { get; set; }

        public virtual Colour PrimaryKitColour { get; set; }

        public virtual Colour SecondaryKitColour { get; set; }

        public virtual Town Town { get; set; }

        public virtual ICollection<Player> Players { get; set; }
    }
}
