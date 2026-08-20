using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace GreenLifeOrganicStore
{
    public partial class SearchProductsForm : Form
    {
        private int _userId;
        List<Product> products = new List<Product>();
        List<CartItem> cart = new List<CartItem>();

        public SearchProductsForm(int userId)
        {
            InitializeComponent();
            _userId = userId;
            LoadProducts();
            LoadCategories();
        }
        private void LoadProducts()
        {
            try
            {
                products = Product.GetAllProducts();
                LoadGrid(products);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading products: " + ex.Message);
            }
        }

        private void LoadCategories()
        {
            cmbCategory.Items.Clear();
            cmbCategory.Items.Add("All");

            foreach (var cat in products.Select(p => p.Category).Distinct())
            {
                cmbCategory.Items.Add(cat);
            }

            cmbCategory.SelectedIndex = 0;
        }

        private void LoadGrid(List<Product> list)
        {
            dgvProducts.DataSource = null;
            dgvProducts.DataSource = list;

            dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvProducts.RowHeadersVisible = false;
            dgvProducts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProducts.ScrollBars = ScrollBars.Both;

            dgvProducts.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvProducts.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.ToLower();
            string category = cmbCategory.SelectedItem.ToString();

            decimal minPrice = 0;
            decimal maxPrice = decimal.MaxValue;

            if (!string.IsNullOrWhiteSpace(txtMinPrice.Text))
            {
                if (!decimal.TryParse(txtMinPrice.Text, out minPrice))
                {
                    MessageBox.Show("Invalid minimum price");
                    return;
                }
            }

            if (!string.IsNullOrWhiteSpace(txtMaxPrice.Text))
            {
                if (!decimal.TryParse(txtMaxPrice.Text, out maxPrice))
                {
                    MessageBox.Show("Invalid maximum price");
                    return;
                }
            }

            var results = products.Where(p =>
                (string.IsNullOrEmpty(keyword) || p.Name.ToLower().Contains(keyword)) &&
                (category == "All" || p.Category == category) &&
                (p.FinalPrice >= minPrice && p.FinalPrice <= maxPrice)
            ).ToList();

            LoadGrid(results);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            txtMinPrice.Clear();
            txtMaxPrice.Clear();
            cmbCategory.SelectedIndex = 0;
            LoadGrid(products);
        }

        private void btnAddToCart_Click(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null) return;

            Product selected = dgvProducts.CurrentRow.DataBoundItem as Product;

            if (selected == null) return;

            int quantity = (int)numericquantity.Value;

            if (quantity > selected.Stock)
            {
                MessageBox.Show("Not enough stock available.");
                return;
            }

            if (selected.Stock <= 0)
            {
                MessageBox.Show("Product is out of stock");
                return;
            }

            CartItem item = new CartItem
            {
                Product = selected,
                Quantity = quantity
            };

            CartManager.Cart.Add(item);
            MessageBox.Show("Added to cart");
        }
        private void btnViewCart_Click(object sender, EventArgs e)
        {
            CartForm cartForm = new CartForm(_userId, CartManager.Cart);
            cartForm.Show();
            this.Close();
        }
        private void SearchProductsForm_Load(object sender, EventArgs e)
        {
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            CustomerDashboardForm customerDashboard = new CustomerDashboardForm(_userId);
            customerDashboard.Show();
            this.Close();
        }

        private void btnSearchProducts_Click(object sender, EventArgs e)
        {

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

        private void btnHome_Click_1(object sender, EventArgs e)
        {
            CustomerDashboardForm customerDashboard = new CustomerDashboardForm(_userId);
            customerDashboard.Show();
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
