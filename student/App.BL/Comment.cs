using System;

namespace App.BL
{
    public class Comment
    {
        public Guid Id { get; set; }

        public string Text { get; set; }

        public string AuthorName { get; set; }

        public DateTime Created { get; set; }

        public long Index { get; set; }

        public Guid ThreadId { get; set; }
    }
}