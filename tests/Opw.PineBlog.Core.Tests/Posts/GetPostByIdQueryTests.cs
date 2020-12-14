//using FluentAssertions;
//using FluentValidation.Results;
//using Microsoft.Extensions.DependencyInjection;
//using Opw.PineBlog.Entities;
//using Opw.HttpExceptions;
//using System;
//using System.Threading.Tasks;
//using Xunit;
//using System.Linq;

//namespace Opw.PineBlog.Posts
//{
//    public class GetPostByIdQueryTests : MediatRTestsBase
//    {
//        private Guid _postId;

//        public GetPostByIdQueryTests()
//        {
//            SeedDatabase();
//        }

//        [Fact]
//        public async Task Validator_Should_ThrowValidationErrorException()
//        {
//            Task action() => Mediator.Send(new GetPostByIdQuery());

//            var ex = await Assert.ThrowsAsync<ValidationErrorException<ValidationFailure>>(action);
//            ex.Errors.Single(e => e.Key.Equals(nameof(GetPostByIdQuery.Id))).Should().NotBeNull();
//        }

//        [Fact]
//        public async Task Handler_Should_ReturnNotFoundException()
//        {
//            var result = await Mediator.Send(new GetPostByIdQuery { Id = Guid.NewGuid() });

//            result.IsSuccess.Should().BeFalse();
//            result.Exception.Should().BeOfType<NotFoundException<Post>>();
//        }

//        [Fact]
//        public async Task Handler_Should_ReturnPost()
//        {
//            var result = await Mediator.Send(new GetPostByIdQuery { Id = _postId });

//            result.IsSuccess.Should().BeTrue();
//            result.Value.Title.Should().Be("Post title 0");
//        }

//        private void SeedDatabase()
//        {
//            var context = ServiceProvider.GetRequiredService<BlogEntityDbContext>();

//            var author = new Author { UserName = "user@example.com", DisplayName = "Author 1" };
//            context.Authors.Add(author);
//            context.SaveChanges();

//            var post = CreatePost(0, author.Id);
//            context.Posts.Add(post);
//            context.SaveChanges();

//            _postId = post.Id;
//        }

//        private Post CreatePost(int i, Guid authorId)
//        {
//            return new Post
//            {
//                AuthorId = authorId,
//                Title = "Post title " + i,
//                Slug = "post-title-" + i,
//                Description = "Description",
//                Content = "Content",
//                Published = DateTime.UtcNow.AddDays(-30 + i),
//                CoverUrl = "https://ofpinewood.com/cover-url",
//                CoverCaption = "Cover caption",
//                CoverLink = "https://ofpinewood.com/cover-link"
//            };
//        }
//    }
//}
