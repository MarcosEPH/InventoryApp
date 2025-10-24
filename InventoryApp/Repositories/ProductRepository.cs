using System.Data;
using MySql.Data.MySqlClient;
using InventoryApp.Database;

namespace InventoryApp.Repositories
{
    public class ProductRepository
    {
        public DataTable GetAll()
        {
            var dt = new DataTable();
            using var conn = Db.GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT id, nombre, precio, stock FROM producto";
            conn.Open();
            using var adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(dt);
            return dt;
        }

        public int Insert(string nombre, decimal precio, int stock)
        {
            using var conn = Db.GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO producto (nombre, precio, stock) VALUES (@n, @p, @s)";
            cmd.Parameters.AddWithValue("@n", nombre);
            cmd.Parameters.AddWithValue("@p", precio);
            cmd.Parameters.AddWithValue("@s", stock);
            conn.Open();
            return cmd.ExecuteNonQuery();
        }

        public int Update(int id, string nombre, decimal precio, int stock)
        {
            using var conn = Db.GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE producto SET nombre=@n, precio=@p, stock=@s WHERE id=@id";
            cmd.Parameters.AddWithValue("@n", nombre);
            cmd.Parameters.AddWithValue("@p", precio);
            cmd.Parameters.AddWithValue("@s", stock);
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            return cmd.ExecuteNonQuery();
        }

        public int Delete(int id)
        {
            using var conn = Db.GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM producto WHERE id=@id";
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            return cmd.ExecuteNonQuery();
        }
    }
}
