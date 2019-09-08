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
            private readonly IBlogEntityDbContext _context;

            /// <summary>
            /// Implementation of UnpublishPostCommand.Handler.
            /// </summary>
            /// <param name="context">The blog entity context.</param>
            public Handler(IBlogEntityDbContext context)
            {
                _context = context;
            }

            /// <summary>
            /// Handle the DeletePostCommand request.
            /// </summary>
            /// <param name="request">The DeletePostCommand request.</param>
            /// <param name="cancellationToken">A cancellation token.</param>
            public async Task<Result> Handle(DeletePostCommand request, CancellationToken cancellationToken)
            {
                var entity = await _context.Posts.SingleOrDefaultAsync(e => e.Id.Equals(request.Id));
                if (entity == null)
                    return Result.Fail(new NotFoundException<Post>($"Could not find post, id: \"{request.Id}\""));

                entity.Published = null;
                
                _context.Posts.Remove(entity);
                var result = await _context.SaveChangesAsync(true, cancellationToken);
                if (!result.IsSuccess)
                    return Result.Fail(result.Exception);

                return Result.Success();
            }
        }
    }
}
