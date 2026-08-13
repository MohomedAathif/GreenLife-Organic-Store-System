using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLifeOrganicStore
{
    public interface IDiscountable
    {
        decimal CalculateFinalPrice();
    }
    public class Product : IDiscountable
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Supplier { get; set; }
        public double Discount { get; set; }

        public decimal FinalPrice => CalculateFinalPrice();

        public decimal CalculateFinalPrice()
        {
            return Price - (Price * (decimal)Discount / 100);
        }

        public bool IsLowStock()
        {
            return Stock < 5;
        }

        // ADD PRODUCT
        public void AddProduct()
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string sql = @"INSERT INTO Products 
                               (Name, Category, Price, Stock, Supplier, Discount)
                               VALUES (@Name, @Category, @Price, @Stock, @Supplier, @Discount)";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@Name", Name);
                cmd.Parameters.AddWithValue("@Category", Category);
                cmd.Parameters.AddWithValue("@Price", Price);
                cmd.Parameters.AddWithValue("@Stock", Stock);
                cmd.Parameters.AddWithValue("@Supplier", Supplier);
                cmd.Parameters.AddWithValue("@Discount", Discount);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // UPDATE PRODUCT
        public void UpdateProduct()
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string sql = @"UPDATE Products SET
                               Name=@Name,
                               Category=@Category,
                               Price=@Price,
                               Stock=@Stock,
                               Supplier=@Supplier,
                               Discount=@Discount
                               WHERE ProductId=@ProductId";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@ProductId", ProductId);
                cmd.Parameters.AddWithValue("@Name", Name);
                cmd.Parameters.AddWithValue("@Category", Category);
                cmd.Parameters.AddWithValue("@Price", Price);
                cmd.Parameters.AddWithValue("@Stock", Stock);
                cmd.Parameters.AddWithValue("@Supplier", Supplier);
                cmd.Parameters.AddWithValue("@Discount", Discount);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // DELETE PRODUCT
        public void DeleteProduct()
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string sql = "DELETE FROM Products WHERE ProductId=@ProductId";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@ProductId", ProductId);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // GET ALL PRODUCTS
        public static List<Product> GetAllProducts()
        {
            List<Product> products = new List<Product>();

            using (SqlConnection con = DbConnection.GetConnection())
            {
                string sql = "SELECT * FROM Products";
                SqlCommand cmd = new SqlCommand(sql, con);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    products.Add(new Product
                    {
                        ProductId = (int)reader["ProductId"],
                        Name = reader["Name"].ToString(),
                        Category = reader["Category"].ToString(),
                        Price = (decimal)reader["Price"],
                        Stock = (int)reader["Stock"],
                        Supplier = reader["Supplier"].ToString(),
                        Discount = Convert.ToDouble(reader["Discount"])
                    });
                }
            }
            return products;
        }

        public static int GetProductCount()
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string sql = "SELECT COUNT(*) FROM Products";
                SqlCommand cmd = new SqlCommand(sql, con);

                con.Open();
                return (int)cmd.ExecuteScalar();
            }
        }

        public static List<Product> GetLowStockProducts()
        {
            List<Product> products = new List<Product>();

            using (SqlConnection con = DbConnection.GetConnection())
            {
                string sql = "SELECT * FROM Products WHERE Stock < 5";
                SqlCommand cmd = new SqlCommand(sql, con);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    products.Add(new Product
                    {
                        ProductId = (int)reader["ProductId"],
                        Name = reader["Name"].ToString(),
                        Category = reader["Category"].ToString(),
                        Price = (decimal)reader["Price"],
                        Stock = (int)reader["Stock"],
                        Supplier = reader["Supplier"].ToString(),
                        Discount = Convert.ToDouble(reader["Discount"])
                    });
                }
            }

            return products;
        }
        public static int GetLowStockCount()
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string sql = "SELECT COUNT(*) FROM Products WHERE Stock < 5";
                SqlCommand cmd = new SqlCommand(sql, con);

                con.Open();
                return (int)cmd.ExecuteScalar();
            }
        }

        public double AverageRating
        {
            get
            {
                return ProductReview.GetAverageRating(ProductId);
            }
        }


    }
}
