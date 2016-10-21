using System;
using System.Data.SqlClient;
using System.Text;

class GetMinionNames
{
    static void Main()
    {
        int villainId = int.Parse(Console.ReadLine());
        string connectionString = "Server=.; Database=MinionsDB; Trusted_Connection=True;";
        SqlConnection connection = new SqlConnection(connectionString);
        connection.Open();

        using (connection)
        {
            string selectVillainWithMinions = "SELECT v.VillainName, m.Name, m.Age " +
                                                "FROM Minions AS m " +
                                               "INNER JOIN VillainsMinions AS vm " +
                                                  "ON m.MinionId = vm.MinionId " +
                                               "RIGHT JOIN Villains AS v " +
                                                  "ON v.VillainId = vm.VillainId " +
                                               "WHERE v.VillainId = @villainId";
            SqlCommand command = new SqlCommand(selectVillainWithMinions, connection);
            command.Parameters.AddWithValue("@villainId", villainId);
            SqlDataReader reader = command.ExecuteReader();

            StringBuilder result = new StringBuilder();
            int minionsCount = 1;

            reader.Read();

            if (reader.HasRows)
            {
                result.AppendLine($"Villain: {reader[0]}");
                if(reader[1].ToString() != "")
                {
                    result.AppendLine($"{minionsCount}. {reader[1]} {reader[2]}");

                    while (reader.Read())
                    {
                        minionsCount++;
                        result.AppendLine($"{minionsCount}. {reader[1]} {reader[2]}");
                    }
                }
                else
                {
                    result.AppendLine("<no minions>");
                }
                

                Console.WriteLine(result.ToString().TrimEnd());
            }
            else
            {
                Console.WriteLine($"No villain with ID {villainId} exists in the database.");
            }

        }
    }
}
