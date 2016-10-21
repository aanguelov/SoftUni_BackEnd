using System;
using System.Data.SqlClient;

class GetVillainsNames
{
    static void Main()
    {
        string connectionString = "Server=.; Database=MinionsDB; Trusted_Connection=True;";
        SqlConnection connection = new SqlConnection(connectionString);
        connection.Open();

        using (connection)
        {
            string villansAndTheirMinions = "SELECT v.VillainName, Count(vm.MinionId) AS NumberOfMinions " +
                                              "FROM Villains AS v " +
                                             "INNER JOIN [dbo].[VillainsMinions] AS vm " +
                                                "ON v.VillainId = vm.VillainId " +
                                             "GROUP BY v.VillainName " +
                                             "ORDER BY Count(vm.MinionId) DESC";
            SqlCommand command = new SqlCommand(villansAndTheirMinions, connection);
            SqlDataReader reader = command.ExecuteReader();

            while(reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Console.Write(reader[i] + " "); 
                }
                Console.WriteLine();
            }
        }
    }
}
