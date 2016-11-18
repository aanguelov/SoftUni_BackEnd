namespace AdvancedQueryingExercises.AuthorsSearch
{
    using BookShopSystem.Data;
    using System;
    using System.Linq;

    public class AuthorsSearch
    {
        static void Main()
        {
            var ctx = new BookShopContext();
            var input = Console.ReadLine();

            var authorsNames = ctx.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .Select(a => a.FirstName + " " + a.LastName)
                .ToList();

            authorsNames.ForEach(a =>
            {
                Console.WriteLine(a);
            });
        }
    }
}
