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
using GreenLifeOrganicStore;

namespace GreenLifeOrganicStore
{
    public partial class ManageCustomersForm : Form
    {
        public ManageCustomersForm()
        {
            InitializeComponent();
            LoadCustomers();
        }

        private void ManageCustomersForm_Load(object sender, EventArgs e)
        {

        }
        private void LoadCustomers()
        {
            try
            {
                List<User> customers = User.GetAllCustomers();

                dgvCustomers.DataSource = null;
                dgvCustomers.DataSource = customers;

                dgvCustomers.Columns["UserId"].ReadOnly = true;

                dgvCustomers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvCustomers.RowHeadersVisible = false;
                dgvCustomers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                dgvCustomers.ScrollBars = ScrollBars.Both;

                dgvCustomers.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvCustomers.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading customers: " + ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            {
                try
                {
                    foreach (DataGridViewRow row in dgvCustomers.Rows)
                    {
                        if (row.IsNewRow) continue;

                        User customer = new User
                        {
                            UserId = Convert.ToInt32(row.Cells["UserId"].Value),
                            FirstName = row.Cells["FirstName"].Value?.ToString(),
                            LastName = row.Cells["LastName"].Value?.ToString(),
                            Address = row.Cells["Address"].Value?.ToString(),
                            Email = row.Cells["Email"].Value?.ToString(),
                            Password = row.Cells["Password"].Value?.ToString(),
                            Role = "Customer"
                        };

                        customer.UpdateCustomer();
                    }

                    MessageBox.Show("Customer details updated successfully");
                    LoadCustomers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Update failed: " + ex.Message);
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            AdminDashboardForm adminDashboard = new AdminDashboardForm();
            adminDashboard.Show();
            this.Close();
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

        private void btnManageProducts_Click(object sender, EventArgs e)
        {
            ManageProductsForm manageProducts = new ManageProductsForm();
            manageProducts.Show();
            this.Close();
        }

        private void btnManageOrders_Click(object sender, EventArgs e)
        {
            ManageOrdersForm manageOrders = new ManageOrdersForm();
            manageOrders.Show();
            this.Close();
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            ReportsForm reportsform = new ReportsForm();
            reportsform.Show();
            this.Close();
        }

        private void btnManageCustomers_Click(object sender, EventArgs e)
        {

        }
    }
}
