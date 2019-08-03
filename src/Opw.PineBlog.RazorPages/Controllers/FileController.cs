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
        /// Get list of images and files.
        /// </summary>
        /// <param name="page">Page number</param>
        [HttpGet]
        public async Task<IActionResult> Get(int page = 1)
        {
            //var blog = await _data.CustomFields.GetBlogSettings();
            //var pager = new Pager(page, blog.ItemsPerPage);
            //IEnumerable<AssetItem> items;

            //if (string.IsNullOrEmpty(search))
            //{
            //    if (filter == "filterImages")
            //    {
            //        items = await _store.Find(a => a.AssetType == AssetType.Image, pager, "", !User.Identity.IsAuthenticated);
            //    }
            //    else if (filter == "filterAttachments")
            //    {
            //        items = await _store.Find(a => a.AssetType == AssetType.Attachment, pager, "", !User.Identity.IsAuthenticated);
            //    }
            //    else
            //    {
            //        items = await _store.Find(null, pager, "", !User.Identity.IsAuthenticated);
            //    }
            //}
            //else
            //{
            //    items = await _store.Find(a => a.Title.Contains(search), pager, "", !User.Identity.IsAuthenticated);
            //}

            //if (page < 1 || page > pager.LastPage)
            //    return null;

            //return new AssetsModel
            //{
            //    Assets = items,
            //    Pager = pager
            //};
            return null;
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
