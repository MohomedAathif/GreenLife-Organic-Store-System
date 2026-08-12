using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace GreenLifeOrganicStore
{
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string Role { get; set; }

        public string GetFullName()
        {
            return FirstName + " " + LastName;
        }

        public bool IsAdmin()
        {
            return Role == "Admin";
        }

        public bool IsCustomer()
        {
            return Role == "Customer";
        }

        // LOGIN METHOD
        public static User Login(string email, string password)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string sql = @"SELECT UserId, FirstName, LastName, Email, Address, Role
                               FROM Users
                               WHERE Email=@Email AND Password=@Password";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new User
                    {
                        UserId = (int)reader["UserId"],
                        FirstName = reader["FirstName"]?.ToString(),
                        LastName = reader["LastName"]?.ToString(),
                        Email = reader["Email"].ToString(),
                        Address = reader["Address"]?.ToString(),
                        Role = reader["Role"].ToString()
                    };
                }
            }

            return null;
        }

        // REGISTER METHOD
        public bool Register()
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                con.Open();

                // Check if email already exists
                string checkSql = "SELECT COUNT(*) FROM Users WHERE Email=@Email";
                SqlCommand checkCmd = new SqlCommand(checkSql, con);
                checkCmd.Parameters.AddWithValue("@Email", Email);

                int exists = (int)checkCmd.ExecuteScalar();

                if (exists > 0)
                {
                    return false;
                }

                // Insert new user
                string insertSql = @"INSERT INTO Users 
                             (FirstName, LastName, Email, Password, Address, Role)
                             VALUES (@FirstName, @LastName, @Email, @Password, @Address, @Role)";

                SqlCommand insertCmd = new SqlCommand(insertSql, con);
                insertCmd.Parameters.AddWithValue("@FirstName", FirstName ?? "");
                insertCmd.Parameters.AddWithValue("@LastName", LastName ?? "");
                insertCmd.Parameters.AddWithValue("@Email", Email);
                insertCmd.Parameters.AddWithValue("@Password", Password);
                insertCmd.Parameters.AddWithValue("@Address", Address ?? "");
                insertCmd.Parameters.AddWithValue("@Role", "Customer");

                insertCmd.ExecuteNonQuery();
            }

            return true;
        }

        public static List<User> GetAllCustomers()
        {
            List<User> customers = new List<User>();

            using (SqlConnection con = DbConnection.GetConnection())
            {
                string sql = @"SELECT UserId, FirstName, LastName, Address, Email, Password
                       FROM Users WHERE Role = 'Customer'";

                SqlCommand cmd = new SqlCommand(sql, con);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    customers.Add(new User
                    {
                        UserId = (int)reader["UserId"],
                        FirstName = reader["FirstName"]?.ToString(),
                        LastName = reader["LastName"]?.ToString(),
                        Address = reader["Address"]?.ToString(),
                        Email = reader["Email"].ToString(),
                        Password = reader["Password"].ToString(),
                        Role = "Customer"
                    });
                }
            }

            return customers;
        }

        public void UpdateCustomer()
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string sql = @"UPDATE Users SET
                       FirstName=@FirstName,
                       LastName=@LastName,
                       Address=@Address,
                       Email=@Email,
                       Password=@Password
                       WHERE UserId=@UserId";

                SqlCommand cmd = new SqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@FirstName", FirstName);
                cmd.Parameters.AddWithValue("@LastName", LastName);
                cmd.Parameters.AddWithValue("@Address", Address);
                cmd.Parameters.AddWithValue("@Email", Email);
                cmd.Parameters.AddWithValue("@Password", Password);
                cmd.Parameters.AddWithValue("@UserId", UserId);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public static User GetUserById(int userId)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string sql = @"SELECT UserId, FirstName, LastName, 
                       Email, Address, Role
                       FROM Users
                       WHERE UserId=@UserId";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@UserId", userId);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new User
                    {
                        UserId = (int)reader["UserId"],
                        FirstName = reader["FirstName"]?.ToString(),
                        LastName = reader["LastName"]?.ToString(),
                        Email = reader["Email"].ToString(),
                        Address = reader["Address"]?.ToString(),
                        Role = reader["Role"].ToString()
                    };
                }
            }

            return null;
        }

        public void UpdateProfile()
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string sql = @"UPDATE Users SET
                       FirstName=@FirstName,
                       LastName=@LastName,
                       Address=@Address
                       WHERE UserId=@UserId";

                SqlCommand cmd = new SqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@FirstName", FirstName);
                cmd.Parameters.AddWithValue("@LastName", LastName);
                cmd.Parameters.AddWithValue("@Address", Address);
                cmd.Parameters.AddWithValue("@UserId", UserId);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public bool ChangePassword(string currentPassword, string newPassword)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string checkSql = @"SELECT COUNT(*) FROM Users 
                            WHERE UserId=@UserId AND Password=@Password";

                SqlCommand checkCmd = new SqlCommand(checkSql, con);
                checkCmd.Parameters.AddWithValue("@UserId", UserId);
                checkCmd.Parameters.AddWithValue("@Password", currentPassword);

                con.Open();
                int valid = (int)checkCmd.ExecuteScalar();

                if (valid == 0)
                    return false;

                string updateSql = @"UPDATE Users 
                             SET Password=@Password 
                             WHERE UserId=@UserId";

                SqlCommand updateCmd = new SqlCommand(updateSql, con);
                updateCmd.Parameters.AddWithValue("@Password", newPassword);
                updateCmd.Parameters.AddWithValue("@UserId", UserId);

                updateCmd.ExecuteNonQuery();
            }

            return true;
        }
        public static int GetCustomerCount()
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string sql = "SELECT COUNT(*) FROM Users WHERE Role = 'Customer'";
                SqlCommand cmd = new SqlCommand(sql, con);

                con.Open();
                return (int)cmd.ExecuteScalar();
            }
        }
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }

    }
}
