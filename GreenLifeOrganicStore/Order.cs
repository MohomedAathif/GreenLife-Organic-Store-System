using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Data.SqlClient;

namespace GreenLifeOrganicStore
{
    public class Order
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public List<Product> Products { get; set; }
        public string Status { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string CustomerName { get; set; }

        public Order()
        {
            Products = new List<Product>();
            Status = "Pending";
            OrderDate = DateTime.Now;
        }
        public void AddProduct(Product product)
        {
            Products.Add(product);
        }
        public decimal CalculateTotal()
        {
            return Products.Sum(p => p.FinalPrice);
        }
        public void UpdateStatus(string newStatus)
        {
            Status = newStatus;
        }


        public static int GetOrderCount()
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string sql = "SELECT COUNT(*) FROM Orders";
                SqlCommand cmd = new SqlCommand(sql, con);

                con.Open();
                return (int)cmd.ExecuteScalar();
            }
        }

        public static decimal GetTotalSales()
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string sql = "SELECT ISNULL(SUM(TotalAmount), 0) FROM Orders";
                SqlCommand cmd = new SqlCommand(sql, con);

                con.Open();
                return Convert.ToDecimal(cmd.ExecuteScalar());
            }
        }

        public static List<Order> GetAllOrders()
        {
            List<Order> orders = new List<Order>();

            using (SqlConnection con = DbConnection.GetConnection())
            {
                string sql = @"SELECT OrderId, UserId, OrderDate, 
                       TotalAmount, Status 
                       FROM Orders";

                SqlCommand cmd = new SqlCommand(sql, con);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    orders.Add(new Order
                    {
                        OrderId = (int)reader["OrderId"],
                        CustomerId = (int)reader["UserId"],
                        OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                        TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                        Status = reader["Status"].ToString()
                    });
                }
            }

            return orders;
        }

        public void UpdateOrderStatus()
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string sql = @"UPDATE Orders 
                       SET Status=@Status 
                       WHERE OrderId=@OrderId";

                SqlCommand cmd = new SqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@Status", Status);
                cmd.Parameters.AddWithValue("@OrderId", OrderId);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public static List<Order> GetAllOrdersForReport()
        {
            List<Order> orders = new List<Order>();

            using (SqlConnection con = DbConnection.GetConnection())
            {
                string sql = @"SELECT o.OrderId,
                              u.FirstName + ' ' + u.LastName AS FullName,
                              o.OrderDate,
                              o.TotalAmount,
                              o.Status
                       FROM Orders o
                       INNER JOIN Users u ON o.UserId = u.UserId";

                SqlCommand cmd = new SqlCommand(sql, con);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    orders.Add(new Order
                    {
                        OrderId = (int)reader["OrderId"],
                        CustomerName = reader["FullName"].ToString(),
                        OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                        TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                        Status = reader["Status"].ToString()
                    });
                }
            }

            return orders;
        }

        public static List<Order> GetOrdersByUser(int userId)
        {
            List<Order> orders = new List<Order>();

            using (SqlConnection con = DbConnection.GetConnection())
            {
                string sql = @"SELECT o.OrderId,
                              u.FirstName + ' ' + u.LastName AS FullName,
                              o.OrderDate,
                              o.TotalAmount,
                              o.Status
                       FROM Orders o
                       INNER JOIN Users u ON o.UserId = u.UserId
                       WHERE o.UserId = @UserId";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@UserId", userId);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    orders.Add(new Order
                    {
                        OrderId = (int)reader["OrderId"],
                        CustomerName = reader["FullName"].ToString(),
                        OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                        TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                        Status = reader["Status"].ToString()
                    });
                }
            }

            return orders;
        }

        public static void PlaceOrder(int userId, List<CartItem> cart)
        {
            if (cart == null || cart.Count == 0)
                throw new Exception("Cart is empty");

            using (SqlConnection con = DbConnection.GetConnection())
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();

                try
                {
                    decimal totalAmount = cart.Sum(i => i.SubTotal);

                    // Insert Order
                    string orderSql = @"INSERT INTO Orders 
                                (UserId, OrderDate, TotalAmount, Status)
                                OUTPUT INSERTED.OrderId
                                VALUES (@UserId, @Date, @Total, @Status)";

                    SqlCommand orderCmd = new SqlCommand(orderSql, con, transaction);

                    orderCmd.Parameters.AddWithValue("@UserId", userId);
                    orderCmd.Parameters.AddWithValue("@Date", DateTime.Now);
                    orderCmd.Parameters.AddWithValue("@Total", totalAmount);
                    orderCmd.Parameters.AddWithValue("@Status", "Pending");

                    int orderId = (int)orderCmd.ExecuteScalar();

                    foreach (CartItem item in cart)
                    {
                        string itemSql = @"INSERT INTO OrderItems 
                                   (OrderId, ProductId, Price, Quantity) VALUES (@OrderId, @ProductId, @Price, @Quantity)";

                        SqlCommand itemCmd = new SqlCommand(itemSql, con, transaction);

                        itemCmd.Parameters.AddWithValue("@OrderId", orderId);
                        itemCmd.Parameters.AddWithValue("@ProductId", item.Product.ProductId);
                        itemCmd.Parameters.AddWithValue("@Price", item.Product.FinalPrice);
                        itemCmd.Parameters.AddWithValue("@Quantity", item.Quantity);
                        itemCmd.ExecuteNonQuery();

                        string updateStockSql = @"UPDATE Products SET Stock = Stock - @Qty WHERE ProductId = @ProductId";

                        SqlCommand stockCmd = new SqlCommand(updateStockSql, con, transaction);
                        stockCmd.Parameters.AddWithValue("@Qty", item.Quantity);
                        stockCmd.Parameters.AddWithValue("@ProductId", item.Product.ProductId);

                        stockCmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
        public static int GetOrderCountByUser(int userId)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string sql = "SELECT COUNT(*) FROM Orders WHERE UserId=@UserId";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@UserId", userId);

                con.Open();
                return (int)cmd.ExecuteScalar();
            }
        }
        public static decimal GetTotalSpentByUser(int userId)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string sql = @"SELECT ISNULL(SUM(TotalAmount), 0) 
                       FROM Orders 
                       WHERE UserId=@UserId";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@UserId", userId);

                con.Open();
                return Convert.ToDecimal(cmd.ExecuteScalar());
            }
        }

        public static bool HasUserPurchased(int userId, int productId)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string sql = @"SELECT COUNT(*)
                       FROM Orders o
                       JOIN OrderItems oi ON o.OrderId = oi.OrderId
                       WHERE o.UserId=@UserId
                       AND oi.ProductId=@ProductId
                       AND o.Status='Delivered'";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@ProductId", productId);

                con.Open();
                return (int)cmd.ExecuteScalar() > 0;
            }
        }
        public static List<Product> GetDeliveredProductsByUser(int userId)
        {
            List<Product> products = new List<Product>();

            using (SqlConnection con = DbConnection.GetConnection())
            {
                string sql = @"SELECT DISTINCT p.ProductId, p.Name
                       FROM Orders o
                       INNER JOIN OrderItems oi ON o.OrderId = oi.OrderId
                       INNER JOIN Products p ON oi.ProductId = p.ProductId
                       WHERE o.UserId = @UserId
                       AND o.Status = 'Delivered'";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@UserId", userId);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    products.Add(new Product
                    {
                        ProductId = (int)reader["ProductId"],
                        Name = reader["Name"].ToString()
                    });
                }
            }

            return products;
        }
    }
}
