using MediatR;
using Opw.HttpExceptions;
using Opw.PineBlog.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Opw.PineBlog.Posts
{
    /// <summary>
    /// Command that deletes a post.
    /// </summary>
    public class DeletePostCommand : IRequest<Result>
    {
        /// <summary>
        /// The post id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Handler for the DeletePostCommand.
        /// </summary>
        public class Handler : IRequestHandler<DeletePostCommand, Result>
        {
            private readonly IRepository _repo;

            /// <summary>
            /// Implementation of UnpublishPostCommand.Handler.
            /// </summary>
            /// <param name="repo">The blog entity context.</param>
            public Handler(IRepository repo)
            {
                _repo = repo;
            }

            /// <summary>
            /// Handle the DeletePostCommand request.
            /// </summary>
            /// <param name="request">The DeletePostCommand request.</param>
            /// <param name="cancellationToken">A cancellation token.</param>
            public async Task<Result> Handle(DeletePostCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var entity = await _repo.GetPostByIdAsync(request.Id, cancellationToken);
                    if (entity == null)
                        return Result.Fail(new NotFoundException<Post>($"Could not find post, id: \"{request.Id}\""));

                    return await _repo.DeletePostAsync(entity, cancellationToken) is { } result
                        ? Result.Success()
                        : Result.Fail(result.Exception);
                }
                catch (Exception ex)
                {
                    return Result.Fail(ex);
                }
            }
        }
    }
}
