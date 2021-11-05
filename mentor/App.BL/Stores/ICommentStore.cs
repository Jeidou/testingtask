using App.BL.Models;

namespace App.BL.Stores
{
    public interface ICommentStore
    {
        void AddCommentToThread(Comment comment);
    }
}