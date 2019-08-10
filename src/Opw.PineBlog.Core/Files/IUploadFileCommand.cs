using MediatR;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Opw.PineBlog.Files
{
    /// <summary>
    /// Interface for commands that upload a file.
    /// </summary>
    public interface IUploadFileCommand : IRequest<Result<string>>
    {
        /// <summary>
        /// The file sent with the HTTP request.
        /// </summary>
        IFormFile File { get; set; }

        /// <summary>
        /// Allowed file type.
        /// </summary>
        FileType AllowedFileType { get; set; }

        /// <summary>
        /// The target file path, excluding the file name.
        /// </summary>
        string TargetPath { get; set; }
    }

    /// <summary>
    /// Interface for handlers for the IUploadFileCommand.
    /// </summary>
    public interface IUploadFileCommandHandler<TRequest> : IRequestHandler<TRequest, Result<string>>
        where TRequest : IUploadFileCommand
    {
        /// <summary>
        /// Handle a IUploadFileCommand request.
        /// </summary>
        /// <param name="request">A IUploadFileCommand request.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        new Task<Result<string>> Handle(TRequest request, CancellationToken cancellationToken);
    }
}