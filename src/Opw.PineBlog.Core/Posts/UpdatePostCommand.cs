using MediatR;
using Microsoft.EntityFrameworkCore;
using Opw.HttpExceptions;
using Opw.PineBlog.Entities;
using Opw.PineBlog.Files;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Opw.PineBlog.Posts
{
    /// <summary>
    /// Command that updates a post.
    /// </summary>
    public class UpdatePostCommand : IRequest<Result<Post>>, IEditPostCommand
    {
        /// <summary>
        /// The post id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The post title.
        /// </summary>
        public string Title { get; set; }

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
        /// Handler for the UpdatePostCommand.
        /// </summary>
        public class Handler : IRequestHandler<UpdatePostCommand, Result<Post>>
        {
            private readonly IBlogEntityDbContext _context;
            private readonly PostUrlHelper _postUrlHelper;

            /// <summary>
            /// Implementation of UpdatePostCommand.Handler.
            /// </summary>
            /// <param name="context">The blog entity context.</param>
            /// <param name="postUrlHelper">Post URL helper.</param>
            public Handler(IBlogEntityDbContext context, PostUrlHelper postUrlHelper)
            {
                _context = context;
                _postUrlHelper = postUrlHelper;
            }

            /// <summary>
            /// Handle the UpdatePostCommand request.
            /// </summary>
            /// <param name="request">The UpdatePostCommand request.</param>
            /// <param name="cancellationToken">A cancellation token.</param>
            public async Task<Result<Post>> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
            {
                var entity = await _context.Posts.SingleOrDefaultAsync(e => e.Id.Equals(request.Id));
                if (entity == null)
                    return Result<Post>.Fail(new NotFoundException<Post>($"Could not find post, id: \"{request.Id}\""));

                var oldSlug = entity.Slug;

                entity.Title = request.Title;
                entity.Slug = request.Title.ToSlug();
                entity.Description = request.Description;
                entity.Content = request.Content;
                entity.Categories = request.Categories;
                entity.Published = request.Published;
                entity.CoverUrl = request.CoverUrl;
                entity.CoverCaption = request.CoverCaption;
                entity.CoverLink = request.CoverLink;

                entity = _postUrlHelper.ReplaceBaseUrlWithUrlFormat(entity);

                _context.Posts.Update(entity);
                var result = await _context.SaveChangesAsync(true, cancellationToken);
                if (!result.IsSuccess)
                    return Result<Post>.Fail(result.Exception);

                if (!oldSlug.Equals(entity.Slug))
                {
                    // TODO: update folders
                }

                return Result<Post>.Success(entity);
            }
        }
    }
}
