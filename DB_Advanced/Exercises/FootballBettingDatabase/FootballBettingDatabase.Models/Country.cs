namespace FootballBettingDatabase.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Country
    {
        public Country()
        {
            this.Towns = new HashSet<Town>();
            this.Continents = new HashSet<Continent>();
        }

        [StringLength(3)]
        public string CountryId { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Continent> Continents { get; set; }

        public virtual ICollection<Town> Towns { get; set; }
    }
}
