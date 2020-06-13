using MediatR;
using Microsoft.Extensions.Options;
using Opw.PineBlog.Entities;
using Opw.PineBlog.Files;
using Opw.PineBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Opw.PineBlog.Posts
{
    /// <summary>
    /// Query that gets a PostListModel.
    /// </summary>
    public class GetPagedPostListQuery : IRequest<Result<PostListModel>>
    {
        /// <summary>
        /// The requested page.
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Include unpublished posts or not.
        /// </summary>
        public bool IncludeUnpublished { get; set; }

        /// <summary>
        /// The number of items per page, if not set the BlogOptions.ItemsPerPage will be used.
        /// </summary>
        public int? ItemsPerPage { get; set; }

        /// <summary>
        /// Category to filter on.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Handler for the GetPagedPostListQuery.
        /// </summary>
        public class Handler : IRequestHandler<GetPagedPostListQuery, Result<PostListModel>>
        {
            private readonly IOptionsSnapshot<PineBlogOptions> _blogOptions;
            private readonly IRepository _repo;
            private readonly PostUrlHelper _postUrlHelper;
            private readonly FileUrlHelper _fileUrlHelper;

            /// <summary>
            /// Implementation of GetPagedPostListQuery.Handler.
            /// </summary>
            /// <param name="repo">The blog entity repository.</param>
            /// <param name="blogOptions">The blog options.</param>
            /// <param name="postUrlHelper">Post URL helper.</param>
            /// <param name="fileUrlHelper">File URL helper.</param>
            public Handler(IRepository repo, IOptionsSnapshot<PineBlogOptions> blogOptions, PostUrlHelper postUrlHelper, FileUrlHelper fileUrlHelper)
            {
                _blogOptions = blogOptions;
                _repo = repo;
                _postUrlHelper = postUrlHelper;
                _fileUrlHelper = fileUrlHelper;
            }

            /// <summary>
            /// Handle the GetPagedPostListQuery request.
            /// </summary>
            /// <param name="request">The GetPagedPostListQuery request.</param>
            /// <param name="cancellationToken">A cancellation token.</param>
            public async Task<Result<PostListModel>> Handle(GetPagedPostListQuery request, CancellationToken cancellationToken)
            {
                var itemsPerPage = (request.ItemsPerPage.HasValue) ? request.ItemsPerPage : _blogOptions.Value.ItemsPerPage;
                var pager = new Pager(request.Page, itemsPerPage.Value);

                IEnumerable<Post> posts = await _repo.GetPostListAsync(request.IncludeUnpublished, pager, request.Category, _blogOptions.Value, cancellationToken);

                posts = posts.Select(p => _postUrlHelper.ReplaceUrlFormatWithBaseUrl(p));

                var model = new PostListModel
                {
                    Blog = new BlogModel(_blogOptions.Value),
                    PostListType = PostListType.Blog,
                    Posts = posts,
                    Pager = pager
                };

                model.Blog.CoverUrl = _fileUrlHelper.ReplaceUrlFormatWithBaseUrl(model.Blog.CoverUrl);

                if (!string.IsNullOrWhiteSpace(request.Category))
                {
                    model.PostListType = PostListType.Category;
                    model.Category = request.Category;
                }

                return Result<PostListModel>.Success(model);
            }
        }
    }
}
