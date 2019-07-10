using System.Collections.Generic;
using System.Text;

namespace Opw.EntityFrameworkCore
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }
}
