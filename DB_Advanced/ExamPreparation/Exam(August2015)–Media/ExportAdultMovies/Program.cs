namespace ExportAdultMovies
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
            var adultMovies = ctx.Movies
                .Where(m => m.Restriction == AgeRestriction.Adult)
                .Select(m => new
                {
                    title = m.Title,
                    ratingsGiven = m.Ratings.Count
                })
                .OrderBy(m => m.title)
                .ThenBy(m => m.ratingsGiven);

            var jsonMovies = JsonConvert.SerializeObject(adultMovies, Formatting.Indented);
            File.WriteAllText("../../../exported/adult-movies.json", jsonMovies);
        }
    }
}
