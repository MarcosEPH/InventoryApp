using System.Data;
using MySql.Data.MySqlClient;
using InventoryApp.Database;

namespace InventoryApp.Repositories
{
    public class ClientRepository
    {
        public DataTable GetAll()
        {
            var dt = new DataTable();
            using var conn = Db.GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT id, nombre, nit, correo, telefono, direccion FROM cliente";
            conn.Open();
            using var adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(dt);
            return dt;
        }

        public int Insert(string nombre, string nit, string correo, string telefono, string direccion)
        {
            using var conn = Db.GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO cliente (nombre, nit, correo, telefono, direccion) VALUES (@n, @nit, @c, @t, @d)";
            cmd.Parameters.AddWithValue("@n", nombre);
            cmd.Parameters.AddWithValue("@nit", nit);
            cmd.Parameters.AddWithValue("@c", correo);
            cmd.Parameters.AddWithValue("@t", telefono);
            cmd.Parameters.AddWithValue("@d", direccion);
            conn.Open();
            return cmd.ExecuteNonQuery();
        }

        public int Update(int id, string nombre, string nit, string correo, string telefono, string direccion)
        {
            using var conn = Db.GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE cliente SET nombre=@n, nit=@nit, correo=@c, telefono=@t, direccion=@d WHERE id=@id";
            cmd.Parameters.AddWithValue("@n", nombre);
            cmd.Parameters.AddWithValue("@nit", nit);
            cmd.Parameters.AddWithValue("@c", correo);
            cmd.Parameters.AddWithValue("@t", telefono);
            cmd.Parameters.AddWithValue("@d", direccion);
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            return cmd.ExecuteNonQuery();
        }

        public int Delete(int id)
        {
            using var conn = Db.GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM cliente WHERE id=@id";
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            return cmd.ExecuteNonQuery();
        }
    }
}
