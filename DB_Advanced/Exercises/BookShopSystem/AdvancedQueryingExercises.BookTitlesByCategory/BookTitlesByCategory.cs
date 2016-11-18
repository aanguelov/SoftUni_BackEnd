namespace AdvancedQueryingExercises.BookTitlesByCategory
{
    using BookShopSystem.Data;
    using System;
    using System.Linq;

    public class BookTitlesByCategory
    {
        static void Main()
        {
            var categories = Console.ReadLine().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var ctx = new BookShopContext();
            var bookTitles = ctx.Books
                .Where(b => b.Categories.Where(c => categories.Contains(c.Name.ToLower())).Any())
                .Select(b => b.Title);

            foreach (var title in bookTitles)
            {
                Console.WriteLine(title);
            }
        }
    }
}
