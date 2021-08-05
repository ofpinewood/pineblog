using Opw.HttpExceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Opw.PineBlog.Git
{
    public class GitException : HttpException
    {
        public GitException()
        {

        }

        public GitException(string message)
            : base(message)
        {
        }

        public GitException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        #region Private Constructors

        private GitException(System.Net.HttpStatusCode statusCode) : base(statusCode)
        {
        }

        private GitException(System.Net.HttpStatusCode statusCode, string message) : base(statusCode, message)
        {
        }

        private GitException(System.Net.HttpStatusCode statusCode, string message, Exception innerException) : base(statusCode, message, innerException)
        {
        }

        #endregion
    }
}
