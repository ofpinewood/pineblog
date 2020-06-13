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
            private readonly IRepository _repo;
            private readonly PostUrlHelper _postUrlHelper;

            /// <summary>
            /// Implementation of GetPostByIdQuery.Handler.
            /// </summary>
            /// <param name="repo">The blog entity repo.</param>
            /// <param name="postUrlHelper">Post URL helper.</param>
            public Handler(IRepository repo, PostUrlHelper postUrlHelper)
            {
                _repo = repo;
                _postUrlHelper = postUrlHelper;
            }

            /// <summary>
            /// Handle the GetPostByIdQuery request.
            /// </summary>
            /// <param name="request">The GetPostByIdQuery request.</param>
            /// <param name="cancellationToken">A cancellation token.</param>
            public async Task<Result<Post>> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var post = await _repo.GetPostByIdAsync(request.Id, cancellationToken);
                    if (post == null)
                        return Result<Post>.Fail(new NotFoundException<Post>($"Could not find post, id: \"{request.Id}\""));

                    post = _postUrlHelper.ReplaceUrlFormatWithBaseUrl(post);

                    return Result<Post>.Success(post);
                }
                catch (Exception ex)
                {
                    return Result<Post>.Fail(ex);
                }
            }
        }
    }
}
