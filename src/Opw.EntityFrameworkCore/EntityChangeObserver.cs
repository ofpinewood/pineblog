using System;
using System.Threading;

namespace Opw.EntityFrameworkCore
{
    /// <summary>
    /// Observer for changes on entities.
    /// </summary>
    public class EntityChangeObserver
    {
        /// <summary>
        /// An event fired when an entity that is tracked by the associated Microsoft.EntityFrameworkCore.DbContext
        /// has moved from one Microsoft.EntityFrameworkCore.EntityState to another.
        /// </summary>
        public event EventHandler<EntityChangeEventArgs> Changed;

        /// <summary>
        /// Let the EntityChangeObserver know an entity has changed.
        /// </summary>
        /// <param name="e">Event arguments for events relating to tracked Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntrys.</param>
        public void OnChanged(EntityChangeEventArgs e)
        {
            ThreadPool.QueueUserWorkItem((_) => Changed?.Invoke(this, e));
        }

        #region singleton

        private static readonly Lazy<EntityChangeObserver> lazy = new Lazy<EntityChangeObserver>(() => new EntityChangeObserver());

        private EntityChangeObserver() { }

        /// <summary>
        /// Singleton instance of EntityChangeObserver.
        /// </summary>
        public static EntityChangeObserver Instance => lazy.Value;

        #endregion
    }
}
