namespace AdvancedQueryingExercises.FindProfit
{
    using BookShopSystem.Data;
    using System;
    using System.Linq;

    class FindProfit
    {
        static void Main()
        {
            var ctx = new BookShopContext();

            var categories = ctx.Categories
                .Select(c => new
                {
                    c.Name,
                    totalProfit = c.Books.Select(b => b.Copies * b.Price).Sum()
                });

            foreach (var category in categories)
            {
                Console.WriteLine($"{category.Name} - ${category.totalProfit}");
            }
        }
    }
}
