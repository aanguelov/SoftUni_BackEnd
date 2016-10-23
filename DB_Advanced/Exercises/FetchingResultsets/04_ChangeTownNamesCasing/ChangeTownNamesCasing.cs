using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _04_ChangeTownNamesCasing
{
    class ChangeTownNamesCasing
    {
        static void Main()
        {
            string countryName = Console.ReadLine();
            string connectionStr = "Server=.; Database=MinionsDB; Trusted_Connection=True;";
            SqlConnection connection = new SqlConnection(connectionStr);
            connection.Open();

            using (connection)
            {
                int countryId = GetCountryId(countryName, connection);
                if (countryId > 0)
                {
                    int updatedTowns = UpdateTownsCasing(countryId, connection);

                    if (updatedTowns > 0)
                    {
                        string[] townNames = GetTownNamesByCountryId(countryId, updatedTowns, connection);
                        Console.WriteLine($"{updatedTowns} town names were affected.");
                        Console.WriteLine("[" + string.Join(", ", townNames) + "]");
                    }
                    else
                    {
                        Console.WriteLine("No town names were affected.");
                    }
                }
                else
                {
                    Console.WriteLine("No town names were affected.");
                }
            }
        }

        private static string[] GetTownNamesByCountryId(int countryId, int townsCount, SqlConnection connection)
        {
            string getTownsStr = "SELECT TownName FROM Towns WHERE CountryId = @countryId";
            SqlCommand command = new SqlCommand(getTownsStr, connection);
            command.Parameters.AddWithValue("@countryId", countryId);

            SqlDataReader reader = command.ExecuteReader();
            string[] result = new string[townsCount];
            int index = 0;

            while (reader.Read())
            {        
                result[index] = reader[0].ToString();
                index++;
            }

            return result;
        }

        private static int UpdateTownsCasing(int countryId, SqlConnection connection)
        {
            string updateCasingStr = "UPDATE Towns SET TownName = UPPER(TownName) WHERE CountryId = @countryId";
            SqlCommand command = new SqlCommand(updateCasingStr, connection);
            command.Parameters.AddWithValue("@countryId", countryId);

            return command.ExecuteNonQuery();
        }

        private static int GetCountryId(string countryName, SqlConnection connection)
        {
            if (!CheckIfCountryExists(countryName, connection))
            {
                return -1;
            }

            string getCountryIdStr = "SELECT CountryId FROM Countries WHERE CountryName = @countryName";
            SqlCommand command = new SqlCommand(getCountryIdStr, connection);
            command.Parameters.AddWithValue("@countryName", countryName);

            return (int)command.ExecuteScalar();
        }

        private static bool CheckIfCountryExists(string countryName, SqlConnection connection)
        {
            string checkForCountryStr = "SELECT COUNT(*) FROM Countries WHERE CountryName = @countryName";
            SqlCommand command = new SqlCommand(checkForCountryStr, connection);
            command.Parameters.AddWithValue("@countryName", countryName);

            return (int)command.ExecuteScalar() > 0;
        }
    }
}
