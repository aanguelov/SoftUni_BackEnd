namespace Top10FavouriteMovies
{
    using Movies.Data;
    using Movies.Models;
    using Newtonsoft.Json;
    using System.IO;
    using System.Linq;

    public class Program
    {
        static void Main()
        {
            var ctx = new MoviesEntities();
            var topFavouritedMovies = ctx.Movies
                .Where(m => m.Restriction == AgeRestriction.Teen)
                .Select(m => new
                {
                    isbn = m.Isbn,
                    title = m.Title,
                    favouritedBy = m.Users.Select(u => u.Username)
                })
                .OrderByDescending(m => m.favouritedBy.Count())
                .ThenBy(m => m.title)
                .Take(10);

            var jsonMovies = JsonConvert.SerializeObject(topFavouritedMovies, Formatting.Indented);
            File.WriteAllText("../../../exported/top-10-favourite-movies.json", jsonMovies);
        }
    }
}
