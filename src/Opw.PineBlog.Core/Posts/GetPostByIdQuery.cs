using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Opw.HttpExceptions;
using Opw.PineBlog.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Opw.PineBlog.Posts
{
    public class GetPostByIdQuery : IRequest<Result<SinglePostModel>>
    {
        public Guid Id { get; set; }

        public class Handler : IRequestHandler<GetPostByIdQuery, Result<SinglePostModel>>
        {
            private readonly IOptions<BlogOptions> _blogOptions;
            private readonly IBlogEntityDbContext _context;

            public Handler(IBlogEntityDbContext context, IOptions<BlogOptions> blogOptions)
            {
                _blogOptions = blogOptions;
                _context = context;
            }

            public async Task<Result<SinglePostModel>> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
            {
                var post = await _context.Posts
                    .Include(p => p.Author)
                    .Include(p => p.Cover)
                    .Where(p => p.Published != null)
                    .Where(p => p.Id.Equals(request.Id))
                    .SingleOrDefaultAsync(cancellationToken);

                if (post == null)
                    return Result<SinglePostModel>.Fail(new NotFoundException($"Could not find post: \"{request.Id}\""));

                var model = new SinglePostModel
                {
                    Blog = new BlogModel(_blogOptions.Value),
                    Post = post,
                };

                if (model.Post.Cover == null)
                    model.Post.Cover = model.Blog.Cover;

                return Result<SinglePostModel>.Success(model);
            }
        }
    }
}
