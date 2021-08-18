using System;

namespace Opw.PineBlog.GitDb
{
    /// <summary>
    /// Event arguments for events relating to tracked files.
    /// </summary>
    public class FileChangeEventArgs : EventArgs
    {
        /// <summary>
        /// The name for the file.
        /// </summary>
        public string File { get; }

        /// <summary>
        /// The bytes for the file.
        /// </summary>
        public byte[] bytes { get; }
        public byte[] Bytes { get; }

        public FileChangeEventArgs(string file, byte[] bytes)
        {
            File = file;
            Bytes = bytes;
        }
    }
}
