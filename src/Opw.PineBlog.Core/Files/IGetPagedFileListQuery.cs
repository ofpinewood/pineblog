using MediatR;
using Opw.PineBlog.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Opw.PineBlog.Files
{
    /// <summary>
    /// Interface for query that gets a FileListModel.
    /// </summary>
    public interface IGetPagedFileListQuery : IRequest<Result<FileListModel>>
    {
        /// <summary>
        /// The requested page.
        /// </summary>
        int Page { get; set; }

        /// <summary>
        /// The number of items per page.
        /// </summary>
        int ItemsPerPage { get; set; }

        /// <summary>
        /// The directory path to get the files from.
        /// </summary>
        string DirectoryPath { get; set; }

        /// <summary>
        /// The file type to filter on.
        /// </summary>
        FileType FileType { get; set; }
    }

    /// <summary>
    /// Interface for handlers for the IGetPagedFileListQuery.
    /// </summary>
    public interface IGetPagedFileListQueryHandler<TRequest> : IRequestHandler<TRequest, Result<FileListModel>>
        where TRequest : IGetPagedFileListQuery
    {
        /// <summary>
        /// Handle a IGetPagedFileListQuery request.
        /// </summary>
        /// <param name="request">A IGetPagedFileListQuery request.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        new Task<Result<FileListModel>> Handle(TRequest request, CancellationToken cancellationToken);
    }
}