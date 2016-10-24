using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _06_PrintAllMinionsNames
{
    class PrintAllMinionsNames
    {
        static void Main()
        {
            string connectionString = "Server=.; Database=MinionsDB; Trusted_Connection=True;";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            using (connection)
            {
                List<string> minionNames = GetMinionNames(connection);
                OrderMinionNames(minionNames);
                foreach (var name in minionNames)
                {
                    Console.WriteLine(name);
                }
            }

        }

        private static void OrderMinionNames(List<string> minionNames)
        {
            int length = minionNames.Count();
            int orders = 1;

            while (orders < length)
            {
                string current = minionNames[length - 1];
                minionNames.RemoveAt(length - 1);
                minionNames.Insert(orders, current);
                orders += 2;
            }
        }

        private static List<string> GetMinionNames(SqlConnection connection)
        {
            string selectNames = "SELECT Name FROM Minions";
            SqlCommand command = new SqlCommand(selectNames, connection);

            SqlDataReader reader = command.ExecuteReader();
            List<string> result = new List<string> { };

            while (reader.Read())
            {
                result.Add(reader[0].ToString());
            }

            reader.Close();

            return result;
        }
    }
}
