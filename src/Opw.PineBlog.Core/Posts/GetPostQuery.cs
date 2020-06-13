using MediatR;
using Microsoft.Extensions.Options;
using Opw.HttpExceptions;
using Opw.PineBlog.Entities;
using Opw.PineBlog.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Opw.PineBlog.Posts
{
    public class GetPostQuery : IRequest<Result<PostModel>>
    {
        public string Slug { get; set; }

        public class Handler : IRequestHandler<GetPostQuery, Result<PostModel>>
        {
            private readonly IOptionsSnapshot<PineBlogOptions> _blogOptions;
            private readonly IRepository _repo;
            private readonly PostUrlHelper _postUrlHelper;

            public Handler(IRepository repo, IOptionsSnapshot<PineBlogOptions> blogOptions, PostUrlHelper postUrlHelper)
            {
                _blogOptions = blogOptions;
                _repo = repo;
                _postUrlHelper = postUrlHelper;
            }

            public async Task<Result<PostModel>> Handle(GetPostQuery request, CancellationToken cancellationToken)
            {
                var post = await _repo.GetPostBySlugAsync(request.Slug, cancellationToken);
                if (post == null)
                    return Result<PostModel>.Fail(new NotFoundException<Post>($"Could not find post for slug: \"{request.Slug}\""));

                var model = new PostModel
                {
                    Blog = new BlogModel(_blogOptions.Value),
                    Post = post,
                    Next = null,
                    Previous = null
                };

                if (string.IsNullOrWhiteSpace(model.Post.CoverUrl))
                {
                    model.Post.CoverUrl = model.Blog.CoverUrl;
                    model.Post.CoverCaption = model.Blog.CoverCaption;
                    model.Post.CoverLink = model.Blog.CoverLink;
                }

                (model.Next, model.Previous) = await (_repo.GetNextPostAsync(post, cancellationToken), _repo.GetPreviousPostAsync(post, cancellationToken));

                model.Post = _postUrlHelper.ReplaceUrlFormatWithBaseUrl(model.Post);
                model.Next = _postUrlHelper.ReplaceUrlFormatWithBaseUrl(model.Next);
                model.Previous = _postUrlHelper.ReplaceUrlFormatWithBaseUrl(model.Previous);

                return Result<PostModel>.Success(model);
            }
        }
    }
}
