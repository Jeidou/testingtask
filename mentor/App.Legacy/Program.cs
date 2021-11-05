using System;
using App.BL;

namespace App.Legacy
{
    // DO NOT CHANGE IT AT ALL
    class Program
    {
        static void Main(string[] args)
        {
            var commentService = new CommentService();
            commentService.AddCommentToThread("Nice work!", "Elon Musk", Guid.Parse("CA36C396-DE6B-4907-BBB6-8AAD4D4657EE"));
        }
    }
}