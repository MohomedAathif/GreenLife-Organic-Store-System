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

namespace GreenLifeOrganicStore
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))

            {
                MessageBox.Show("Email and password are required");
                return;
            }

            using (SqlConnection con = DbConnection.GetConnection())
            {
                try
                {
                    User newUser = new User
                    {
                        FirstName = txtFirstName.Text,
                        LastName = txtLastName.Text,
                        Email = txtEmail.Text.Trim(),
                        Password = txtPassword.Text.Trim(),
                        Address = txtAddress.Text
                    };

                    bool success = newUser.Register();

                    if (!success)
                    {
                        MessageBox.Show("Email already exists");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Registration error: " + ex.Message);
                }
            }

            MessageBox.Show("Registration successful");

            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Close();
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Close();
        }
        private void RegisterForm_Load(object sender, EventArgs e)
        {
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Close();
        }
    }
}
