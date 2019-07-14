using MediatR;
using Opw.PineBlog.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Opw.PineBlog.Posts
{
    /// <summary>
    /// Command that adds a post.
    /// </summary>
    public class AddPostCommand : IRequest<Result<Post>>
    {
        /// <summary>
        /// The author id.
        /// </summary>
        public Guid AuthorId { get; set; }

        /// <summary>
        /// The post title
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


        //public string CoverUrl { get; set; }
        //public string CoverCaption { get; set; }
        //public string CoverLink { get; set; }

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
                var entity = new Post
                {
                    AuthorId = request.AuthorId,
                    Title = request.Title,
                    Description = request.Description,
                    Content = request.Content,
                    Categories = request.Categories,
                    //Cover = new Cover
                    //{
                    //    Url= request.CoverUrl,
                    //    Caption = request.CoverCaption,
                    //    Link = request.CoverLink
                    //}
                };

                _context.Posts.Add(entity);
                await _context.SaveChangesAsync(cancellationToken);

                return Result<Post>.Success(entity);
            }
        }
    }
}
