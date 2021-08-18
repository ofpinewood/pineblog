using System;
using System.Threading;

namespace Opw.PineBlog.GitDb
{
    /// <summary>
    /// Observer for changes on files in a Git branch.
    /// </summary>
    public class FileChangeObserver
    {
        /// <summary>
        /// An event fired when an entity that is tracked by the associated Git branch has changed.
        /// </summary>
        public event EventHandler<FileChangeEventArgs> Changed;

        /// <summary>
        /// Let the FileChangeObserver know an file has changed.
        /// </summary>
        /// <param name="e">Event arguments for events relating to tracked files.</param>
        public void OnChanged(FileChangeEventArgs e)
        {
            ThreadPool.QueueUserWorkItem((_) => Changed?.Invoke(this, e));
        }

        #region singleton

        private static readonly Lazy<FileChangeObserver> lazy = new Lazy<FileChangeObserver>(() => new FileChangeObserver());

        private FileChangeObserver() { }

        /// <summary>
        /// Singleton instance of FileChangeObserver.
        /// </summary>
        public static FileChangeObserver Instance => lazy.Value;

        #endregion
    }
}
