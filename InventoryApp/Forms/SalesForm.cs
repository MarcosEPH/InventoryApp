using System;
using System.Data;
using System.Windows.Forms;
using InventoryApp.Repositories;
using MySql.Data.MySqlClient;

namespace InventoryApp.Forms
{
    public class SalesForm : Form
    {
        private DataGridView dgvMaster, dgvDetail;
        private ComboBox cmbClients;
        private DateTimePicker dtFrom, dtTo;
        private Button btnFilter, btnClear;
        private SaleRepository repo = new SaleRepository();

        public SalesForm()
        {
            Text = "Ventas - Visualización";
            Width = 1000;
            Height = 600;
            StartPosition = FormStartPosition.CenterParent;

            var panelTop = new Panel() { Dock = DockStyle.Top, Height = 40 };
            cmbClients = new ComboBox() { Left = 10, Top = 8, Width = 250 };
            dtFrom = new DateTimePicker() { Left = 270, Top = 8, Width = 140 };
            dtTo = new DateTimePicker() { Left = 420, Top = 8, Width = 140 };
            btnFilter = new Button() { Text = "Filtrar", Left = 570, Top = 6, Width = 80 };
            btnClear = new Button() { Text = "Limpiar", Left = 660, Top = 6, Width = 80 };

            panelTop.Controls.AddRange(new Control[]{cmbClients, dtFrom, dtTo, btnFilter, btnClear});
            Controls.Add(panelTop);

            var split = new SplitContainer() { Dock = DockStyle.Fill, Orientation = Orientation.Horizontal };
            dgvMaster = new DataGridView() { Dock = DockStyle.Fill, ReadOnly = true, AutoGenerateColumns = true };
            dgvDetail = new DataGridView() { Dock = DockStyle.Fill, ReadOnly = true, AutoGenerateColumns = true };
            split.Panel1.Controls.Add(dgvMaster);
            split.Panel2.Controls.Add(dgvDetail);
            Controls.Add(split);

            btnFilter.Click += (s,e) => LoadMaster();
            btnClear.Click += (s,e) => { cmbClients.SelectedIndex = -1; dtFrom.Value = DateTime.Today.AddMonths(-1); dtTo.Value = DateTime.Today; LoadMaster(); };
            dgvMaster.SelectionChanged += DgvMaster_SelectionChanged;

            Load += (s,e) => {
                LoadClientsList();
                dtFrom.Value = DateTime.Today.AddMonths(-1);
                dtTo.Value = DateTime.Today;
                LoadMaster();
            };
        }

        private void LoadClientsList()
        {
            var dt = new InventoryApp.Repositories.ClientRepository().GetAll();
            var tbl = new DataTable();
            tbl.Columns.Add("id", typeof(int));
            tbl.Columns.Add("nombre", typeof(string));
            foreach (DataRow r in dt.Rows) tbl.Rows.Add(Convert.ToInt32(r["id"]), r["nombre"].ToString());
            cmbClients.DataSource = tbl;
            cmbClients.DisplayMember = "nombre";
            cmbClients.ValueMember = "id";
            cmbClients.SelectedIndex = -1;
        }

        private void LoadMaster()
        {
            // simple filter by client and date range
            int? clientId = cmbClients.SelectedIndex >= 0 ? (int?)Convert.ToInt32(((DataRowView)cmbClients.SelectedItem)["id"]) : null;
            DateTime from = dtFrom.Value.Date;
            DateTime to = dtTo.Value.Date.AddDays(1).AddSeconds(-1);

            var dt = repo.GetSalesMasterFiltered(clientId, from, to);
            dgvMaster.DataSource = dt;
        }

        private void DgvMaster_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvMaster.CurrentRow == null) return;
            var idObj = dgvMaster.CurrentRow.Cells[0].Value;
            if (idObj == null) return;
            int ventaId = Convert.ToInt32(idObj);
            dgvDetail.DataSource = repo.GetSaleDetails(ventaId);
        }
    }
}
