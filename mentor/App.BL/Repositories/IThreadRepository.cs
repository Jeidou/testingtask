using System;
using App.BL.Models;

namespace App.BL.Repositories
{
    public interface IThreadRepository
    {
        CommentThread GetById(Guid id);
    }
}