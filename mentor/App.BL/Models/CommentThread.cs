using System;

namespace App.BL.Models
{
    public class CommentThread
    {
        public Guid Id { get; set; }

        public bool Resolved { get; set; }

        public long LastCommentIndex { get; set; }

        public DateTime Created { get; set; }
    }
}