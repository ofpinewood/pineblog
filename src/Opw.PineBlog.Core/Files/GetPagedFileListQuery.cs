using MediatR;
using Microsoft.Extensions.Options;
using Opw.PineBlog.Files.Azure;
using Opw.PineBlog.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Opw.PineBlog.Files
{
    /// <summary>
    /// Query that gets a FileListModel.
    /// </summary>
    public class GetPagedFileListQuery : IRequest<Result<FileListModel>>
    {
        /// <summary>
        /// The requested page.
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// The number of items per page, if not set the BlogOptions.ItemsPerPage will be used.
        /// </summary>
        public int? ItemsPerPage { get; set; }

        /// <summary>
        /// Handler for the GetPagedFileListQuery.
        /// </summary>
        public class Handler : IRequestHandler<GetPagedFileListQuery, Result<FileListModel>>
        {
            private readonly IOptions<PineBlogOptions> _blogOptions;
            private readonly IMediator _mediator;

            /// <summary>
            /// Implementation of GetPagedFileListQuery.Handler.
            /// </summary>
            /// <param name="mediator">Mediator</param>
            /// <param name="blogOptions">The blog options.</param>
            public Handler(IMediator mediator, IOptions<PineBlogOptions> blogOptions)
            {
                _blogOptions = blogOptions;
                _mediator = mediator;
            }

            /// <summary>
            /// Handle the GetPagedPostListQuery request.
            /// </summary>
            /// <param name="request">The GetPagedPostListQuery request.</param>
            /// <param name="cancellationToken">A cancellation token.</param>
            public async Task<Result<FileListModel>> Handle(GetPagedFileListQuery request, CancellationToken cancellationToken)
            {
                var itemsPerPage = request.ItemsPerPage.HasValue ? request.ItemsPerPage : _blogOptions.Value.ItemsPerPage;
                var pager = new Pager(request.Page, itemsPerPage.Value);

                // TODO: make file source configurable (not only azure blob storage)
                return await _mediator.Send(new GetPagedAzureBlobListQuery { Pager = pager }, cancellationToken);
            }
        }
    }
}
