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
            private readonly IBlogUnitOfWork _uow;

            /// <summary>
            /// Implementation of PublishPostCommand.Handler.
            /// </summary>
            /// <param name="uow">The blog unit of work.</param>
            public Handler(IBlogUnitOfWork uow)
            {
                _uow = uow;
            }

            /// <summary>
            /// Handle the PublishPostCommand request.
            /// </summary>
            /// <param name="request">The PublishPostCommand request.</param>
            /// <param name="cancellationToken">A cancellation token.</param>
            public async Task<Result<Post>> Handle(PublishPostCommand request, CancellationToken cancellationToken)
            {
                var entity = await _uow.Posts.SingleOrDefaultAsync(e => e.Id.Equals(request.Id), cancellationToken);
                if (entity == null)
                    return Result<Post>.Fail(new NotFoundException<Post>($"Could not find post, id: \"{request.Id}\""));

                entity.Published = DateTime.UtcNow;

                _uow.Posts.Update(entity);
                var result = await _uow.SaveChangesAsync(cancellationToken);
                if (!result.IsSuccess)
                    return Result<Post>.Fail(result.Exception);

                return Result<Post>.Success(entity);
            }
        }
    }
}
