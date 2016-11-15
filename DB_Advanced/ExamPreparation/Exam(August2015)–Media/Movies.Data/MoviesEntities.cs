namespace Movies.Data
{
    using Migrations;
    using Models;
    using System.Data.Entity;

    public class MoviesEntities : DbContext
    {
        public MoviesEntities()
            : base("name=MoviesEntities")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<MoviesEntities, Configuration>());
        }

        public IDbSet<Country> Countries { get; set; }

        public IDbSet<Movie> Movies { get; set; }

        public IDbSet<User> Users { get; set; }

        public IDbSet<Rating> Ratings { get; set; }
    }
}