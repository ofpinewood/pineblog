using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;

namespace Opw.EntityFrameworkCore
{
    /// <summary>
    /// Event arguments for events relating to tracked Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntrys.
    /// </summary>    
    public class EntityChangeEventArgs : EventArgs
    {
        /// <summary>
        /// The Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry for the entity.
        /// </summary>
        public EntityEntry Entry { get; }

        /// <summary>
        /// This API supports the Entity Framework Core infrastructure and is not intended
        /// to be used directly from your code. This API may change or be removed in future releases.
        /// </summary>      
        public EntityChangeEventArgs(EntityEntry entityEntry)
        {
            Entry = entityEntry;
        }
    }
}
