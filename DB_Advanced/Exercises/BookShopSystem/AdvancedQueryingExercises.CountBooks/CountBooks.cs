namespace AdvancedQueryingExercises.CountBooks
{
    using BookShopSystem.Data;
    using System;
    using System.Linq;

    public class CountBooks
    {
        static void Main()
        {
            var ctx = new BookShopContext();
            var input = int.Parse(Console.ReadLine());

            var booksCount = ctx.Books
                .Where(b => b.Title.Length > input)
                .Count();

            Console.WriteLine(booksCount);
        }
    }
}
