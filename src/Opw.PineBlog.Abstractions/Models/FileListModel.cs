using System.Collections.Generic;

namespace Opw.PineBlog.Models
{
    public class FileListModel
    {
        public IEnumerable<string> Files { get; set; }
        public Pager Pager { get; set; }
    }
}
