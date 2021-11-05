using System.Configuration;
using System.Data.SqlClient;
using App.BL.Models;

namespace App.BL.Stores
{
    // DO NOT CHANGE STATIC
    public static class CommentStore
    {
        public static void AddCommentToThread(Comment comment)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["db"].ConnectionString;
            using var sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            var query = $"INSERT INTO Comments VALUES ({comment.Id},{comment.Text},{comment.AuthorName},{comment.Created},{comment.ThreadId})";
            new SqlCommand(query, sqlConnection).ExecuteNonQuery();
        }
    }
}