using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace GreenLifeOrganicStore
{
    public partial class CartForm : Form
    {

        private int _userId;
        List<CartItem> _cart;

        public CartForm(int userId, List<CartItem> cart)
        {
            InitializeComponent();
            _userId = userId;
            _cart = cart;
            LoadCart();
        }
        private void LoadCart()
        {
            dgvCart.Columns.Clear();
            dgvCart.DataSource = null;
            dgvCart.DataSource = _cart;

            dgvCart.Columns["Product"].Visible = false;

            dgvCart.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvCart.RowHeadersVisible = false;
            dgvCart.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCart.ScrollBars = ScrollBars.Both;

            dgvCart.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCart.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            decimal total = _cart.Sum(i => i.SubTotal);
            lblTotal.Text = "Total: Rs. " + total;
        }

        private void CartForm_Load(object sender, EventArgs e)
        {

        }

        private void btnRemoveItem_Click(object sender, EventArgs e)
        {
            if (dgvCart.CurrentRow == null) return;
            CartItem selected = dgvCart.CurrentRow.DataBoundItem as CartItem;
            if (selected == null) return;

            _cart.Remove(selected);
            LoadCart();
        }
        private void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            if (_cart.Count == 0)
            {
                MessageBox.Show("Cart is empty");
                return;
            }

            try
            {
                Order.PlaceOrder(_userId, _cart);

                _cart.Clear();
                LoadCart();

                MessageBox.Show("Order placed successfully");
                SearchProductsForm search = new SearchProductsForm(_userId);
                search.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error placing order: " + ex.Message);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            SearchProductsForm searchProducts = new SearchProductsForm(_userId);
            searchProducts.Show();
            this.Close();
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

        private void btnViewOrders_Click(object sender, EventArgs e)
        {
            OrderTrackingForm form = new OrderTrackingForm(_userId);
            form.Show();
            this.Close();
        }

        private void btnProfile_Click(object sender, EventArgs e)
        {
            CustomerProfileForm customerProfileForm = new CustomerProfileForm(_userId);
            customerProfileForm.Show();
            this.Close();
        }
    }
}
