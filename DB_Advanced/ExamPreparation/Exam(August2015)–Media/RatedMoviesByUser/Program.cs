namespace RatedMoviesByUser
{
    using Movies.Data;
    using Newtonsoft.Json;
    using System;
    using System.IO;
    using System.Linq;

    public class Program
    {
        static void Main()
        {
            var ctx = new MoviesEntities();
            var username = Console.ReadLine();
            var user = ctx.Users.FirstOrDefault(u => u.Username == username);

            if (user != null)
            {
                var ratedMovies = ctx.Users
                    .Where(u => u.Username == username)
                    .Select(u => new
                    {
                        username = u.Username,
                        ratedMovies = u.RatedMovies.Select(m => new
                        {
                            title = m.Movie.Title,
                            userRating = m.Stars,
                            averageRating = m.Movie.Ratings.Average(r => r.Stars)
                        })
                        .OrderBy(m => m.title)
                    });
                var jsonMovies = JsonConvert.SerializeObject(ratedMovies, Formatting.Indented);
                File.WriteAllText($"../../../exported/rated-movies-by-{username}.json", jsonMovies);
                Console.WriteLine("Movies exported.");
            }
            else
            {
                throw new InvalidOperationException($"User {username} is not in the database!");
            }
        }
    }
}
