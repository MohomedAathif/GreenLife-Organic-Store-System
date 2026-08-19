using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GreenLifeOrganicStore;
using Microsoft.VisualBasic.ApplicationServices;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace GreenLifeOrganicStore
{
    public partial class CustomerDashboardForm : Form
    {
        private int _userId;

        public CustomerDashboardForm(int userId)
        {
            InitializeComponent();
            _userId = userId;
            LoadCustomerDashboard();
        }

        private void LoadCustomerDashboard()
        {
            try
            {
                int totalOrders = Order.GetOrderCountByUser(_userId);
                decimal totalSpent = Order.GetTotalSpentByUser(_userId);

                lblTotalOrdersValue.Text = totalOrders.ToString();
                lblTotalSpentValue.Text = "Rs. " + totalSpent.ToString("N2");


                lblCartItemsValue.Text = CartManager.GetCartCount().ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading dashboard: " + ex.Message);
            }
        }

        private void CustomerDashboardForm_Load(object sender, EventArgs e)
        {

        }

        private void btnSearchProducts_Click(object sender, EventArgs e)
        {
            SearchProductsForm form = new SearchProductsForm(_userId);
            form.Show();
            this.Close();
        }

        private void btnViewOrders_Click(object sender, EventArgs e)
        {
            OrderTrackingForm form = new OrderTrackingForm(_userId);
            form.Show();
            this.Close();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Close();
        }

        private void btnProfile_Click(object sender, EventArgs e)
        {
            CustomerProfileForm customerProfileForm = new CustomerProfileForm(_userId);
            customerProfileForm.Show();
            this.Close();
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void CustomerDashboardForm_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

        }
    }
}
