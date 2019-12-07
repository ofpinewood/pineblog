using System.Collections.Generic;

namespace Opw.PineBlog.Models
{
    /// <summary>
    /// File model.
    /// </summary>
    public class FileModel
    {
        /// <summary>
        /// The absolute URL.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// File name.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Mime type.
        /// </summary>
        public string MimeType { get; set; }
    }
}
