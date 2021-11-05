using System;

namespace App.BL
{
    public class CommentThread
    {
        public Guid Id { get; set; }

        public bool Resolved { get; set; }

        public long LastCommentIndex { get; set; }

        public DateTime Created { get; set; }
    }
}