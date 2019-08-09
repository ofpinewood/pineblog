using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Opw.PineBlog.Files
{
    /// <summary>
    /// Interface for commands that move files.
    /// </summary>
    public interface IMoveFilesCommand : IRequest<Result<string>>
    {
        /// <summary>
        /// The source folder path.
        /// </summary>
        string SourcePath { get; set; }

        /// <summary>
        /// The target folder path.
        /// </summary>
        string TargetPath { get; set; }
    }

    /// <summary>
    /// Interface for handlers for the IMoveFilesCommand.
    /// </summary>
    public interface IMoveFilesCommandHandler<TRequest> : IRequestHandler<TRequest, Result<string>>
        where TRequest : IMoveFilesCommand
    {
        /// <summary>
        /// Handle a IMoveFilesCommand request.
        /// </summary>
        /// <param name="request">A IMoveFilesCommand request.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        new Task<Result<string>> Handle(TRequest request, CancellationToken cancellationToken);
    }
}