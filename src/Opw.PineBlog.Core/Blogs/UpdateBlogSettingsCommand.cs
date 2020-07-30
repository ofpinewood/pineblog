using MediatR;
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
            private readonly IBlogUnitOfWork _uow;
            private readonly FileUrlHelper _fileUrlHelper;

            /// <summary>
            /// Implementation of UpdateBlogSettingsCommand.Handler.
            /// </summary>
            /// <param name="uow">The blog unit of work.</param>
            /// <param name="fileUrlHelper">File URL helper.</param>
            public Handler(IBlogUnitOfWork uow, FileUrlHelper fileUrlHelper)
            {
                _uow = uow;
                _fileUrlHelper = fileUrlHelper;
            }

            /// <summary>
            /// Handle the UpdateBlogSettingsCommand request.
            /// </summary>
            /// <param name="request">The UpdateBlogSettingsCommand request.</param>
            /// <param name="cancellationToken">A cancellation token.</param>
            public async Task<Result<BlogSettings>> Handle(UpdateBlogSettingsCommand request, CancellationToken cancellationToken)
            {
                var entity = await _uow.BlogSettings.SingleOrDefaultAsync(cancellationToken);
                if (entity == null)
                    entity = new BlogSettings();

                entity.Title = request.Title;
                entity.Description = request.Description;
                entity.CoverUrl = _fileUrlHelper.ReplaceBaseUrlWithUrlFormat(request.CoverUrl);
                entity.CoverCaption = request.CoverCaption;
                entity.CoverLink = request.CoverLink;

                if (entity.Created == DateTime.MinValue)
                    _uow.BlogSettings.Add(entity);
                else
                    _uow.BlogSettings.Update(entity);

                var result = await _uow.SaveChangesAsync(cancellationToken);
                if (!result.IsSuccess)
                    return Result<BlogSettings>.Fail(result.Exception);

                return Result<BlogSettings>.Success(entity);
            }
        }
    }
}
