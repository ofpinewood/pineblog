using Opw.PineBlog.Files.Azure;

namespace Opw.PineBlog.Files
{
    /// <summary>
    /// Creates the correct IDeleteFileCommand based on the configuration.
    /// </summary>
    public class DeleteFileCommandFactory : IDeleteFileCommandFactory
    {
        /// <summary>
        /// Create a IDeleteFileCommand based on the configuration.
        /// </summary>
        /// <param name="fileName">The name of the file to the delete.</param>
        /// <param name="targetPath">The target file path, excluding the file name.</param>
        /// <returns></returns>
        public IDeleteFileCommand Create(string fileName, string targetPath)
        {
            return new DeleteAzureBlobCommand
            {
                FileName = fileName,
                TargetPath = targetPath
            };
        }
    }
}
