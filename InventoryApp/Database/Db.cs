using System.Configuration;
using MySql.Data.MySqlClient;

namespace InventoryApp.Database
{
    public static class Db
    {
        public static string ConnectionString =>
            ConfigurationManager.ConnectionStrings["InventarioDb"].ConnectionString;

        public static MySqlConnection GetConnection() => new MySqlConnection(ConnectionString);
    }
}
