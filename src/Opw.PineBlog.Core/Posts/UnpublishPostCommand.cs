using MediatR;
using Opw.HttpExceptions;
using Opw.PineBlog.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Opw.PineBlog.Posts
{
    /// <summary>
    /// Command that unpublishes a post.
    /// </summary>
    public class UnpublishPostCommand : IRequest<Result<Post>>
    {
        /// <summary>
        /// The post id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Handler for the UnpublishPostCommand.
        /// </summary>
        public class Handler : IRequestHandler<UnpublishPostCommand, Result<Post>>
        {
            private readonly IRepository _repo;

            /// <summary>
            /// Implementation of UnpublishPostCommand.Handler.
            /// </summary>
            /// <param name="repo">The blog entity repo.</param>
            public Handler(IRepository repo)
            {
                _repo = repo;
            }

            /// <summary>
            /// Handle the UnpublishPostCommand request.
            /// </summary>
            /// <param name="request">The UnpublishPostCommand request.</param>
            /// <param name="cancellationToken">A cancellation token.</param>
            public async Task<Result<Post>> Handle(UnpublishPostCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    return await _repo.UnpublishPostAsync(request.Id, cancellationToken);
                }
                catch (Exception ex)
                {
                    return Result<Post>.Fail(ex);
                }
            }
        }
    }
}
