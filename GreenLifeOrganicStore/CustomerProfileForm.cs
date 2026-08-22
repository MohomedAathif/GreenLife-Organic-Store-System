using Microsoft.VisualBasic.ApplicationServices;
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
    public partial class CustomerProfileForm : Form
    {
        private int _userId;
        private User _user;
        public CustomerProfileForm(int userId)
        {
            InitializeComponent();
            _userId = userId;
            LoadProfile();
        }
        private void LoadProfile()
        {
            try
            {
                _user = User.GetUserById(_userId);

                if (_user == null) return;

                txtFirstName.Text = _user.FirstName;
                txtLastName.Text = _user.LastName;
                txtEmail.Text = _user.Email;
                txtAddress.Text = _user.Address;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading profile: " + ex.Message);
            }
        }
        private void CustomerProfileForm_Load(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            CustomerDashboardForm customerDashboard = new CustomerDashboardForm(_userId);
            customerDashboard.Show();
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                MessageBox.Show("First name and last name are required");
                return;
            }

            try
            {
                _user.FirstName = txtFirstName.Text.Trim();
                _user.LastName = txtLastName.Text.Trim();
                _user.Address = txtAddress.Text.Trim();

                _user.UpdateProfile();

                bool changePassword =
                    !string.IsNullOrWhiteSpace(txtCurrentPassword.Text) ||
                    !string.IsNullOrWhiteSpace(txtNewPassword.Text) ||
                    !string.IsNullOrWhiteSpace(txtConfirmPassword.Text);

                if (changePassword)
                {
                    if (txtNewPassword.Text != txtConfirmPassword.Text)
                    {
                        MessageBox.Show("New passwords do not match");
                        return;
                    }

                    bool success = _user.ChangePassword(
                        txtCurrentPassword.Text,
                        txtNewPassword.Text
                    );

                    if (!success)
                    {
                        MessageBox.Show("Current password is incorrect");
                        return;
                    }
                }

                txtCurrentPassword.Clear();
                txtNewPassword.Clear();
                txtConfirmPassword.Clear();

                MessageBox.Show("Profile updated successfully");

                CustomerDashboardForm dashboard = new CustomerDashboardForm(_userId);
                dashboard.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating profile: " + ex.Message);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            CustomerDashboardForm customerDashboard = new CustomerDashboardForm(_userId);
            customerDashboard.Show();
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

        private void btnLogout_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Close();
        }
    }
}
