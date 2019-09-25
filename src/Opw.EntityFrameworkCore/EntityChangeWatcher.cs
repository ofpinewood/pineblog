using System;
using System.Threading;

namespace Opw.EntityFrameworkCore
{
    /// <summary>
    /// Watches for changes on entities.
    /// </summary>
    public class EntityChangeWatcher
    {
        /// <summary>
        /// An event fired when an entity that is tracked by the associated Microsoft.EntityFrameworkCore.DbContext
        /// has moved from one Microsoft.EntityFrameworkCore.EntityState to another.
        /// </summary>
        public event EventHandler<EntityChangeEventArgs> Changed;

        /// <summary>
        /// Let the EntityChangeWatcher know an entity has changed.
        /// </summary>
        /// <param name="e">Event arguments for events relating to tracked Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntrys.</param>
        public void OnChanged(EntityChangeEventArgs e)
        {
            ThreadPool.QueueUserWorkItem((_) => Changed?.Invoke(this, e));
        }

        #region singleton

        private static readonly Lazy<EntityChangeWatcher> lazy = new Lazy<EntityChangeWatcher>(() => new EntityChangeWatcher());

        private EntityChangeWatcher() { }

        /// <summary>
        /// Singleton instance of EntityChangeWatcher.
        /// </summary>
        public static EntityChangeWatcher Instance => lazy.Value;

        #endregion
    }
}
