using Opw.HttpExceptions;
using System;

namespace Opw.PineBlog.GitDb
{
    public class GitDbException : HttpException
    {
        public GitDbException()
        {

        }

        public GitDbException(string message)
            : base(message)
        {
        }

        public GitDbException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        #region Private Constructors

        private GitDbException(System.Net.HttpStatusCode statusCode) : base(statusCode)
        {
        }

        private GitDbException(System.Net.HttpStatusCode statusCode, string message) : base(statusCode, message)
        {
        }

        private GitDbException(System.Net.HttpStatusCode statusCode, string message, Exception innerException) : base(statusCode, message, innerException)
        {
        }

        #endregion
    }
}
