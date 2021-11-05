using System;
using App.BL;
using App.BL.Models;
using App.BL.Repositories;
using App.BL.Services;
using App.BL.Stores;
using AutoFixture;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace App.Tests
{
    public class CommentServiceTests
    {
        private readonly CommentService sut;
        private readonly IThreadRepository threadRepository = Substitute.For<IThreadRepository>();
        private readonly ICommentStore commentStore = Substitute.For<ICommentStore>();
        private readonly IDateTimeProvider dateTimeProvider = Substitute.For<IDateTimeProvider>();

        private readonly Fixture fixture = new();

        public CommentServiceTests()
        {
            this.sut = new CommentService(this.threadRepository, this.commentStore, this.dateTimeProvider);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void AddCommentToThread_ShouldThrowArgumentExceptionForCommentText_WhenCommentTextIsNullOrEmpty(
            string commentText)
        {
            // Arrange 
            var authorName = this.fixture.Create<string>();
            var threadId = this.fixture.Create<Guid>();

            // Act
            Action act = () => this.sut.AddCommentToThread(commentText, authorName, threadId);

            // Assert
            var exception = Assert.Throws<ArgumentException>(act);
            Assert.Equal("Comment cannot be null or empty", exception.Message);
        }

        [Fact]
        public void AddCommentToThread_ShouldReturnFalse_WhenCommentThreadDoesNotExist()
        {
            // Arrange 
            var commentText = this.fixture.Create<string>();
            var authorName = this.fixture.Create<string>();
            var threadId = this.fixture.Create<Guid>();

            this.threadRepository.GetById(threadId).ReturnsNull();

            // Act
            var result = this.sut.AddCommentToThread(commentText, authorName, threadId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void
            AddCommentToThread_ShouldThrowArgumentExceptionForThreadCreatedDate_WhenCommentCreatedAfter70MinutesOfThreadCreation()
        {
            // Arrange 
            var commentText = this.fixture.Create<string>();
            var authorName = this.fixture.Create<string>();
            var threadId = this.fixture.Create<Guid>();

            var commentThread = this.fixture.Build<CommentThread>()
                .With(ct => ct.Created, new DateTime(2021, 11, 5, 14, 0, 0))
                .With(ct => ct.Id, threadId)
                .Create();

            this.threadRepository.GetById(threadId).Returns(commentThread);

            this.dateTimeProvider.DateTimeNow.Returns(new DateTime(2021, 11, 5, 16, 0, 0));

            // Act
            Action act = () => this.sut.AddCommentToThread(commentText, authorName, threadId);

            // Assert
            var exception = Assert.Throws<ArgumentException>(act);
            Assert.Equal("You cannot add comment to thread after 70 minutes of it creation", exception.Message);
        }

        [Fact]
        public void AddCommentToThread_ShouldReturnFalse_WhenCommentThreadIsResolved()
        {
            // Arrange 
            var commentText = this.fixture.Create<string>();
            var authorName = this.fixture.Create<string>();
            var threadId = this.fixture.Create<Guid>();

            var commentThread = this.fixture.Build<CommentThread>()
                .With(ct => ct.Id, threadId)
                .With(ct => ct.Resolved, true)
                .With(ct => ct.Created, new DateTime(2021, 11, 5, 14, 0, 0))
                .Create();

            this.threadRepository.GetById(threadId).Returns(commentThread);

            this.dateTimeProvider.DateTimeNow.Returns(new DateTime(2021, 11, 5, 14, 15, 0));

            // Act
            var result = this.sut.AddCommentToThread(commentText, authorName, threadId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void AddCommentToThread_ShouldReturnTrue_WhenCommentAddedToThread()
        {
            // Arrange 
            var commentText = this.fixture.Create<string>();
            var authorName = this.fixture.Create<string>();
            var threadId = this.fixture.Create<Guid>();

            var commentThread = this.fixture.Build<CommentThread>()
                .With(ct => ct.Id, threadId)
                .With(ct => ct.Resolved, false)
                .With(ct => ct.Created, new DateTime(2021, 11, 5, 14, 0, 0))
                .Create();

            this.threadRepository.GetById(threadId).Returns(commentThread);

            this.dateTimeProvider.DateTimeNow.Returns(new DateTime(2021, 11, 5, 14, 15, 0));

            // Act
            var result = this.sut.AddCommentToThread(commentText, authorName, threadId);

            // Assert
            Assert.True(result);
        }
    }
}