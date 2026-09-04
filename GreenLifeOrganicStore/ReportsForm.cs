using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GreenLifeOrganicStore
{
    public partial class ReportsForm : Form
    {
        public ReportsForm()
        {
            InitializeComponent();
            LoadCustomers();
        }
        private void LoadCustomers()
        {
            cmbCustomers.DataSource = User.GetAllCustomers();
            cmbCustomers.DisplayMember = "FullName";
            cmbCustomers.ValueMember = "UserId";

        }

        private void btnSalesReport_Click(object sender, EventArgs e)
        {

            try
            {
                List<Order> orders = Order.GetAllOrdersForReport();

                dgvReport.DataSource = null;
                dgvReport.DataSource = orders;

                dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvReport.RowHeadersVisible = false;
                dgvReport.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                dgvReport.ScrollBars = ScrollBars.Both;
                dgvReport.Columns["CustomerId"].Visible = false;

                dgvReport.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvReport.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                decimal totalSales = orders.Sum(o => o.TotalAmount);
                lblSummary.Text = "Total Sales: Rs. " + totalSales;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating sales report: " + ex.Message);
            }
        }

        private void btnStockReport_Click(object sender, EventArgs e)
        {

            try
            {
                List<Product> products = Product.GetAllProducts();

                var stockReport = products.Select(p => new
                {
                    p.ProductId,
                    p.Name,
                    p.Stock
                }).ToList();

                dgvReport.DataSource = null;
                dgvReport.DataSource = stockReport;
                dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvReport.RowHeadersVisible = false;
                dgvReport.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                dgvReport.ScrollBars = ScrollBars.Both;

                dgvReport.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvReport.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                foreach (DataGridViewRow row in dgvReport.Rows)
                {
                    if (row.DataBoundItem is Product product)
                    {
                        if (product.Stock < 5)
                        {
                            row.DefaultCellStyle.BackColor = Color.LightCoral;
                        }
                    }
                }
                lblSummary.Text = "All current stock shown";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating stock report: " + ex.Message);
            }
        }

        private void btnCustomerOrders_Click(object sender, EventArgs e)
        {
            if (cmbCustomers.SelectedValue == null)
                return;

            int userId = (int)cmbCustomers.SelectedValue;

            dgvReport.DataSource = Order.GetOrdersByUser(userId);

            dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvReport.Columns["CustomerId"].Visible = false;

            dgvReport.Columns["TotalAmount"].DefaultCellStyle.Format = "Rs. 0.00";
        }
        private void btnExportCSV_Click(object sender, EventArgs e)
        {
            if (dgvReport.Rows.Count == 0) return;

            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                StringBuilder sb = new StringBuilder();

                // Headers
                foreach (DataGridViewColumn col in dgvReport.Columns)
                {
                    sb.Append(col.HeaderText + ",");
                }
                sb.AppendLine();

                // Rows
                foreach (DataGridViewRow row in dgvReport.Rows)
                {
                    if (row.DataBoundItem == null) continue;

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        sb.Append(cell.Value + ",");
                    }
                    sb.AppendLine();
                }

                File.WriteAllText(sfd.FileName, sb.ToString());
                MessageBox.Show("Report exported successfully");
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            AdminDashboardForm adminDashboard = new AdminDashboardForm();
            adminDashboard.Show();
            this.Close();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            AdminDashboardForm adminDashboard = new AdminDashboardForm();
            adminDashboard.Show();
            this.Hide();
        }

        private void btnManageCustomers_Click(object sender, EventArgs e)
        {
            ManageCustomersForm form = new ManageCustomersForm();
            form.Show();
            this.Close();
        }

        private void btnManageProducts_Click(object sender, EventArgs e)
        {
            ManageProductsForm form = new ManageProductsForm();
            form.Show();
            this.Close();
        }

        private void btnManageOrders_Click(object sender, EventArgs e)
        {
            ManageOrdersForm form = new ManageOrdersForm();
            form.Show();
            this.Close();
        }
    }
}
