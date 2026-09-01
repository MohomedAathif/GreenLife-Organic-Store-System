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
    public partial class ManageOrdersForm : Form
    {
        List<Order> orders = new List<Order>();
        public ManageOrdersForm()
        {
            InitializeComponent();
            LoadStatusOptions();
            LoadOrders();
        }

        private void LoadStatusOptions()
        {
            cmbStatus.Items.Clear();
            cmbStatus.Items.AddRange(new string[]
            {
                "Pending",
                "Confirmed",
                "Processing",
                "Shipped",
                "Delivered"
            });
        }
        private void LoadOrders()
        {
            orders.Clear();

            try
            {
                orders = Order.GetAllOrders();

                dgvOrders.DataSource = null;
                dgvOrders.DataSource = orders;

                dgvOrders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvOrders.RowHeadersVisible = false;
                dgvOrders.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                dgvOrders.ScrollBars = ScrollBars.Both;

                dgvOrders.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvOrders.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading orders: " + ex.Message);
            }
        }
        private void ManageOrdersForm_Load(object sender, EventArgs e)
        {

        }
        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void btnUpdateStatus_Click(object sender, EventArgs e)
        {
            if (dgvOrders.CurrentRow == null || cmbStatus.SelectedItem == null)
            {
                MessageBox.Show("Please select an order and a status");
                return;
            }

            try
            {
                Order selected = dgvOrders.CurrentRow.DataBoundItem as Order;
                if (selected == null) return;

                selected.Status = cmbStatus.SelectedItem.ToString();
                selected.UpdateOrderStatus();

                LoadOrders();
                MessageBox.Show("Order status updated successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating status: " + ex.Message);
            }
        }

        private void dgvOrders_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvOrders.CurrentRow == null) return;

            Order selected = dgvOrders.CurrentRow.DataBoundItem as Order;
            cmbStatus.SelectedItem = selected.Status;
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

        private void btnReports_Click(object sender, EventArgs e)
        {
            ReportsForm form = new ReportsForm();
            form.Show();
            this.Close();
        }
    }
}
