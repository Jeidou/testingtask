using System;
using App.BL.Models;
using App.BL.Repositories;
using App.BL.Services;
using App.BL.Stores;

namespace App.BL
{
    public class CommentService
    {
        private readonly IThreadRepository threadRepository;
        private readonly ICommentStore commentStore;
        private readonly IDateTimeProvider dateTimeProvider;

        public CommentService() : this(new ThreadRepository(), new CommentStoreProxy(), new DateTimeProvider())
        {
        }

        public CommentService(IThreadRepository threadRepository, ICommentStore commentStore, IDateTimeProvider dateTimeProvider)
        {
            this.threadRepository = threadRepository;
            this.commentStore = commentStore;
            this.dateTimeProvider = dateTimeProvider;
        }

        public bool AddCommentToThread(string commentText, string authorName, Guid threadId)
        {
            if (string.IsNullOrEmpty(commentText))
            {
                throw new ArgumentException("Comment cannot be null or empty");
            }

            var thread = this.threadRepository.GetById(threadId);

            if (thread == null)
            {
                return false;
            }

            var today = this.dateTimeProvider.DateTimeNow;
            var timeSpan = today.Subtract(thread.Created);
            if (timeSpan.TotalMinutes > 70)
            {
                throw new ArgumentException("You cannot add comment to thread after 70 minutes of it creation");
            }

            if (thread.Resolved)
            {
                return false;
            }


            var comment = new Comment()
            {
                Id = Guid.NewGuid(),
                AuthorName = authorName,
                Created = DateTime.Now,
                Text = commentText,
                ThreadId = threadId,
                Index = ++thread.LastCommentIndex,
            };

            commentStore.AddCommentToThread(comment);

            return true;
        }
    }
}