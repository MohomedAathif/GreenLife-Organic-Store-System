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
    public partial class OrderTrackingForm : Form
    {
        private int _userId;
        List<Order> orders = new List<Order>();
        public OrderTrackingForm(int userId)
        {
            InitializeComponent();
            _userId = userId;
            LoadOrders();
        }
        private void LoadOrders()
        {
            orders.Clear();
            try
            {
                orders = Order.GetOrdersByUser(_userId);

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
        private void OrderTrackingForm_Load(object sender, EventArgs e)
        {

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            CustomerDashboardForm customerdashboard = new CustomerDashboardForm(_userId);
            customerdashboard.Show();
            this.Close();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadOrders();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            CustomerDashboardForm customerDashboard = new CustomerDashboardForm(_userId);
            customerDashboard.Show();
            this.Close();
        }

        private void btnSearchProducts_Click(object sender, EventArgs e)
        {
            SearchProductsForm form = new SearchProductsForm(_userId);
            form.Show();
            this.Close();
        }

        private void btnProfile_Click(object sender, EventArgs e)
        {
            CustomerProfileForm customerProfileForm = new CustomerProfileForm(_userId);
            customerProfileForm.Show();
            this.Close();
        }

        private void btnReview_Click(object sender, EventArgs e)
        {
            var deliveredProducts = Order.GetDeliveredProductsByUser(_userId);

            if (deliveredProducts.Count == 0)
            {
                MessageBox.Show("You can only review products that were delivered to you.");
                return;
            }

            ProductReviewForm reviewForm = new ProductReviewForm(_userId);
            reviewForm.Show();
            this.Close();
        }
    }
}
