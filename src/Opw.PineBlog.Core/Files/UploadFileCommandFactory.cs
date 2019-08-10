using Microsoft.AspNetCore.Http;
using Opw.PineBlog.Files.Azure;

namespace Opw.PineBlog.Files
{
    /// <summary>
    /// Creates the correct IUploadFileCommand based on the configuration.
    /// </summary>
    public class UploadFileCommandFactory : IUploadFileCommandFactory
    {
        /// <summary>
        /// Create a IUploadFileCommand based on the configuration.
        /// </summary>
        /// <param name="file">The file sent with the HTTP request.</param>
        /// <param name="allowedFileType">Allowed file type.</param>
        /// <param name="targetPath">The target file path, excluding the file name.</param>
        /// <returns></returns>
        public IUploadFileCommand Create(IFormFile file, FileType allowedFileType, string targetPath)
        {
            return new UploadAzureBlobCommand
            {
                File = file,
                AllowedFileType = allowedFileType,
                TargetPath = targetPath
            };
        }
    }
}
