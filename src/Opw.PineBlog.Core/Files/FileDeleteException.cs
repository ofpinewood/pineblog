using Opw.HttpExceptions;
using System;

namespace Opw.PineBlog.Files
{
    /// <summary>
    /// Represents HTTP BadRequest (400) errors because of a file delete error.
    /// </summary>
    public class FileDeleteException : BadRequestException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileDeleteException"></see> class with status code BadRequest.
        /// </summary>
        public FileDeleteException() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileDeleteException"></see> class with status code BadRequest and a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public FileDeleteException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileDeleteException"></see> class with status code BadRequest, a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference
        /// if no inner exception is specified.</param>
        public FileDeleteException(string message, Exception innerException) : base(message, innerException) { }
    }
}
