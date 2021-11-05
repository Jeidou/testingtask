using App.BL.Models;

namespace App.BL.Stores
{
    public class CommentStoreProxy : ICommentStore
    {
        public void AddCommentToThread(Comment comment)
        {
            CommentStore.AddCommentToThread(comment);
        }
    }
}