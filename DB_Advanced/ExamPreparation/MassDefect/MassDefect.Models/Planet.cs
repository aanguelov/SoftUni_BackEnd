namespace MassDefect.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Planet
    {
        public Planet()
        {
            this.OriginPlanetAnomalies = new HashSet<Anomaly>();
            this.TeleportPlanetAnomalies = new HashSet<Anomaly>();
            this.Persons = new HashSet<Person>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public virtual Star Sun { get; set; }

        [Required]
        public virtual SolarSystem SolarSystem { get; set; }

        public virtual ICollection<Person> Persons { get; set; }

        public virtual ICollection<Anomaly> OriginPlanetAnomalies { get; set; }

        public virtual ICollection<Anomaly> TeleportPlanetAnomalies { get; set; }
    }
}
