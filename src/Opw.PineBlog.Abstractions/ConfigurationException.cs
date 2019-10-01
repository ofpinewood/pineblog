using Opw.HttpExceptions;
using System;

namespace Opw.PineBlog
{
    /// <summary>
    /// Configuration error execution.
    /// </summary>
    public class ConfigurationException : HttpException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationException"></see> class with status code InternalServerError and a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public ConfigurationException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationException"></see> class with status code InternalServerError, a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference
        /// if no inner exception is specified.</param>
        public ConfigurationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the exception class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="info">info</paramref> parameter is null.</exception>
        /// <exception cref="System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0).</exception>
        public ConfigurationException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }

        #region Private Constructors

        private ConfigurationException() : base()
        {
        }

        private ConfigurationException(System.Net.HttpStatusCode statusCode) : base(statusCode)
        {
        }

        private ConfigurationException(System.Net.HttpStatusCode statusCode, string message) : base(statusCode, message)
        {
        }

        private ConfigurationException(System.Net.HttpStatusCode statusCode, string message, Exception innerException) : base(statusCode, message, innerException)
        {
        }

        #endregion
    }
}
