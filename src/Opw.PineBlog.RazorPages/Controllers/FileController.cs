using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Opw.PineBlog.Files;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Opw.PineBlog.Controllers
{
    /// <summary>
    /// API controller for managing files.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUploadFileCommandFactory _uploadFileCommandFactory;
        private readonly IGetPagedFileListQueryFactory _getPagedFileListQueryFactory;
        private readonly ILogger<FileController> _logger;

        /// <summary>
        /// Implementation of FileController.
        /// </summary>
        /// <param name="mediator">Mediator.</param>
        /// <param name="uploadFileCommandFactory">Upload file command factory</param>
        /// <param name="getPagedFileListQueryFactory">Get paged file list query factory.</param>
        /// <param name="logger">Logger.</param>
        public FileController(
            IMediator mediator,
            IUploadFileCommandFactory uploadFileCommandFactory,
            IGetPagedFileListQueryFactory getPagedFileListQueryFactory,
            ILogger<FileController> logger)
        {
            _mediator = mediator;
            _uploadFileCommandFactory = uploadFileCommandFactory;
            _getPagedFileListQueryFactory = getPagedFileListQueryFactory;
            _logger = logger;
        }

        /// <summary>
        /// Get list of images and files.
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="directoryPath">The directory path to get the files from</param>
        /// <param name="fileType">The file type to filter on.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken, int page = 1, string directoryPath = "", FileType fileType = FileType.All)
        {
            try
            {
                var result = await _mediator.Send(_getPagedFileListQueryFactory.Create(page, 9, directoryPath, fileType), cancellationToken);
                if (!result.IsSuccess)
                    throw result.Exception;

                return Ok(result.Value);
            }
            catch (Exception)
            {
                // TODO: is a StatusCode result caught by HttpExceptions middleware?
                return StatusCode(StatusCodes.Status500InternalServerError, "Get files error");
            }
        }

        /// <summary>
        /// Upload file(s).
        /// </summary>
        /// <param name="files">Selected files.</param>
        /// <param name="targetPath">Target path.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(ICollection<IFormFile> files, string targetPath, CancellationToken cancellationToken)
        {
            try
            {
                foreach (var file in files)
                {
                    var result = await _mediator.Send(_uploadFileCommandFactory.Create(file, FileType.All, targetPath), cancellationToken);
                    if (!result.IsSuccess)
                        throw result.Exception;
                }
                return Ok("Created");
            }
            catch (Exception)
            {
                // TODO: is a StatusCode result caught by HttpExceptions middleware?
                return StatusCode(StatusCodes.Status500InternalServerError, "File upload error");
            }
        }
    }
}
