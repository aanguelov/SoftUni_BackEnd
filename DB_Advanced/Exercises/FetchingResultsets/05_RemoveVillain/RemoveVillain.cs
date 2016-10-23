using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _05_RemoveVillain
{
    class RemoveVillain
    {
        static void Main()
        {
            int villainId = int.Parse(Console.ReadLine());

            string connectionString = "Server=.; Database=MinionsDB; Trusted_Connection=True;";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            using (connection)
            {
                string[] villainData = GetVillainInfoById(villainId, connection);
                if (villainData[0] != null)
                {
                    if (ReleaseMinions(villainId, connection))
                    {
                        if (DeleteVillainFromDB(villainId, connection))
                        {
                            Console.WriteLine($"{villainData[0]} was deleted");
                            Console.WriteLine($"{villainData[1]} minions released");
                        }
                    }
                    
                }
                else
                {
                    Console.WriteLine("No such villain was found");
                }
            }
        }

        private static bool DeleteVillainFromDB(int villainId, SqlConnection connection)
        {
            string deleteFromVillains = "DELETE FROM Villains WHERE VillainId = @villainId";
            SqlCommand command = new SqlCommand(deleteFromVillains, connection);
            command.Parameters.AddWithValue("@villainId", villainId);

            SqlTransaction transaction = connection.BeginTransaction();
            command.Transaction = transaction;

            try
            {
                command.ExecuteNonQuery();
                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return false;
            }
        }

        private static bool ReleaseMinions(int villainId, SqlConnection connection)
        {
            string deleteFromVillainsMinionsStr = "DELETE FROM VillainsMinions WHERE VillainId = @villainId";
            SqlCommand command = new SqlCommand(deleteFromVillainsMinionsStr, connection);
            command.Parameters.AddWithValue("@villainId", villainId);

            SqlTransaction transaction = connection.BeginTransaction();
            command.Transaction = transaction;

            try
            {
                command.ExecuteNonQuery();
                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return false;
            }
        }

        private static string[] GetVillainInfoById(int villainId, SqlConnection connection)
        {
            string selectVillainStr = "SELECT v.VillainName, COUNT(vm.MinionId) " + 
                                        "FROM Villains AS v " +  
                                        "LEFT JOIN VillainsMinions AS vm " + 
                                          "ON v.VillainId = vm.VillainId " +  
                                       "WHERE v.VillainId = @villainId " + 
                                       "GROUP BY v.VillainName";
            SqlCommand command = new SqlCommand(selectVillainStr, connection);
            command.Parameters.AddWithValue("@villainId", villainId);

            SqlDataReader reader = command.ExecuteReader();
            string[] villainInfo = new string[reader.FieldCount];

            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    villainInfo[i] = reader[i].ToString();
                }
            }
            reader.Close();

            return villainInfo;
        }
    }
}
