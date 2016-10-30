namespace BookShopSystem.ConsoleClient
{
    using Data;
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading;

    public class ConsoleClient
    {
        static void Main()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            var ctx = new BookShopContext();

            using (ctx)
            {
                //GetAllBooksAfterTheYear2000(ctx);

                //GetAuthorsWithBooksBefore1990(ctx);

                //GetAuthorsByNumberOfBooks(ctx);

                //GetBooksFromRogerPorter(ctx);

                //GetMostRecentBooksByCategory(ctx);

                var relatedBooks = ctx.Books.Take(3);

                foreach (var book in relatedBooks)
                {
                    Console.WriteLine($"--{book.Title}");
                    foreach (var related in book.RelatedBooks)
                    {
                        Console.WriteLine(related.Title);
                    }
                }

            }
        }

        private static void GetMostRecentBooksByCategory(BookShopContext ctx)
        {
            var categories = ctx.Categories.OrderByDescending(c => c.Books.Count);

            foreach (var category in categories)
            {
                Console.WriteLine($"--{category.Name}: {category.Books.Count} books");

                var top3Books = category.Books
                                            .OrderByDescending(b => b.ReleaseDate.Value.Year)
                                            .ThenBy(b => b.Title)
                                            .Take(3);
                foreach (var book in top3Books)
                {
                    Console.WriteLine($"{book.Title} ({book.ReleaseDate.Value.Year})");
                }
            }
        }

        private static void GetBooksFromRogerPorter(BookShopContext ctx)
        {
            var books = ctx.Books
                            .Where(b => b.Author.FirstName == "Roger" && b.Author.LastName == "Porter")
                            .OrderByDescending(b => b.ReleaseDate)
                            .ThenBy(b => b.Title);
            foreach (var book in books)
            {
                Console.WriteLine($"{book.Title} {book.ReleaseDate} {book.Copies}");
            }
        }

        private static void GetAuthorsByNumberOfBooks(BookShopContext ctx)
        {
            var authors = ctx.Authors.OrderByDescending(a => a.Books.Count);

            foreach (var author in authors)
            {
                Console.WriteLine($"{author.FirstName} {author.LastName} - {author.Books.Count} books");
            }
        }

        private static void GetAuthorsWithBooksBefore1990(BookShopContext ctx)
        {
            var authors = ctx.Authors.Where(a => a.Books.Count(b => b.ReleaseDate.Value.Year < 1990) > 0);

            foreach (var author in authors)
            {
                Console.WriteLine($"{author.FirstName} {author.LastName}");
            }
        }

        private static void GetAllBooksAfterTheYear2000(BookShopContext ctx)
        {
            var books = ctx.Books
                            .Where(b => b.ReleaseDate.Value.Year > 2000)
                            .Select(b => b.Title);
            foreach (var title in books)
            {
                Console.WriteLine(title);
            }
        }
    }
}
