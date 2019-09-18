using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Opw.HttpExceptions;
using Opw.PineBlog.Entities;
using Opw.PineBlog.Files;
using System.Threading;
using System.Threading.Tasks;

namespace Opw.PineBlog.Blogs
{
    /// <summary>
    /// Query that gets the blog settings.
    /// </summary>
    public class GetBlogSettigsQuery : IRequest<Result<BlogSettings>>
    {
        /// <summary>
        /// Handler for the GetBlogSettigsQuery.
        /// </summary>
        public class Handler : IRequestHandler<GetBlogSettigsQuery, Result<BlogSettings>>
        {
            private readonly IBlogEntityDbContext _context;
            private readonly FileUrlHelper _fileUrlHelper;
            private readonly IOptions<PineBlogOptions> _blogOptions;

            /// <summary>
            /// Implementation of GetBlogSettigsQuery.Handler.
            /// </summary>
            /// <param name="context">The blog entity context.</param>
            /// <param name="fileUrlHelper">File URL helper.</param>
            public Handler(IBlogEntityDbContext context, FileUrlHelper fileUrlHelper, IOptions<PineBlogOptions> blogOptions)
            {
                _context = context;
                _fileUrlHelper = fileUrlHelper;
                _blogOptions = blogOptions;
            }

            /// <summary>
            /// Handle the GetBlogSettigsQuery request.
            /// </summary>
            /// <param name="request">The GetBlogSettigsQuery request.</param>
            /// <param name="cancellationToken">A cancellation token.</param>
            public async Task<Result<BlogSettings>> Handle(GetBlogSettigsQuery request, CancellationToken cancellationToken)
            {
                var blogSettings = await _context.BlogSettings.SingleOrDefaultAsync();
                if (blogSettings == null)
                {
                    blogSettings = new BlogSettings
                    {
                        Title = _blogOptions.Value.Title,
                        Description = _blogOptions.Value.Description,
                        CoverCaption = _blogOptions.Value.CoverCaption,
                        CoverLink = _blogOptions.Value.CoverLink,
                        CoverUrl = _blogOptions.Value.CoverUrl
                    };
                }

                blogSettings.CoverUrl = _fileUrlHelper.ReplaceUrlFormatWithBaseUrl(blogSettings.CoverUrl);

                return Result<BlogSettings>.Success(blogSettings);
            }
        }
    }
}
