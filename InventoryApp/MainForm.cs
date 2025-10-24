using System;
using System.Windows.Forms;

namespace InventoryApp
{
    public class MainForm : Form
    {
        private Button btnProducts;
        private Button btnClients;
        private Button btnSales;

        public MainForm()
        {
            Text = "Inventario - Menú (versión net8.0)";
            Width = 420;
            Height = 180;
            StartPosition = FormStartPosition.CenterScreen;

            btnProducts = new Button() { Text = "Productos", Left = 30, Top = 30, Width = 110 };
            btnClients = new Button() { Text = "Clientes", Left = 150, Top = 30, Width = 110 };
            btnSales = new Button() { Text = "Ventas", Left = 270, Top = 30, Width = 110 };

            btnProducts.Click += (s,e) => new Forms.ProductsForm().ShowDialog();
            btnClients.Click += (s,e) => new Forms.ClientsForm().ShowDialog();
            btnSales.Click += (s,e) => new Forms.SalesForm().ShowDialog();

            Controls.Add(btnProducts);
            Controls.Add(btnClients);
            Controls.Add(btnSales);
        }
    }
}
