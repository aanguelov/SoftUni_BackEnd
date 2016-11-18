namespace AdvancedQueryingExercises.BookTitlesByAgeRestriction
{
    using BookShopSystem.Data;
    using System;
    using System.Linq;

    public class BookTitlesByAgeRestriction
    {
        static void Main()
        {
            var ctx = new BookShopContext();
            var ageRestriction = Console.ReadLine().ToLower();

            var booksByAgeRestriction = ctx.Books
                .Where(b => b.AgeRestriction.ToString().ToLower() == ageRestriction)
                .Select(b => b.Title)
                .ToList();

            booksByAgeRestriction.ForEach(b =>
            {
                Console.WriteLine(b);
            });
        }
    }
}
