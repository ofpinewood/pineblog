using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Opw.PineBlog.Files
{
    /// <summary>
    /// Interface for commands that delete a file.
    /// </summary>
    public interface IDeleteFileCommand : IRequest<Result>
    {
        /// <summary>
        /// The name of the file to delete.
        /// </summary>
        string FileName { get; set; }

        /// <summary>
        /// The target file path, excluding the file name.
        /// </summary>
        string TargetPath { get; set; }
    }

    /// <summary>
    /// Interface for handlers for the IDeleteFileCommand.
    /// </summary>
    public interface IDeleteFileCommandHandler<TRequest> : IRequestHandler<TRequest, Result>
        where TRequest : IDeleteFileCommand
    {
        /// <summary>
        /// Handle a IDeleteFileCommand request.
        /// </summary>
        /// <param name="request">A IDeleteFileCommand request.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        new Task<Result> Handle(TRequest request, CancellationToken cancellationToken);
    }
}