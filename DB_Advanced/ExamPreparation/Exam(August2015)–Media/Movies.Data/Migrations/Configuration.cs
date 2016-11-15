namespace Movies.Data.Migrations
{
    using Models;
    using Models.JsonModels;
    using Newtonsoft.Json;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MoviesEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "Movies.Data.MoviesEntities";
        }

        protected override void Seed(MoviesEntities context)
        {
            if (!context.Countries.Any())
            {
                this.ImportCountriesFromJson(context);
            }

            if (!context.Movies.Any())
            {
                this.ImportMoviesFromJson(context);
            }

            if (!context.Users.Any())
            {
                this.ImportUsersFromJson(context);
                this.ImportUsersFavMovies(context);
            }

            if (!context.Ratings.Any())
            {
                this.ImportRatingsFromJson(context);
            }

            base.Seed(context);
        }

        private void ImportRatingsFromJson(MoviesEntities context)
        {
            var ratings = File.ReadAllText("../../../data/movie-ratings.json");
            var jsonRatings = JsonConvert.DeserializeObject<JsonMovieRating[]>(ratings);

            foreach (var rating in jsonRatings)
            {
                var newRating = new Rating
                {
                    Stars = rating.Rating,
                    User = context.Users.FirstOrDefault(u => u.Username == rating.User),
                    Movie = context.Movies.FirstOrDefault(m => m.Isbn == rating.Movie)
                };
                context.Ratings.Add(newRating);
            }
            context.SaveChanges();
        }

        private void ImportUsersFavMovies(MoviesEntities context)
        {
            var usersAndMovies = File.ReadAllText("../../../data/users-and-favourite-movies.json");
            var json = JsonConvert.DeserializeObject<JsonUserAndMovies[]>(usersAndMovies);

            foreach (var user in json)
            {
                var currentUser = context.Users.FirstOrDefault(u => u.Username == user.Username);
                if (currentUser != null)
                {
                    foreach (var isbn in user.FavouriteMovies)
                    {
                        var currentMovie = context.Movies.FirstOrDefault(m => m.Isbn == isbn);
                        if (currentMovie != null)
                        {
                            currentUser.FavouriteMovies.Add(currentMovie);
                        }
                    }
                }
            }
            context.SaveChanges();
        }

        private void ImportUsersFromJson(MoviesEntities context)
        {
            var users = File.ReadAllText("../../../data/users.json");
            var jsonUsers = JsonConvert.DeserializeObject<JsonUser[]>(users);

            foreach (var jsonUser in jsonUsers)
            {
                var newUser = new User
                {
                    Username = jsonUser.Username,
                    Email = jsonUser.Email,
                    Age = jsonUser.Age,
                    Country = context.Countries.FirstOrDefault(c => c.Name == jsonUser.Country)
                };
                context.Users.Add(newUser);
            }
            context.SaveChanges();
        }

        private void ImportMoviesFromJson(MoviesEntities context)
        {
            var movies = File.ReadAllText("../../../data/movies.json");
            var jsonMovies = JsonConvert.DeserializeObject<JsonMovie[]>(movies);

            foreach (var jsonMovie in jsonMovies)
            {
                context.Movies.Add(new Movie
                {
                    Isbn = jsonMovie.Isbn,
                    Title = jsonMovie.Title,
                    Restriction = (AgeRestriction)jsonMovie.AgeRestriction
                });
            }

            context.SaveChanges();
        }

        private void ImportCountriesFromJson(MoviesEntities context)
        {
            var countries = File.ReadAllText("../../../data/countries.json");
            var jsonCountries = JsonConvert.DeserializeObject<JsonCountry[]>(countries);

            foreach (var jsonCountry in jsonCountries)
            {
                context.Countries.Add(new Country { Name = jsonCountry.Name });
            }

            context.SaveChanges();
        }
    }
}
