namespace AdvancedQueryingExercises.GoldenBooks
{
    using BookShopSystem.Data;
    using BookShopSystem.Models;
    using System.Linq;

    public class GoldenBooks
    {
        static void Main()
        {
            var ctx = new BookShopContext();
            var goldenBooks = ctx.Books
                .Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000)
                .Select(b => b.Title)
                .ToList();

            goldenBooks.ForEach(b =>
            {
                System.Console.WriteLine(b);
            });
        }
    }
}
