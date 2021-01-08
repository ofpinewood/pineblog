using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Opw.PineBlog.MongoDb
{
    /// <summary>
    /// Observer for changes on MongoDb documents.
    /// </summary>
    public class DocumentChangeObserver
    {
        /// <summary>
        /// An event fired when an entity that is tracked by the associated MongoDatabase has moved changed.
        /// </summary>
        public event EventHandler<DocumentChangeEventArgs> Changed;

        /// <summary>
        /// Let the DocumentChangeObserver know an document has changed.
        /// </summary>
        /// <param name="e">Event arguments for events relating to tracked MongoDB.Driver.ChangeStreamDocuments.</param>
        public void OnChanged(DocumentChangeEventArgs e)
        {
            ThreadPool.QueueUserWorkItem((_) => Changed?.Invoke(this, e));
        }

        #region singleton

        private static readonly Lazy<DocumentChangeObserver> lazy = new Lazy<DocumentChangeObserver>(() => new DocumentChangeObserver());

        private DocumentChangeObserver() { }

        /// <summary>
        /// Singleton instance of DocumentChangeObserver.
        /// </summary>
        public static DocumentChangeObserver Instance => lazy.Value;

        #endregion
    }
}
