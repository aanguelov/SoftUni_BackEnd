namespace AdvancedQueryingExercises.TotalBookCopies
{
    using BookShopSystem.Data;
    using System;
    using System.Linq;

    class TotalBookCopies
    {
        static void Main()
        {
            var ctx = new BookShopContext();

            var authors = ctx.Authors
                .Where(a => a.Books.Any())
                .Select(a => new
                {
                    name = a.FirstName + " " + a.LastName,
                    copies = a.Books.Sum(b => b.Copies)
                })
                .OrderByDescending(a => a.copies);

            foreach (var author in authors)
            {
                Console.WriteLine($"{author.name} – {author.copies}");
            }
        }
    }
}
