using MediatR;
using Opw.HttpExceptions;
using Opw.PineBlog.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Opw.PineBlog.Posts
{
    /// <summary>
    /// Command that publishes a post.
    /// </summary>
    public class PublishPostCommand : IRequest<Result<Post>>
    {
        /// <summary>
        /// The post id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Handler for the PublishPostCommand.
        /// </summary>
        public class Handler : IRequestHandler<PublishPostCommand, Result<Post>>
        {
            private readonly IRepository _repo;

            /// <summary>
            /// Implementation of PublishPostCommand.Handler.
            /// </summary>
            /// <param name="repo">The blog entity repo.</param>
            public Handler(IRepository repo)
            {
                _repo = repo;
            }

            /// <summary>
            /// Handle the PublishPostCommand request.
            /// </summary>
            /// <param name="request">The PublishPostCommand request.</param>
            /// <param name="cancellationToken">A cancellation token.</param>
            public async Task<Result<Post>> Handle(PublishPostCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    return await _repo.PublishPostAsync(request.Id, cancellationToken);
                }
                catch (Exception ex)
                {
                    return Result<Post>.Fail(ex);
                }
            }
        }
    }
}
