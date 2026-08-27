using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using GreenLifeOrganicStore;

namespace GreenLifeOrganicStore
{
    public partial class AdminDashboardForm : Form
    {
        public AdminDashboardForm()
        {
            InitializeComponent();
        }

        private void AdminDashboardForm_Load(object sender, EventArgs e)
        {
            LoadDashboardData();
        }
        private void LoadDashboardData()
        {
            try
            {
                int productCount = Product.GetProductCount();
                int orderCount = Order.GetOrderCount();
                decimal totalSales = Order.GetTotalSales();
                int customerCount = User.GetCustomerCount();
                int lowStockCount = Product.GetLowStockCount();

                lblProductsValue.Text = productCount.ToString();
                lblOrdersValue.Text = orderCount.ToString();
                lblSalesValue.Text = totalSales.ToString("N2");
                lblCustomersValue.Text = customerCount.ToString();
                lblLowStockValue.Text = lowStockCount.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading dashboard data: " + ex.Message);
            }
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

        private void btnLogout_Click(object sender, EventArgs e)
        {
            LoginForm login = new LoginForm();
            login.Show();
            this.Close();
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Close();
        }
    }
}
