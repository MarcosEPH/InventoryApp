using System;
using System.Data;
using System.Windows.Forms;
using InventoryApp.Repositories;

namespace InventoryApp.Forms
{
    public class ProductsForm : Form
    {
        private DataGridView dgv;
        private Button btnRefresh, btnSaveChanges, btnDelete, btnNew;
        private ProductRepository repo = new ProductRepository();
        private BindingSource bs = new BindingSource();

        public ProductsForm()
        {
            Text = "Productos - Gestión";
            Width = 700;
            Height = 450;
            StartPosition = FormStartPosition.CenterParent;

            dgv = new DataGridView() { Dock = DockStyle.Top, Height = 320, AutoGenerateColumns = true };
            btnRefresh = new Button() { Text = "Refrescar", Left = 10, Top = 330, Width = 100 };
            btnSaveChanges = new Button() { Text = "Guardar Cambios", Left = 120, Top = 330, Width = 140 };
            btnDelete = new Button() { Text = "Eliminar", Left = 270, Top = 330, Width = 100 };
            btnNew = new Button() { Text = "Nuevo", Left = 380, Top = 330, Width = 100 };

            btnRefresh.Click += (s,e) => LoadData();
            btnSaveChanges.Click += async (s,e) => SaveChanges();
            btnDelete.Click += async (s,e) => DeleteSelected();
            btnNew.Click += (s,e) => NewProduct();

            Controls.Add(dgv);
            Controls.Add(btnRefresh);
            Controls.Add(btnSaveChanges);
            Controls.Add(btnDelete);
            Controls.Add(btnNew);

            Load += (s,e) => LoadData();
        }

        private void LoadData()
        {
            var dt = repo.GetAll();
            bs.DataSource = dt;
            dgv.DataSource = bs;
            if (dgv.Columns.Contains("creado_en")) dgv.Columns["creado_en"].Visible = false;
        }

        private void NewProduct()
        {
            var frm = new Form() { Text = "Nuevo Producto", Width = 350, Height = 220, StartPosition = FormStartPosition.CenterParent };
            var lblName = new Label() { Text = "Nombre", Left = 10, Top = 20 };
            var txtName = new TextBox() { Left = 100, Top = 20, Width = 200 };
            var lblPrice = new Label() { Text = "Precio", Left = 10, Top = 60 };
            var txtPrice = new TextBox() { Left = 100, Top = 60, Width = 200 };
            var lblStock = new Label() { Text = "Stock", Left = 10, Top = 100 };
            var txtStock = new TextBox() { Left = 100, Top = 100, Width = 200 };
            var btnOk = new Button() { Text = "Agregar", Left = 100, Top = 140, Width = 80 };
            btnOk.Click += (s,e) => {
                if (string.IsNullOrWhiteSpace(txtName.Text)) { MessageBox.Show("Nombre es obligatorio"); return; }
                if (!decimal.TryParse(txtPrice.Text, out var price) || price < 0) { MessageBox.Show("Precio inválido"); return; }
                if (!int.TryParse(txtStock.Text, out var stock) || stock < 0) { MessageBox.Show("Stock inválido"); return; }
                repo.Insert(txtName.Text.Trim(), price, stock);
                MessageBox.Show("Producto agregado");
                frm.Close();
                LoadData();
            };
            frm.Controls.AddRange(new Control[]{lblName, txtName, lblPrice, txtPrice, lblStock, txtStock, btnOk});
            frm.ShowDialog();
        }

        private void SaveChanges()
        {
            dgv.EndEdit();
            if (bs.DataSource is DataTable dt)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (row.RowState == DataRowState.Modified)
                    {
                        int id = Convert.ToInt32(row["id"]);
                        string nombre = row["nombre"].ToString();
                        if (string.IsNullOrWhiteSpace(nombre)) { MessageBox.Show("Nombre obligatorio"); return; }
                        if (!decimal.TryParse(row["precio"].ToString(), out var precio) || precio < 0) { MessageBox.Show("Precio inválido"); return; }
                        if (!int.TryParse(row["stock"].ToString(), out var stock) || stock < 0) { MessageBox.Show("Stock inválido"); return; }
                        repo.Update(id, nombre, precio, stock);
                    }
                }
                MessageBox.Show("Cambios guardados");
                LoadData();
            }
        }

        private void DeleteSelected()
        {
            if (dgv.CurrentRow == null) return;
            var idObj = dgv.CurrentRow.Cells[0].Value;
            if (idObj == null) return;
            int id = Convert.ToInt32(idObj);
            var ok = MessageBox.Show("¿Eliminar producto seleccionado?", "Confirmar", MessageBoxButtons.YesNo);
            if (ok == DialogResult.Yes)
            {
                repo.Delete(id);
                LoadData();
            }
        }
    }
}
