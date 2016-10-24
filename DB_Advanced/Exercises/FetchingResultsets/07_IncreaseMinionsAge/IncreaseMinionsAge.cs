using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _07_IncreaseMinionsAge
{
    // Changed the condition of the problem a little because all the names in the database are title case allready.
    // So this program makes them letter case instead. If you want title case, change LOWER to UPPER on line 33 
    class IncreaseMinionsAge
    {
        static void Main()
        {
            string[] minionIds = Console.ReadLine().Split(' ').ToArray();

            string connectionString = "Server=.; Database=MinionsDB; Trusted_Connection=True;";
            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();

            using (connection)
            {
                IncreaseAgeOfMinions(minionIds, connection);
            }
        }

        private static void IncreaseAgeOfMinions(string[] minionIds, SqlConnection connection)
        {
            string joinedIds = string.Join(", ", minionIds);
            string updateAgeOfMinions = "UPDATE Minions " +
                                           "SET Age = Age + 1, Name = LOWER(LEFT(Name, 1)) + RIGHT(Name, LEN(Name) - 1) " +
                                        $"WHERE MinionId IN ({joinedIds})";

            SqlTransaction transaction = connection.BeginTransaction();
            SqlCommand updateCommand = new SqlCommand(updateAgeOfMinions, connection, transaction);

            updateCommand.ExecuteNonQuery();

            PrintMinions(connection, transaction);
        }

        private static void PrintMinions(SqlConnection connection, SqlTransaction transaction)
        {
            string selectMinions = "SELECT Name, Age FROM Minions";
            SqlCommand command = new SqlCommand(selectMinions, connection, transaction);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine($"{reader[0]} {reader[1]}");
            }

            reader.Close();
            transaction.Rollback();
        }
    }
}
