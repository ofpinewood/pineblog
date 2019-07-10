using MediatR;
using Opw.PineBlog.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Opw.PineBlog.Posts
{
    public class AddPostCommand : IRequest<Result<Post>>
    {
        public Guid AuthorId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Categories { get; set; }
        public string CoverUrl { get; set; }
        public string CoverCaption { get; set; }
        public string CoverLink { get; set; }

        public class Handler : IRequestHandler<AddPostCommand, Result<Post>>
        {
            private readonly IBlogEntityDbContext _context;

            public Handler(IBlogEntityDbContext context)
            {
                _context = context;
            }

            public async Task<Result<Post>> Handle(AddPostCommand request, CancellationToken cancellationToken)
            {
                var entity = new Post
                {
                    AuthorId = request.AuthorId,
                    Title = request.Title,
                    Description = request.Description,
                    Content = request.Content,
                    Categories = request.Categories,
                    Cover = new Cover
                    {
                        Url= request.CoverUrl,
                        Caption = request.CoverCaption,
                        Link = request.CoverLink
                    }
                };

                _context.Posts.Add(entity);
                await _context.SaveChangesAsync(cancellationToken);

                return Result<Post>.Success(entity);
            }
        }
    }
}
