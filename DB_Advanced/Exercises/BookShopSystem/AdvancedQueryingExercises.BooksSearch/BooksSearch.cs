namespace AdvancedQueryingExercises.BooksSearch
{
    using BookShopSystem.Data;
    using System;
    using System.Linq;

    public class BooksSearch
    {
        static void Main()
        {
            var ctx = new BookShopContext();
            var input = Console.ReadLine().ToLower();

            var bookTitles = ctx.Books
                .Where(b => b.Title.ToLower().Contains(input))
                .Select(b => b.Title)
                .ToList();

            bookTitles.ForEach(t =>
            {
                Console.WriteLine(t);
            });
        }
    }
}
