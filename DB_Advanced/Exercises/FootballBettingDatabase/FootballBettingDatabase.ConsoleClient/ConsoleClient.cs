namespace FootballBettingDatabase.ConsoleClient
{
    using Data;
    using System.Linq;

    public class ConsoleClient
    {
        static void Main()
        {
            var ctx = new FootballBettingContext();
            ctx.Bets.Count();
        }
    }
}
