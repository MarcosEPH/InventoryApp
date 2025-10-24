using System;
using System.Data;
using MySql.Data.MySqlClient;
using InventoryApp.Database;

namespace InventoryApp.Repositories
{
    public class SaleRepository
    {
        public DataTable GetSalesMaster()
        {
            return GetSalesMasterFiltered(null, DateTime.MinValue, DateTime.MaxValue);
        }

        public DataTable GetSalesMasterFiltered(int? clientId, DateTime from, DateTime to)
        {
            var dt = new DataTable();
            using var conn = Db.GetConnection();
            using var cmd = conn.CreateCommand();
            var sql = @"SELECT v.id AS VentaID, c.nombre AS Cliente, v.fecha, v.total
                         FROM venta v
                         JOIN cliente c ON v.cliente_id = c.id
                         WHERE v.fecha BETWEEN @from AND @to";
            if (clientId != null) sql += " AND v.cliente_id = @clientId";
            sql += " ORDER BY v.fecha DESC";
            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("@from", from);
            cmd.Parameters.AddWithValue("@to", to);
            if (clientId != null) cmd.Parameters.AddWithValue("@clientId", clientId.Value);
            conn.Open();
            using var adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(dt);
            return dt;
        }

        public DataTable GetSaleDetails(int ventaId)
        {
            var dt = new DataTable();
            using var conn = Db.GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT dv.venta_id AS VentaID, p.nombre AS Producto, dv.cantidad, dv.precio_unit, dv.subtotal
                                FROM detalle_venta dv
                                JOIN producto p ON dv.producto_id = p.id
                                WHERE dv.venta_id = @ventaId";
            cmd.Parameters.AddWithValue("@ventaId", ventaId);
            conn.Open();
            using var adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(dt);
            return dt;
        }
    }
}
