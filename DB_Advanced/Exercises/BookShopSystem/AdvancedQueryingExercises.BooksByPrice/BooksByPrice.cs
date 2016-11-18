namespace AdvancedQueryingExercises.BooksByPrice
{
    using BookShopSystem.Data;
    using System.Linq;

    public class BooksByPrice
    {
        static void Main()
        {
            var ctx = new BookShopContext();
            var booksByPrice = ctx.Books
                .Where(b => b.Price < 5 || b.Price > 40)
                .Select(b => new
                {
                    title = b.Title,
                    price = b.Price
                })
                .ToList();

            booksByPrice.ForEach(b =>
            {
                System.Console.WriteLine($"{b.title} - ${b.price}");
            });
        }
    }
}
