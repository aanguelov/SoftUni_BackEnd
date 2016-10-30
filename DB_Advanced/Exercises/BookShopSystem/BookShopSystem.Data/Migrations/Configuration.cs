namespace BookShopSystem.Data.Migrations
{
    using Models;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;
    using System;
    using System.Globalization;

    internal sealed class Configuration : DbMigrationsConfiguration<BookShopContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
            this.ContextKey = "BookShopSystem.Data.BookShopContext";
        }

        protected override void Seed(BookShopContext context)
        {
            if (!context.Authors.Any())
            {
                this.AddAuthorsToDatabase(context);
            }

            if (!context.Categories.Any())
            {
                this.AddCategoriesToDatabase(context);
            }

            if (!context.Books.Any())
            {
                this.AddBooksToDatabase(context);
                this.RandomlyAddCategoriesToBook(context);
            }
        }

        private void RandomlyAddCategoriesToBook(BookShopContext context)
        {
            Random random = new Random();
            for (int i = 0; i < context.Books.Count(); i++)
            {
                int categoriesPerBook = random.Next(1, 5);
                for (int j = 0; j < categoriesPerBook; j++)
                {
                    int categoryId = random.Next(1, 9);
                    if (!context.Books.Find(i + 1).Categories.Contains(context.Categories.Find(categoryId)))
                    {
                        context.Books.Find(i + 1).Categories.Add(context.Categories.Find(categoryId));
                        context.Categories.Find(categoryId).Books.Add(context.Books.Find(i + 1));
                    }
                }
            }
        }

        private void AddBooksToDatabase(BookShopContext context)
        {
            using (var reader = new StreamReader("../../../InsertionData/books.txt"))
            {
                var line = reader.ReadLine();
                line = reader.ReadLine();

                while (line != null)
                {
                    var bookData = line.Split(new[] { ' ' }, 6);

                    var editionType = (EditionType)int.Parse(bookData[0]);
                    var releaseDate = DateTime.ParseExact(bookData[1], "d/M/yyyy", CultureInfo.InvariantCulture);
                    var copies = int.Parse(bookData[2]);
                    var price = decimal.Parse(bookData[3]);
                    var ageRestriction = (AgeRestriction)int.Parse(bookData[4]);
                    var title = bookData[5];

                    Random random = new Random();
                    var authors = context.Authors.ToArray();
                    var authorIndex = random.Next(0, authors.Length);
                    var authorId = authors[authorIndex].Id;

                    context.Books.Add(new Book()
                    {
                        Title = title,
                        EditionType = editionType,
                        Price = price,
                        Copies = copies,
                        ReleaseDate = releaseDate,
                        AuthorId = authorId,
                        AgeRestriction = ageRestriction
                    });

                    line = reader.ReadLine();
                }
            }
        }

        private void AddCategoriesToDatabase(BookShopContext context)
        {
            using (var reader = new StreamReader("../../../InsertionData/categories.txt"))
            {
                var line = reader.ReadLine();

                while (line != null)
                {
                    if (line != "")
                    {
                        context.Categories.Add(new Category()
                        {
                            Name = line
                        });
                    }

                    line = reader.ReadLine();
                }
            }
        }

        private void AddAuthorsToDatabase(BookShopContext context)
        {
            using (var reader = new StreamReader("../../../InsertionData/authors.txt"))
            {
                var line = reader.ReadLine();

                while (line != null)
                {
                    var authorData = line.Split(new[] { ' ' }, 2);
                    var authorFirstName = authorData[0];
                    var authorLastName = authorData[1];

                    context.Authors.Add(new Author()
                    {
                        FirstName = authorFirstName,
                        LastName = authorLastName
                    });

                    line = reader.ReadLine();
                }
            }
        }
    }
}
