using MediatR;
using Opw.HttpExceptions;
using Opw.PineBlog.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Opw.PineBlog.Posts
{
    /// <summary>
    /// Query that gets a post by id.
    /// </summary>
    public class GetPostByIdQuery : IRequest<Result<Post>>
    {
        /// <summary>
        /// Post id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Handler for the GetPostByIdQuery.
        /// </summary>
        public class Handler : IRequestHandler<GetPostByIdQuery, Result<Post>>
        {
            private readonly IBlogUnitOfWork _uow;
            private readonly PostUrlHelper _postUrlHelper;

            /// <summary>
            /// Implementation of GetPostByIdQuery.Handler.
            /// </summary>
            /// <param name="uow">The blog unit of work.</param>
            /// <param name="postUrlHelper">Post URL helper.</param>
            public Handler(IBlogUnitOfWork uow, PostUrlHelper postUrlHelper)
            {
                _uow = uow;
                _postUrlHelper = postUrlHelper;
            }

            /// <summary>
            /// Handle the GetPostByIdQuery request.
            /// </summary>
            /// <param name="request">The GetPostByIdQuery request.</param>
            /// <param name="cancellationToken">A cancellation token.</param>
            public async Task<Result<Post>> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
            {
                var post = await _uow.Posts.SingleOrDefaultAsync(p => p.Id.Equals(request.Id), cancellationToken);
                if (post == null)
                    return Result<Post>.Fail(new NotFoundException<Post>($"Could not find post, id: \"{request.Id}\""));

                post = _postUrlHelper.ReplaceUrlFormatWithBaseUrl(post);

                return Result<Post>.Success(post);
            }
        }
    }
}
