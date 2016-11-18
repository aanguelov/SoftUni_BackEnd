namespace AdvancedQueryingExercises.BooksReleasedBeforeDate
{
    using BookShopSystem.Data;
    using System;
    using System.Globalization;
    using System.Linq;

    public class BooksReleasedBeforeDate
    {
        static void Main()
        {
            var ctx = new BookShopContext();
            var inputDate = DateTime.ParseExact(Console.ReadLine(), "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var books = ctx.Books
                .Where(b => b.ReleaseDate < inputDate)
                .Select(b => new
                {
                    b.Title,
                    b.EditionType,
                    b.Price
                })
                .ToList();

            books.ForEach(b =>
            {
                Console.WriteLine($"Title: {b.Title}, Type: {b.EditionType}, Price: ${b.Price}");
            });
        }
    }
}
