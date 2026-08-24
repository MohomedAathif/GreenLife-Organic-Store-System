using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Data.SqlClient;

namespace GreenLifeOrganicStore
{
    public partial class ProductReviewForm : Form
    {
        private int _userId;
        public ProductReviewForm(int userId)
        {
            InitializeComponent();
            _userId = userId;

            LoadRatings();
            LoadPurchasedProducts();
        }

        private void LoadPurchasedProducts()
        {
            List<Product> deliveredProducts = Order.GetDeliveredProductsByUser(_userId);

            cmbProduct.DataSource = null;
            cmbProduct.DataSource = deliveredProducts;
            cmbProduct.DisplayMember = "Name";
            cmbProduct.ValueMember = "ProductId";

            if (deliveredProducts.Count == 0)
            {
                MessageBox.Show("You can only review products that were delivered to you.");
                btnSubmit.Enabled = false;
                return;
            }
            cmbProduct.SelectedIndex = 0;
            int firstProductId = (int)cmbProduct.SelectedValue;
            LoadReviews(firstProductId);
            ValidateReviewEligibility();
        }
        private void ValidateReviewEligibility()
        {
            if (cmbProduct.SelectedValue == null)
            {
                btnSubmit.Enabled = false;
                return;
            }

            int selectedProductId = (int)cmbProduct.SelectedValue;

            if (ProductReview.HasUserReviewed(_userId, selectedProductId))
            {
                btnSubmit.Enabled = false;
                return;
            }

            btnSubmit.Enabled = true;
        }
        private void cmbProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProduct.SelectedValue == null) return;

            int selectedProductId = (int)cmbProduct.SelectedValue;

            LoadReviews(selectedProductId);
            ValidateReviewEligibility();
        }
        private void LoadRatings()
        {
            cmbRating.Items.Add("1 - Poor");
            cmbRating.Items.Add("2 - Fair");
            cmbRating.Items.Add("3 - Good");
            cmbRating.Items.Add("4 - Very Good");
            cmbRating.Items.Add("5 - Excellent");
        }
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (cmbProduct.SelectedValue == null)
            {
                MessageBox.Show("Select a product");
                return;
            }

            if (cmbRating.SelectedItem == null)
            {
                MessageBox.Show("Select rating");
                return;
            }

            int selectedProductId = (int)cmbProduct.SelectedValue;

            ProductReview review = new ProductReview
            {
                ProductId = selectedProductId,
                UserId = _userId,
                Rating = int.Parse(cmbRating.SelectedItem.ToString().Substring(0, 1)),
                Comment = txtComment.Text
            };

            review.AddReview();

            MessageBox.Show("Review submitted successfully");

            LoadReviews(selectedProductId);
            ValidateReviewEligibility();
        }

        private void LoadReviews(int productId)
        {
            dgvReviews.DataSource = ProductReview.GetReviewsByProduct(productId);

            double avg = ProductReview.GetAverageRating(productId);
            lblAverageRating.Text = "Average Rating: " + avg.ToString("0.0") + " / 5";

            if (dgvReviews.Columns.Contains("ReviewId"))
                dgvReviews.Columns["ReviewId"].Visible = false;

            if (dgvReviews.Columns.Contains("ProductId"))
                dgvReviews.Columns["ProductId"].Visible = false;

            if (dgvReviews.Columns.Contains("UserId"))
                dgvReviews.Columns["UserId"].Visible = false;

            dgvReviews.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvReviews.RowHeadersVisible = false;
            dgvReviews.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            cmbRating.SelectedIndex = -1;
            txtComment.Clear();
        }
        private void ProductReviewForm_Load(object sender, EventArgs e)
        {

        }
        private void cmbRating_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lblAverageRating_Click(object sender, EventArgs e)
        {

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

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            SearchProductsForm form = new SearchProductsForm(_userId);
            form.Show();
            this.Close();
        }
    }
}
