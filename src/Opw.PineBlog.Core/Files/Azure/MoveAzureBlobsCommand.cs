using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Opw.PineBlog.Files.Azure
{
    /// <summary>
    /// Command that moves blobs in Azure blob storage.
    /// </summary>
    public class MoveAzureBlobsCommand : IMoveFilesCommand
    {
        /// <summary>
        /// The source folder path.
        /// </summary>
        public string SourcePath { get; set; }

        /// <summary>
        /// The target folder path.
        /// </summary>
        public string TargetPath { get; set; }

        /// <summary>
        /// Handler for the MoveAzureBlobsCommand.
        /// </summary>
        public class Handler : IMoveFilesCommandHandler<MoveAzureBlobsCommand>
        {
            private readonly IMediator _mediator;

            /// <summary>
            /// Implementation of MoveAzureBlobsCommand.Handler.
            /// </summary>
            /// <param name="mediator">Mediator</param>
            public Handler(IMediator mediator)
            {
                _mediator = mediator;
            }

            /// <summary>
            /// Handle the MoveAzureBlobsCommand request.
            /// </summary>
            /// <param name="request">The MoveAzureBlobsCommand request.</param>
            /// <param name="cancellationToken">A cancellation token.</param>
            public async Task<Result<string>> Handle(MoveAzureBlobsCommand request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}
