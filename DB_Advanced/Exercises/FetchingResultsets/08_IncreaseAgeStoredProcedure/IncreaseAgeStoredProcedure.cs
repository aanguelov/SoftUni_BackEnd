//CREATE PROCEDURE usp_GetOlder (@MinionId INT)
//AS
//BEGIN

//    BEGIN TRAN

//   UPDATE Minions
//      SET Age = Age + 1
//    WHERE MinionId = @MinionId

//   SELECT Name, Age
//     FROM Minions
//    WHERE MinionId = @MinionId

// ROLLBACK
//END

using System;
using System.Data;
using System.Data.SqlClient;

namespace _08_IncreaseAgeStoredProcedure
{
    class IncreaseAgeStoredProcedure
    {
        static void Main()
        {
            int minionId = int.Parse(Console.ReadLine());

            string connectionString = "Server=.; Database=MinionsDB; Trusted_Connection=True;";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            using (connection)
            {
                SqlTransaction transaction = connection.BeginTransaction();
                SqlCommand command = new SqlCommand("usp_GetOlder", connection, transaction);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@minionId", minionId);

                SqlDataReader reader = command.ExecuteReader();

                reader.Read();
                Console.WriteLine($"{reader[0]} {reader[1]}");

                reader.Close();
            }
        }
    }
}
