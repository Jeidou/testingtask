using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using App.BL.Models;
using FastMember;

namespace App.BL.Repositories
{
    public class ThreadRepository : IThreadRepository
    {
        public CommentThread GetById(Guid id)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["db"].ConnectionString;
            using var sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            var query = $"Select * from CommentThreads where Id = '{id}'";
            var reader = new SqlCommand(query, sqlConnection).ExecuteReader();

            var commentThread = new CommentThread();

            reader.Read();
            var accessor = TypeAccessor.Create(typeof(CommentThread), true);
            var members = accessor.GetMembers();
            for (var i = 0; i < reader.FieldCount; i++)
            {
                if (reader.IsDBNull(i))
                {
                    continue;
                }

                var fieldName = reader.GetName(i);
                if (members.Any(m => string.Equals(m.Name, fieldName, StringComparison.OrdinalIgnoreCase)))
                {
                    accessor[commentThread, fieldName] = reader.GetValue(i);
                }
            }

            return commentThread;
        }
    }
}