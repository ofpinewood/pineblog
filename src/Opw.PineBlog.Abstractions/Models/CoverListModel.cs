using Opw.PineBlog.Entities;
using System.Collections.Generic;

namespace Opw.PineBlog.Models
{
    public class CoverListModel
    {
        public IEnumerable<Cover> Covers { get; set; }
        public Pager Pager { get; set; }
    }
}
