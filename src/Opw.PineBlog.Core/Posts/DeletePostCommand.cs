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
            private readonly IBlogUnitOfWork _uow;

            /// <summary>
            /// Implementation of UnpublishPostCommand.Handler.
            /// </summary>
            /// <param name="uow">The blog unit of work.</param>
            public Handler(IBlogUnitOfWork uow)
            {
                _uow = uow;
            }

            /// <summary>
            /// Handle the DeletePostCommand request.
            /// </summary>
            /// <param name="request">The DeletePostCommand request.</param>
            /// <param name="cancellationToken">A cancellation token.</param>
            public async Task<Result> Handle(DeletePostCommand request, CancellationToken cancellationToken)
            {
                var entity = await _uow.Posts.SingleOrDefaultAsync(e => e.Id.Equals(request.Id), cancellationToken);
                if (entity == null)
                    return Result.Fail(new NotFoundException<Post>($"Could not find post, id: \"{request.Id}\""));

                entity.Published = null;

                _uow.Posts.Remove(entity);
                var result = await _uow.SaveChangesAsync(cancellationToken);
                if (!result.IsSuccess)
                    return Result.Fail(result.Exception);

                return Result.Success();
            }
        }
    }
}
