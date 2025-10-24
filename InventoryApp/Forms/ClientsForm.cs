using System;
using System.Data;
using System.Windows.Forms;
using InventoryApp.Repositories;

namespace InventoryApp.Forms
{
    public class ClientsForm : Form
    {
        private DataGridView dgv;
        private Button btnRefresh, btnAdd, btnEdit, btnDelete;
        private ClientRepository repo = new ClientRepository();

        public ClientsForm()
        {
            Text = "Clientes - CRUD";
            Width = 900;
            Height = 500;
            StartPosition = FormStartPosition.CenterParent;

            dgv = new DataGridView() { Dock = DockStyle.Top, Height = 350, ReadOnly = true, AutoGenerateColumns = true, SelectionMode = DataGridViewSelectionMode.FullRowSelect };
            btnRefresh = new Button() { Text = "Refrescar", Left = 10, Top = 360, Width = 100 };
            btnAdd = new Button() { Text = "Agregar", Left = 120, Top = 360, Width = 100 };
            btnEdit = new Button() { Text = "Editar", Left = 230, Top = 360, Width = 100 };
            btnDelete = new Button() { Text = "Eliminar", Left = 340, Top = 360, Width = 100 };

            btnRefresh.Click += (s,e) => LoadData();
            btnAdd.Click += (s,e) => OpenEditor();
            btnEdit.Click += (s,e) => { if (dgv.CurrentRow!=null) OpenEditor((int)dgv.CurrentRow.Cells[0].Value); };
            btnDelete.Click += async (s,e) => DeleteSelected();

            Controls.Add(dgv);
            Controls.Add(btnRefresh);
            Controls.Add(btnAdd);
            Controls.Add(btnEdit);
            Controls.Add(btnDelete);

            Load += (s,e) => LoadData();
        }

        private void LoadData()
        {
            dgv.DataSource = repo.GetAll();
            if (dgv.Columns.Contains("creado_en")) dgv.Columns["creado_en"].Visible = false;
        }

        private void OpenEditor(int? id = null)
        {
            var frm = new Form() { Width = 520, Height = 360, StartPosition = FormStartPosition.CenterParent };
            var lblNombre = new Label() { Text = "Nombre", Left = 10, Top = 20 };
            var txtNombre = new TextBox() { Left = 120, Top = 20, Width = 360 };
            var lblNit = new Label() { Text = "NIT", Left = 10, Top = 60 };
            var txtNit = new TextBox() { Left = 120, Top = 60, Width = 360 };
            var lblCorreo = new Label() { Text = "Correo", Left = 10, Top = 100 };
            var txtCorreo = new TextBox() { Left = 120, Top = 100, Width = 360 };
            var lblTel = new Label() { Text = "Teléfono", Left = 10, Top = 140 };
            var txtTel = new TextBox() { Left = 120, Top = 140, Width = 360 };
            var lblDir = new Label() { Text = "Dirección", Left = 10, Top = 180 };
            var txtDir = new TextBox() { Left = 120, Top = 180, Width = 360 };
            var btnOk = new Button() { Text = id==null? "Agregar" : "Guardar", Left = 120, Top = 220, Width = 100 };

            if (id != null)
            {
     
                var dt = repo.GetAll();
                foreach (DataRow r in dt.Rows)
                {
                    if (Convert.ToInt32(r["id"]) == id)
                    {
                        txtNombre.Text = r["nombre"].ToString();
                        txtNit.Text = r["nit"].ToString();
                        txtCorreo.Text = r["correo"].ToString();
                        txtTel.Text = r["telefono"].ToString();
                        txtDir.Text = r["direccion"].ToString();
                        break;
                    }
                }
            }

            btnOk.Click += (s,e) => {
                if (string.IsNullOrWhiteSpace(txtNombre.Text)) { MessageBox.Show("Nombre obligatorio"); return; }
                if (string.IsNullOrWhiteSpace(txtNit.Text)) { MessageBox.Show("NIT obligatorio"); return; }
                if (!string.IsNullOrWhiteSpace(txtCorreo.Text) && !txtCorreo.Text.Contains("@")) { MessageBox.Show("Correo inválido"); return; }

                if (id == null)
                {
                    repo.Insert(txtNombre.Text.Trim(), txtNit.Text.Trim(), txtCorreo.Text.Trim(), txtTel.Text.Trim(), txtDir.Text.Trim());
                    MessageBox.Show("Cliente agregado");
                }
                else
                {
                    repo.Update(id.Value, txtNombre.Text.Trim(), txtNit.Text.Trim(), txtCorreo.Text.Trim(), txtTel.Text.Trim(), txtDir.Text.Trim());
                    MessageBox.Show("Cliente actualizado");
                }
                frm.Close();
                LoadData();
            };

            frm.Controls.AddRange(new Control[] { lblNombre, txtNombre, lblNit, txtNit, lblCorreo, txtCorreo, lblTel, txtTel, lblDir, txtDir, btnOk });
            frm.ShowDialog();
        }

        private void DeleteSelected()
        {
            if (dgv.CurrentRow == null) return;
            int id = Convert.ToInt32(dgv.CurrentRow.Cells[0].Value);
            var ok = MessageBox.Show("¿Eliminar cliente seleccionado?", "Confirmar", MessageBoxButtons.YesNo);
            if (ok == DialogResult.Yes)
            {
                repo.Delete(id);
                LoadData();
            }
        }
    }
}
