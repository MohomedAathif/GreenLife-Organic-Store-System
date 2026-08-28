using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GreenLifeOrganicStore
{
    public partial class ManageProductsForm : Form
    {
        List<Product> products = new List<Product>();

        public ManageProductsForm()
        {
            InitializeComponent();
            LoadProducts();
        }

        private void LoadProducts()
        {
            products.Clear();

            try
            {
                products = Product.GetAllProducts();

                dgvProducts.DataSource = null;
                dgvProducts.DataSource = products;

                dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvProducts.RowHeadersVisible = false;
                dgvProducts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                dgvProducts.ScrollBars = ScrollBars.Both;

                dgvProducts.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvProducts.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                dgvProducts.Columns["Discount"].HeaderText = "Discount (%)";
                dgvProducts.Columns["Discount"].DefaultCellStyle.Format = "0'%'";


                CheckLowStock();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading products: " + ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs(out decimal price, out int stock, out double discount))
                return;

            try
            {
                Product p = new Product
                {
                    Name = txtProductName.Text,
                    Category = txtCategory.Text,
                    Price = price,
                    Stock = stock,
                    Supplier = txtSupplier.Text,
                    Discount = discount
                };

                p.AddProduct();

                LoadProducts();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding product: " + ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null) return;

            if (!ValidateInputs(out decimal price, out int stock, out double discount))
                return;

            Product selected = dgvProducts.CurrentRow.DataBoundItem as Product;

            selected.Name = txtProductName.Text;
            selected.Category = txtCategory.Text;
            selected.Price = price;
            selected.Stock = stock;
            selected.Supplier = txtSupplier.Text;
            selected.Discount = discount;

            selected.UpdateProduct();

            LoadProducts();
            ClearFields();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null) return;

            Product selected = dgvProducts.CurrentRow.DataBoundItem as Product;

            if (MessageBox.Show("Are you sure you want to delete this product?",
                "Confirm", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            selected.DeleteProduct();

            LoadProducts();
            ClearFields();
        }

        private void dgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvProducts.CurrentRow == null) return;

            Product selected = dgvProducts.CurrentRow.DataBoundItem as Product;

            txtProductName.Text = selected.Name;
            txtCategory.Text = selected.Category;
            txtPrice.Text = selected.Price.ToString();
            txtStock.Text = selected.Stock.ToString();
            txtSupplier.Text = selected.Supplier;
            txtDiscount.Text = selected.Discount.ToString();
        }

        private bool ValidateInputs(out decimal price, out int stock, out double discount)
        {
            price = 0;
            stock = 0;
            discount = 0;

            if (string.IsNullOrWhiteSpace(txtProductName.Text) ||
                string.IsNullOrWhiteSpace(txtCategory.Text))
            {
                MessageBox.Show("Product name and category are required");
                return false;
            }

            if (!decimal.TryParse(txtPrice.Text, out price))
            {
                MessageBox.Show("Invalid price");
                return false;
            }

            if (!int.TryParse(txtStock.Text, out stock))
            {
                MessageBox.Show("Invalid stock");
                return false;
            }

            if (!double.TryParse(txtDiscount.Text, out discount))
            {
                MessageBox.Show("Invalid discount");
                return false;
            }

            return true;
        }

        private void ClearFields()
        {
            txtProductName.Clear();
            txtCategory.Clear();
            txtPrice.Clear();
            txtStock.Clear();
            txtSupplier.Clear();
            txtDiscount.Clear();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void CheckLowStock()
        {
            if (products.Any(p => p.IsLowStock()))
            {
                MessageBox.Show("Warning: Some products are low in stock");
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            AdminDashboardForm adminDashboard = new AdminDashboardForm();
            adminDashboard.Show();
            this.Close();
        }

        private void dgvProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

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
