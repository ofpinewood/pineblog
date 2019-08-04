using System.Collections.Generic;

namespace Opw.PineBlog.Models
{
    /// <summary>
    /// Model for file lists.
    /// </summary>
    public class FileListModel
    {
        /// <summary>
        /// The list of files.
        /// </summary>
        public IEnumerable<string> Files { get; set; }

        /// <summary>
        /// The pager.
        /// </summary>
        public Pager Pager { get; set; }
    }
}
