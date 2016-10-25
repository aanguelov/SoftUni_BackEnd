using System.Data.SqlClient;

namespace MiniORM
{
    public class ConnectionStringBuilder
    {
        private SqlConnectionStringBuilder builder;

        private string connectionString;

        public ConnectionStringBuilder(string databaseName)
        {
            this.builder = new SqlConnectionStringBuilder();
            builder["Data Source"] = ".";
            builder["Integrated Security"] = true;
            builder["Connect Timeout"] = 1000;
            builder["Trusted_Conection"] = true;
            builder["Initial Catalog"] = databaseName;
            this.connectionString = builder.ToString();
        }

        public string ConnectionString
        {
            get { return this.connectionString; }
        }
    }
}
