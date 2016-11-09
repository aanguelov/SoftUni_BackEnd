namespace BookShopSystem.ConsoleClient
{
    using Data;
    using Models;
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Xml.Linq;

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

                //ExportBooksAsXml(ctx);

                //ExtractAuthorsWithTheirBooksXml(ctx);

                var relatedBooks = ctx.Books
                    .Where(b => b.RelatedBooks.Count > 0)
                    .Select(b => new
                    {
                        b.Title,
                        RelatedBooksTitles = b.RelatedBooks.Select(rb => new
                        {
                            rb.Title
                        })
                    })
                    .OrderByDescending(b => b.RelatedBooksTitles.Count());

                foreach (var book in relatedBooks)
                {
                    Console.WriteLine($"--{book.Title}");
                    foreach (var related in book.RelatedBooksTitles)
                    {
                        Console.WriteLine(related.Title);
                    }
                }

            }
        }

        private static void ExtractAuthorsWithTheirBooksXml(BookShopContext ctx)
        {
            var authors = ctx.Authors
                                .Where(a => a.Books.Count(b => b.ReleaseDate.Value.Year < 1990) > 0)
                                .Select(a => new
                                {
                                    FullName = a.FirstName + " " + a.LastName,
                                    Books = a.Books.Where(b => b.ReleaseDate.Value.Year < 1990).Select(b => new
                                    {
                                        b.Title,
                                        b.Price
                                    })
                                })
                                .ToList();

            var authorWithBooksXml = new XElement("authors",
                    from author in authors
                    select new XElement("author",
                        new XAttribute("name", author.FullName),
                        new XElement("books",
                            from book in author.Books
                            select new XElement("book", 
                                new XAttribute("title", book.Title), 
                                new XAttribute("price", book.Price)
                            )
                        )
                    )
                );
            authorWithBooksXml.Save("../../../authorsWithBooksBefore1990.txt");
        }

        private static void ExportBooksAsXml(BookShopContext ctx)
        {
            var books = ctx.Books
                            .Where(b => b.AgeRestriction == AgeRestriction.Minor)
                            .Select(b => new
                            {
                                Author = b.Author.FirstName + " " + b.Author.LastName,
                                b.Title
                            })
                            .ToList();
            var booksToXml = new XElement("kidsBooks",
                    from book in books
                    select new XElement("book",
                        new XElement("author", book.Author),
                        new XElement("title", book.Title)
                    )
                );
            booksToXml.Save("../../../kidsBooks.txt");
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
