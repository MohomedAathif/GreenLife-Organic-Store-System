using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLifeOrganicStore
{
    public class ProductReview
    {
        public int ReviewId { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; }

        // Add Review
        public void AddReview()
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string sql = @"INSERT INTO ProductReviews 
                               (ProductId, UserId, Rating, Comment)
                               VALUES (@ProductId, @UserId, @Rating, @Comment)";

                SqlCommand cmd = new SqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@ProductId", ProductId);
                cmd.Parameters.AddWithValue("@UserId", UserId);
                cmd.Parameters.AddWithValue("@Rating", Rating);
                cmd.Parameters.AddWithValue("@Comment", Comment);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public static bool HasUserReviewed(int userId, int productId)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string sql = @"SELECT COUNT(*)
                           FROM ProductReviews
                           WHERE UserId=@UserId
                           AND ProductId=@ProductId";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@ProductId", productId);

                con.Open();
                return (int)cmd.ExecuteScalar() > 0;
            }
        }
        // Get Reviews By Product
        public static List<ProductReview> GetReviewsByProduct(int productId)
        {
            List<ProductReview> reviews = new List<ProductReview>();

            using (SqlConnection con = DbConnection.GetConnection())
            {
                string sql = @"SELECT * FROM ProductReviews 
                               WHERE ProductId=@ProductId";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@ProductId", productId);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    reviews.Add(new ProductReview
                    {
                        ReviewId = (int)reader["ReviewId"],
                        ProductId = (int)reader["ProductId"],
                        UserId = (int)reader["UserId"],
                        Rating = (int)reader["Rating"],
                        Comment = reader["Comment"].ToString(),
                        ReviewDate = Convert.ToDateTime(reader["ReviewDate"])
                    });
                }
            }

            return reviews;
        }

        // Get Average Rating
        public static double GetAverageRating(int productId)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string sql = @"SELECT ISNULL(AVG(CAST(Rating AS FLOAT)), 0)
                               FROM ProductReviews
                               WHERE ProductId=@ProductId";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@ProductId", productId);

                con.Open();
                return Convert.ToDouble(cmd.ExecuteScalar());
            }
        }

    }
}
