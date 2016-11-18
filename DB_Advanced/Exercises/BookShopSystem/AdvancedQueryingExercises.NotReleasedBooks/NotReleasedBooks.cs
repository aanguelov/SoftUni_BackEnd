namespace AdvancedQueryingExercises.NotReleasedBooks
{
    using BookShopSystem.Data;
    using System;
    using System.Linq;

    public class NotReleasedBooks
    {
        static void Main()
        {
            var year = int.Parse(Console.ReadLine());
            var ctx = new BookShopContext();
            var notReleasedBooks = ctx.Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .Select(b => b.Title)
                .ToList();

            notReleasedBooks.ForEach(b =>
            {
                Console.WriteLine(b);
            });
        }
    }
}
