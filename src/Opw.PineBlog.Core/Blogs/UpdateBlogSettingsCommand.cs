using MediatR;
using Opw.HttpExceptions;
using Opw.PineBlog.Entities;
using Opw.PineBlog.Files;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Opw.PineBlog.Blogs
{
    /// <summary>
    /// Command that updates the blog settings.
    /// </summary>
    public class UpdateBlogSettingsCommand : IRequest<Result<BlogSettings>>
    {
        /// <summary>
        /// The blog title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// A short description for the blog.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Cover URL.
        /// </summary>
        public string CoverUrl { get; set; }

        /// <summary>
        /// Cover caption text.
        /// </summary>
        public string CoverCaption { get; set; }

        /// <summary>
        /// Cover link.
        /// </summary>
        public string CoverLink { get; set; }

        /// <summary>
        /// Handler for the UpdateBlogSettingsCommand.
        /// </summary>
        public class Handler : IRequestHandler<UpdateBlogSettingsCommand, Result<BlogSettings>>
        {
            private readonly IRepository _repo;
            private readonly FileUrlHelper _fileUrlHelper;

            /// <summary>
            /// Implementation of UpdateBlogSettingsCommand.Handler.
            /// </summary>
            /// <param name="repo">The blog entity repo.</param>
            /// <param name="fileUrlHelper">File URL helper.</param>
            public Handler(IRepository repo, FileUrlHelper fileUrlHelper)
            {
                _repo = repo;
                _fileUrlHelper = fileUrlHelper;
            }

            /// <summary>
            /// Handle the UpdateBlogSettingsCommand request.
            /// </summary>
            /// <param name="request">The UpdateBlogSettingsCommand request.</param>
            /// <param name="cancellationToken">A cancellation token.</param>
            public async Task<Result<BlogSettings>> Handle(UpdateBlogSettingsCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var entity = await _repo.GetBlogSettingsAsync(cancellationToken) ?? new BlogSettings();

                    entity.Title = request.Title;
                    entity.Description = request.Description;
                    entity.CoverUrl = _fileUrlHelper.ReplaceBaseUrlWithUrlFormat(request.CoverUrl);
                    entity.CoverCaption = request.CoverCaption;
                    entity.CoverLink = request.CoverLink;

                    return await _repo.UpdateBlogSettingsAsync(entity, cancellationToken) is { } result
                        ? Result<BlogSettings>.Success(entity)
                        : Result<BlogSettings>.Fail(result.Exception);
                }
                catch (Exception ex)
                {
                    return Result<BlogSettings>.Fail(ex);
                }
            }
        }
    }
}
