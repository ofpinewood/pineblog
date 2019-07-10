using System;
using System.Collections.Generic;
using System.Text;

namespace Opw.EntityFrameworkCore
{
    public interface IEntityModified
    {
        DateTime Modified { get; set; }
    }
}
