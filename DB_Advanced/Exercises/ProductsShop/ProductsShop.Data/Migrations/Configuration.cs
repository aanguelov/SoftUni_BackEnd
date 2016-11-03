namespace ProductsShop.Data.Migrations
{
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using System.Web.Script.Serialization;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<ProductsShopContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
            this.ContextKey = "ProductsShop.Data.ProductsShopContext";
        }

        protected override void Seed(ProductsShopContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            if (!context.Users.Any())
            {
                this.AddUsersToDatabase(context);
            }

            if (!context.Products.Any())
            {
                this.AddProductsToDatabase(context);
            }

            if (!context.Categories.Any())
            {
                this.AddCategoriesToDatabase(context);
                
            }
            //this.RandomlyConnectCategoriesToProducts(context);

        }

        private void RandomlyConnectCategoriesToProducts(ProductsShopContext context)
        {
            var random = new Random();
            var productsCount = context.Products.Count();
            var categoriesCount = context.Categories.Count();

            for (int i = 0; i < productsCount; i++)
            {
                var product = context.Products.Find(i + 1);
                var categoriesPerProduct = random.Next(1, (categoriesCount + 1)/3);

                for (int j = 0; j < categoriesPerProduct; j++)
                {
                    var categoryId = random.Next(1, categoriesCount + 1);
                    var category = context.Categories.Find(categoryId);

                    if (!product.Categories.Contains(category))
                    {
                        product.Categories.Add(category);
                        category.Products.Add(product);
                    }
                }
            }
        }

        private void AddCategoriesToDatabase(ProductsShopContext context)
        {
            using (var reader = new StreamReader("../../../InsertionData/categories.json"))
            {
                var json = reader.ReadToEnd();
                var serializer = new JavaScriptSerializer();

                var categories = serializer.Deserialize<List<Category>>(json);

                foreach (var category in categories)
                {
                    context.Categories.Add(category);
                }
            }
        }

        private void AddProductsToDatabase(ProductsShopContext context)
        {
            using (StreamReader reader = new StreamReader("../../../InsertionData/products.json"))
            {
                var json = reader.ReadToEnd();
                var serializer = new JavaScriptSerializer();
                Random random = new Random();
                var users = context.Users.ToList();

                var products = serializer.Deserialize<List<Product>>(json);

                foreach (var product in products)
                {
                    var userIndexSeller = random.Next(0, users.Count());
                    var sellerId = users[userIndexSeller].Id;

                    product.SellerId = sellerId;

                    var userIndexBuyer = random.Next(0 - users.Count() / 2, users.Count());

                    if (userIndexBuyer > 0 && userIndexBuyer != userIndexSeller)
                    {
                        product.BuyerId = users[userIndexBuyer].Id;
                    }

                    context.Products.Add(product);
                }
            }
        }

        private void AddUsersToDatabase(ProductsShopContext context)
        {
            XDocument usersXml = XDocument.Load("../../../InsertionData/users.xml");

            foreach (var user in usersXml.Descendants("user"))
            {
                var userToAdd = new User();
                foreach (var attribute in user.Attributes())
                {
                    if (attribute.Name == "first-name")
                    {
                        userToAdd.FirstName = attribute.Value;
                    }

                    if (attribute.Name == "last-name")
                    {
                        userToAdd.LastName = attribute.Value;
                    }

                    if (attribute.Name == "age")
                    {
                        userToAdd.Age = int.Parse(attribute.Value);
                    }
                }

                context.Users.Add(userToAdd);
            }
        }
    }
}
