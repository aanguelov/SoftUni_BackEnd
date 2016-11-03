namespace ProductsShop.ConsoleCilent
{
    using Data;
    using Models;
    using System;
    using System.Linq;
    using System.Xml.Linq;
    using Newtonsoft.Json;
    using System.IO;

    public class ConsoleClient
    {
        static void Main()
        {
            var ctx = new ProductsShopContext();

            using (ctx)
            {
                //GetProductsInRange(ctx);

                //GetSuccessfullySoldProducts(ctx);

                //GetCategoriesByProductCount(ctx);

                GetUsersAndProducts(ctx);
                
            }
        }

        private static void GetUsersAndProducts(ProductsShopContext ctx)
        {
            var users = ctx.Users
                .Where(u => u.SellerProducts.Count(p => p.BuyerId != null) > 0)
                .Select(u => new
                {
                    u.FirstName,
                    u.LastName,
                    u.Age,
                    Products = u.SellerProducts
                                    .Where(p => p.BuyerId != null)
                                    .Select(p => new
                                    {
                                        p.Name,
                                        p.Price
                                    })
                })
                .OrderByDescending(u => u.Products.Count())
                .ThenBy(u => u.LastName)
                .ToList();

            var usersWithProductsXml = new XElement("users",
                    new XAttribute("count", users.Count()),
                    from user in users
                    select new XElement("user",
                        from property in user.GetType().GetProperties().Where(p => p.GetValue(user) != null && p.Name != "Products")
                        select new XAttribute(property.Name.ToLower(), property.GetValue(user)),
                        new XElement("sold-products", 
                            new XAttribute("count", user.Products.Count()),
                            from product in user.Products
                            select new XElement("product", 
                                new XAttribute("name", product.Name),
                                new XAttribute("price", product.Price)
                            )
                        )
                    )
                );

            usersWithProductsXml.Save("../../users-and-products.xml");
        }

        private static void GetCategoriesByProductCount(ProductsShopContext ctx)
        {
            var categories = ctx.Categories.Select(c => new
            {
                category = c.Name,
                productsCount = c.Products.Count(),
                averagePrice = c.Products.Average(p => p.Price),
                totalRevenue = c.Products.Sum(p => p.Price)
            }).OrderByDescending(c => c.productsCount);

            string categoriesToJson = JsonConvert.SerializeObject(categories, Formatting.Indented);
            File.WriteAllText(@"../../categories-by-products.json", categoriesToJson);
        }

        private static void GetSuccessfullySoldProducts(ProductsShopContext ctx)
        {
            var users = ctx.Users
                .Where(u => u.SellerProducts.Count(p => p.BuyerId != null) > 0)
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    soldProducts = u.SellerProducts
                                        .Where(p => p.BuyerId != null)
                                        .Select(p => new
                                        {
                                            name = p.Name,
                                            price = p.Price,
                                            buyerFirstName = p.Buyer.FirstName,
                                            buyerLastName = p.Buyer.LastName
                                        })
                })
                .OrderBy(u => u.lastName)
                .ThenBy(u => u.firstName);

            string usersToJson = JsonConvert.SerializeObject(users, Formatting.Indented);
            File.WriteAllText(@"../../users-sold-products.json", usersToJson);
        }

        private static void GetProductsInRange(ProductsShopContext ctx)
        {
            var products = ctx.Products
                    .Where(p => p.BuyerId == null && p.Price > 500 && p.Price < 1000)
                    .Select(p => new
                    {
                        p.Name,
                        p.Price,
                        Seller = p.Seller.FirstName + " " + p.Seller.LastName
                    })
                    .OrderBy(p => p.Price);

            string productsToJson = JsonConvert.SerializeObject(products, Formatting.Indented);
            File.WriteAllText(@"../../products-in-range.json", productsToJson);
        }
    }
}
