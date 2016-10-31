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

                GetMostRecentBooksByCategory(ctx);

                //var relatedBooks = ctx.Books.Select(b => new
                //{
                //    b.Title,
                //    RelatedBooksTitles = b.RelatedBooks.Select(rb => new
                //    {
                //        rb.Title
                //    })
                //}).Take(3);

                //foreach (var book in relatedBooks)
                //{
                //    Console.WriteLine($"--{book.Title}");
                //    foreach (var related in book.RelatedBooksTitles)
                //    {
                //        Console.WriteLine(related.Title);
                //    }
                //}

            }
        }

        private static void GetMostRecentBooksByCategory(BookShopContext ctx)
        {
            var categories = ctx.Categories
                                .Select(c => new
                                {
                                    c.Name,
                                    c.Books,
                                    BookCount = c.Books.Count
                                })
                                .OrderByDescending(c => c.BookCount);

            foreach (var category in categories)
            {
                Console.WriteLine($"--{category.Name}: {category.BookCount} books");

                var top3Books = category.Books
                                            .Select(b => new
                                            {
                                                b.Title,
                                                ReleaseYear = b.ReleaseDate.Value.Year
                                            })
                                            .OrderByDescending(b => b.ReleaseYear)
                                            .ThenBy(b => b.Title)
                                            .Take(3);
                foreach (var book in top3Books)
                {
                    Console.WriteLine($"{book.Title} ({book.ReleaseYear})");
                }
            }
        }

        private static void GetBooksFromRogerPorter(BookShopContext ctx)
        {
            var books = ctx.Books
                            .Where(b => b.Author.FirstName == "Roger" && b.Author.LastName == "Porter")
                            .Select(b => new
                            {
                                b.Title,
                                b.ReleaseDate,
                                b.Copies
                            })
                            .OrderByDescending(b => b.ReleaseDate)
                            .ThenBy(b => b.Title);
            foreach (var book in books)
            {
                Console.WriteLine($"{book.Title} {book.ReleaseDate} {book.Copies}");
            }
        }

        private static void GetAuthorsByNumberOfBooks(BookShopContext ctx)
        {
            var authors = ctx.Authors
                                .Select(a => new
                                {
                                    a.FirstName,
                                    a.LastName,
                                    BooksCount = a.Books.Count
                                })
                                .OrderByDescending(a => a.BooksCount);

            foreach (var author in authors)
            {
                Console.WriteLine($"{author.FirstName} {author.LastName} - {author.BooksCount} books");
            }
        }

        private static void GetAuthorsWithBooksBefore1990(BookShopContext ctx)
        {
            var authorsOfBooks = ctx.Books
                                        .Where(b => b.ReleaseDate.Value.Year < 1990)
                                        .Select(b => new
                                        {
                                            b.Author.FirstName,
                                            b.Author.LastName
                                        })
                                        .Distinct();

            foreach (var author in authorsOfBooks)
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
