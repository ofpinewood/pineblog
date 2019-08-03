using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Opw.PineBlog.Files;
using System;
using System.Collections.Generic;
using System.Text;
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
        private readonly ILogger<FileController> _logger;

        /// <summary>
        /// Implementation of FileController.
        /// </summary>
        /// <param name="mediator">Mediator.</param>
        /// <param name="logger">Logger.</param>
        public FileController(IMediator mediator, ILogger<FileController> logger)
        {
            _mediator = mediator;
            _logger = logger;
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
                    var result = await _mediator.Send(new UploadFileCommand
                    {
                        File = file,
                        FileName = file.FileName,
                        AllowedFileType = FileType.All,
                        TargetPath = targetPath
                    }, cancellationToken);

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
