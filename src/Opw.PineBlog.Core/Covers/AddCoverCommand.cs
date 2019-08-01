using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Opw.HttpExceptions;
using Opw.PineBlog.Entities;
using Opw.PineBlog.Files;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Opw.PineBlog.Covers
{
    /// <summary>
    /// Command that adds a cover.
    /// </summary>
    public class AddCoverCommand : IRequest<Result<Cover>>
    {
        /// <summary>
        /// The cover image file.
        /// </summary>
        public IFormFile File { get; set; }

        /// <summary>
        /// The file name.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// The cover caption.
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// The cover link.
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// Handler for the AddCoverCommand.
        /// </summary>
        public class Handler : IRequestHandler<AddCoverCommand, Result<Cover>>
        {
            private readonly IBlogEntityDbContext _context;
            private readonly IMediator _mediator;
            private readonly IOptions<PineBlogOptions> _blogOptions;
            private readonly FilePathHelper _filePathHelper;

            /// <summary>
            /// Implementation of AddCoverCommand.Handler.
            /// </summary>
            /// <param name="context">The blog entity context.</param>
            /// <param name="mediator">Mediator</param>
            /// <param name="filePathHelper">File path helper.</param>
            /// <param name="blogOptions">The blog options.</param>
            public Handler(IBlogEntityDbContext context, IMediator mediator, FilePathHelper filePathHelper, IOptions<PineBlogOptions> blogOptions)
            {
                _context = context;
                _mediator = mediator;
                _blogOptions = blogOptions;
                _filePathHelper = filePathHelper;
            }

            /// <summary>
            /// Handle the AddCoverCommand request.
            /// </summary>
            /// <param name="request">The AddCoverCommand request.</param>
            /// <param name="cancellationToken">A cancellation token.</param>
            public async Task<Result<Cover>> Handle(AddCoverCommand request, CancellationToken cancellationToken)
            {
                var result = await _mediator.Send(new UploadFileCommand
                {
                    File = request.File,
                    FileName = request.FileName,
                    AllowedFileType = FileType.Image,
                    TargetPath = _blogOptions.Value.CoverImagesPath
                }, cancellationToken);

                if (!result.IsSuccess)
                    return Result<Cover>.Fail(result.Exception);

                var url = _filePathHelper.GetPathFormat(result.Value);
                var entity = new Cover
                {
                    Url = url,
                    Caption = request.Caption,
                    Link = request.Link
                };

                _context.Covers.Add(entity);
                await _context.SaveChangesAsync(cancellationToken);

                return Result<Cover>.Success(entity);
            }
        }
    }
}
