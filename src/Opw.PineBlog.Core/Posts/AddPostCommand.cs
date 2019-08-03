using MediatR;
using Microsoft.EntityFrameworkCore;
using Opw.HttpExceptions;
using Opw.PineBlog.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Opw.PineBlog.Posts
{
    /// <summary>
    /// Command that adds a post.
    /// </summary>
    public class AddPostCommand : IRequest<Result<Post>>, IEditPostCommand
    {
        /// <summary>
        /// The name of the user adding the post.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// The post title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The slug for this post, until the post is published a temporary slug will be used.
        /// </summary>
        public string Slug { get; set; } = DateTime.UtcNow.Ticks.ToString();

        /// <summary>
        /// A short description for the post.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The post content in markdown format.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// A comma separated list of categories.
        /// </summary>
        public string Categories { get; set; }

        /// <summary>
        /// The date the post was published or NULL for unpublished posts.
        /// </summary>
        public DateTime? Published { get; set; }

        /// <summary>
        /// Cover UURLrl.
        /// </summary>
        public string CoverUrl { get; set; }

        /// <summary>
        /// Cover caption.
        /// </summary>
        public string CoverCaption { get; set; }

        /// <summary>
        /// Cover link.
        /// </summary>
        public string CoverLink { get; set; }

        /// <summary>
        /// Handler for the AddPostCommand.
        /// </summary>
        public class Handler : IRequestHandler<AddPostCommand, Result<Post>>
        {
            private readonly IBlogEntityDbContext _context;

            /// <summary>
            /// Implementation of AddPostCommand.Handler.
            /// </summary>
            /// <param name="context">The blog entity context.</param>
            public Handler(IBlogEntityDbContext context)
            {
                _context = context;
            }

            /// <summary>
            /// Handle the AddPostCommand request.
            /// </summary>
            /// <param name="request">The AddPostCommand request.</param>
            /// <param name="cancellationToken">A cancellation token.</param>
            public async Task<Result<Post>> Handle(AddPostCommand request, CancellationToken cancellationToken)
            {
                var author = await _context.Authors.SingleOrDefaultAsync(a => a.UserName.Equals(request.UserName));
                if (author == null)
                    return Result<Post>.Fail(new NotFoundException($"Could not find author for \"{request.UserName}\"."));

                var entity = new Post
                {
                    AuthorId = author.Id,
                    Title = request.Title,
                    Slug = request.Slug,
                    Description = request.Description,
                    Content = request.Content,
                    Categories = request.Categories
                };

                if (!string.IsNullOrEmpty(request.CoverUrl))
                {
                    entity.Cover = new Cover
                    {
                        Url = request.CoverUrl,
                        Caption = request.CoverCaption,
                        Link = request.CoverLink
                    };
                }

                _context.Posts.Add(entity);
                await _context.SaveChangesAsync(cancellationToken);

                return Result<Post>.Success(entity);
            }
        }
    }
}
