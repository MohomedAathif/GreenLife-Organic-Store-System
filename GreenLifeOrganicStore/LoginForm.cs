using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace GreenLifeOrganicStore
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please enter email and password");
                return;
            }

            using (SqlConnection con = DbConnection.GetConnection())
            {
                try
                {
                    User loggedUser = User.Login(
                        txtEmail.Text.Trim(),
                        txtPassword.Text.Trim()
                    );

                    if (loggedUser != null)
                    {
                        this.Hide();

                        if (loggedUser.IsAdmin())
                        {
                            AdminDashboardForm adminForm = new AdminDashboardForm();
                            adminForm.Show();
                        }
                        else if (loggedUser.IsCustomer())
                        {
                            CustomerDashboardForm customerForm =
                                new CustomerDashboardForm(loggedUser.UserId);
                            customerForm.Show();
                        }
                        else
                        {
                            MessageBox.Show("Unknown user role");
                            this.Show();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid login details");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Login error: " + ex.Message);
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
            chkShowPassword.Text = chkShowPassword.Checked ? "Hide Password" : "Show Password";
        }
    }
}

