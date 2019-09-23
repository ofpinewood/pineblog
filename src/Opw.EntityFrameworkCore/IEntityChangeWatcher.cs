using System;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Opw.EntityFrameworkCore
{
    public interface IEntityChangeWatcher
    {
        event EventHandler<EntityEntryEventArgs> Changed;

        void OnChanged(EntityEntryEventArgs e);
    }
}