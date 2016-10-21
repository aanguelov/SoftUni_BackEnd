using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03_AddMinion
{
    class AddMinion
    {
        static void Main(string[] args)
        {
            string[] minionData = Console.ReadLine().Split(new[] { ':', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string[] villainData = Console.ReadLine().Split(new[] { ':', ' ' }, StringSplitOptions.RemoveEmptyEntries);

            string minionName = minionData[1];
            string minionAge = minionData[2];
            string minionTown = minionData[3];

            string villainName = villainData[1];

            string connectionString = "Server=.; Database=MinionsDB; Trusted_Connection=True;";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            using (connection)
            {
                if (!CheckIfTownExists(minionTown, connection))
                {
                    AddTownToDatabase(minionTown, connection);
                }

                if (!CheckIfVillainExists(villainName, connection))
                {
                    AddVillainToDatabase(villainName, connection);
                }

                if (!CheckIfMinionExists(minionName, connection))
                {
                    int townId = GetTownId(minionTown, connection);
                    AddMinionToDatabase(minionName, minionAge, townId, connection);
                }

                int minionId = GetMinionId(minionName, connection);
                int villainId = GetVillainId(villainName, connection);

                AddMinionToVillain(minionId, villainId, connection);
                Console.WriteLine($"Successfully added {minionName} to be minion of {villainName}.");
            }
        }

        private static void AddMinionToVillain(int minionId, int villainId, SqlConnection connection)
        {
            string addMinionToVillainStr = "INSERT INTO VillainsMinions VALUES (@villainId, @minionId)";

            SqlCommand command = new SqlCommand(addMinionToVillainStr, connection);
            command.Parameters.AddWithValue("@villainId", villainId);
            command.Parameters.AddWithValue("@minionId", minionId);

            command.ExecuteNonQuery();
        }

        private static int GetVillainId(string villainName, SqlConnection connection)
        {
            string selectTownByName = "SELECT VillainId " +
                                        "FROM Villains " +
                                       "WHERE VillainName = @villainName";
            int result = 0;

            SqlCommand command = new SqlCommand(selectTownByName, connection);
            command.Parameters.AddWithValue("@villainName", villainName);
            result = (int)command.ExecuteScalar();

            return result;
        }

        private static int GetMinionId(string minionName, SqlConnection connection)
        {
            string selectTownByName = "SELECT MinionId " +
                                        "FROM Minions " +
                                       "WHERE Name = @minionName";
            int result = 0;

            SqlCommand command = new SqlCommand(selectTownByName, connection);
            command.Parameters.AddWithValue("@minionName", minionName);
            result = (int)command.ExecuteScalar();

            return result;
        }

        private static int GetTownId(string minionTown, SqlConnection connection)
        {
            string selectTownByName = "SELECT TownId " +
                                        "FROM Towns " +
                                       "WHERE TownName = @minionTown";
            int result = 0;

            SqlCommand command = new SqlCommand(selectTownByName, connection);
            command.Parameters.AddWithValue("@minionTown", minionTown);
            result = (int)command.ExecuteScalar();

            return result;
        }

        private static void AddMinionToDatabase(string minionName, string minionAge, int townId, SqlConnection connection)
        {
            string insertTown = "INSERT INTO Minions (Name, Age, TownId) " +
                                "VALUES (@minionName, @minionAge, @townId)";

            SqlCommand command = new SqlCommand(insertTown, connection);
            command.Parameters.AddWithValue("@minionName", minionName);
            command.Parameters.AddWithValue("@minionAge", minionAge);
            command.Parameters.AddWithValue("@townId", townId);
            command.ExecuteNonQuery();
        }

        private static bool CheckIfMinionExists(string minionName, SqlConnection connection)
        {
            string selectTownByName = "SELECT COUNT(*) " +
                                        "FROM Minions " +
                                       "WHERE Name = @minionName";
            int result = 0;

            SqlCommand command = new SqlCommand(selectTownByName, connection);
            command.Parameters.AddWithValue("@minionName", minionName);
            result = (int)command.ExecuteScalar();

            return result > 0;
        }

        private static void AddVillainToDatabase(string villainName, SqlConnection connection)
        {
            string insertTown = "INSERT INTO Villains (VillainName, Evilness) " +
                                "VALUES (@villainName, 'evil')";

            SqlCommand command = new SqlCommand(insertTown, connection);
            command.Parameters.AddWithValue("@villainName", villainName);
            command.ExecuteNonQuery();
            Console.WriteLine($"Villain {villainName} was added to the database.");
        }

        private static bool CheckIfVillainExists(string villainName, SqlConnection connection)
        {
            string selectTownByName = "SELECT COUNT(*) " +
                                        "FROM Villains " +
                                       "WHERE VillainName = @villainName";
            int result = 0;

            SqlCommand command = new SqlCommand(selectTownByName, connection);
            command.Parameters.AddWithValue("@villainName", villainName);
            result = (int)command.ExecuteScalar();

            return result > 0;
        }

        private static void AddTownToDatabase(string minionTown, SqlConnection connection)
        {
            string insertTown = "INSERT INTO Towns (TownName) " +
                                "VALUES (@minionTown)";

            SqlCommand command = new SqlCommand(insertTown, connection);
            command.Parameters.AddWithValue("@minionTown", minionTown);
            command.ExecuteNonQuery();
            Console.WriteLine($"Town {minionTown} was added to the database.");
        }

        private static bool CheckIfTownExists(string minionTown, SqlConnection connection)
        {
            string selectTownByName = "SELECT COUNT(*) " +
                                        "FROM Towns " +
                                       "WHERE TownName = @minionTown";
            int result = 0;

            SqlCommand command = new SqlCommand(selectTownByName, connection);
            command.Parameters.AddWithValue("@minionTown", minionTown);
            result = (int)command.ExecuteScalar();

            return result > 0;
        }
    }
}
