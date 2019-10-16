using Microsoft.AspNetCore.Http;

namespace Opw.PineBlog.Files
{
    public interface IUploadFileCommandFactory
    {
        IUploadFileCommand Create(IFormFile file, FileType allowedFileType, string targetPath);
    }
}