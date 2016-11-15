namespace Movies.ConsoleClient
{
    using Data;
    using System.Linq;

    public class ConsoleClient
    {
        static void Main()
        {
            var ctx = new MoviesEntities();
            ctx.Movies.Count();
        }
    }
}
