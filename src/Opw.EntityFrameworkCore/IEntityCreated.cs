using System;
using System.Collections.Generic;
using System.Text;

namespace Opw.EntityFrameworkCore
{
    public interface IEntityCreated
    {
        DateTime Created { get; set; }
    }
}
