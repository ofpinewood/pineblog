using System;

namespace Opw.PineBlog.GitDb
{
    /// <summary>
    /// Event arguments for events relating to tracked files.
    /// </summary>
    public class FileChangeEventArgs : EventArgs
    {
        /// <summary>
        /// The bytes for the file.
        /// </summary>
        public byte[] File { get; }

        public FileChangeEventArgs(byte[] file)
        {
            File = file;
        }
    }
}
