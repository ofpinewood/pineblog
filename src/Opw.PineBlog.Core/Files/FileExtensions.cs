using Opw.PineBlog.Models;
using System;
using System.Collections.Generic;

namespace Opw.PineBlog.Files
{
    /// <summary>
    /// Provides extension methods for FileTypes.
    /// </summary>
    public static class FileTypeExtensions
    {
        private readonly static IList<string> _imageMimeTypes = new List<string>() { "image/jpeg", "image/jpg", "image/gif", "image/png" };
        private readonly static IList<string> _videoMimeTypes = new List<string>() { "video/x-flv", "video/mp4", "video/quicktime", "video/x-msvideo", "video/x-ms-wmv", "video/3gpp", "video/webm" };

        /// <summary>
        /// Check if a certain mime-type is supported for the FileType.
        /// </summary>
        /// <param name="fileType"></param>
        /// <param name="mimeType"></param>
        public static bool IsFileTypeSupported(this FileType fileType, string mimeType)
        {
            switch (fileType)
            {
                case FileType.Image:
                    return _imageMimeTypes.Contains(mimeType.ToLower());
                case FileType.Video:
                    return _videoMimeTypes.Contains(mimeType.ToLower());
                case FileType.All:
                    return true;
                default:
                    throw new NotSupportedException($"{fileType} is not supported for validation.");
            }
        }

        /// <summary>
        /// Get the mime type for the file, based on the filename.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public static string GetMimeType(this string fileName)
        {
            return MimeMapping.MimeUtility.GetMimeMapping(fileName);
        }
    }
}
