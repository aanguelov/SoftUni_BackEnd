using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gringotts
{
    class Program
    {
        static void Main(string[] args)
        {
            GringottsContext ctx = new GringottsContext();

            using (ctx)
            {
                var wizardsFirstNames = ctx.WizzardDeposits
                                            .Where(d => d.DepositGroup == "Troll Chest")
                                            .Select(d => d.FirstName.Substring(0, 1))
                                            .Distinct()
                                            .OrderBy(s => s);
                foreach (var letter in wizardsFirstNames)
                {
                    Console.WriteLine(letter);
                }
            }
        }
    }
}
