namespace MassDefect.Data
{
    using Models;
    using System.Data.Entity;

    public class MassDefectEntities : DbContext
    {
        public MassDefectEntities()
            : base("name=MassDefectEntities")
        {
        }

        public virtual IDbSet<SolarSystem> SolarSystems { get; set; }

        public virtual IDbSet<Star> Stars { get; set; }

        public virtual IDbSet<Planet> Planets { get; set; }

        public virtual IDbSet<Person> Persons { get; set; }

        public virtual IDbSet<Anomaly> Anomalies { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                .HasMany(p => p.Anomalies)
                .WithMany(a => a.Victims)
                .Map(m =>
                {
                    m.MapLeftKey("AnomalyId");
                    m.MapRightKey("PersonId");
                    m.ToTable("AnomalyVictims");
                });

            modelBuilder.Entity<Anomaly>()
                .HasRequired(a => a.TeleportPlanet)
                .WithMany(p => p.TeleportPlanetAnomalies)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Anomaly>()
                .HasRequired(a => a.OriginPlanet)
                .WithMany(p => p.OriginPlanetAnomalies)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Star>()
                .HasRequired(s => s.SolarSystem)
                .WithMany(ss => ss.Stars)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}