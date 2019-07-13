using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Opw.HttpExceptions;
using System;

namespace Opw.PineBlog
{
    /// <summary>
    /// Base class for PageModels.
    /// </summary>
    public abstract class PageModelBase<TPageModel> : PageModel
    {
        /// <summary>
        /// Logger.
        /// </summary>
        protected readonly ILogger Logger;

        /// <summary>
        /// Implementation of PageModelBase.
        /// </summary>
        /// <param name="logger">Logger.</param>
        public PageModelBase(ILogger<TPageModel> logger)
        {
            Logger = logger;
        }

        /// <summary>
        /// Creates a Opw.AspNetCore.Mvc.ExceptionResult result based on the exception of the attempt.
        /// </summary>
        /// <typeparam name="T">Type of the attempt result</typeparam>
        /// <param name="result">The result to create an action result for</param>
        /// <returns>The created Opw.AspNetCore.Mvc.ExceptionResult for the response.</returns>
        protected IStatusCodeActionResult Error<T>(Result<T> result)
        {
            if (result.IsSuccess)
                throw new ArgumentOutOfRangeException("The result was successful, we can not create an exception result for it.");

            return Error(result.Exception);
        }

        /// <summary>
        /// Creates a Opw.AspNetCore.Mvc.ExceptionResult from the exception.
        /// </summary>
        /// <param name="ex">The exception to create a result for.</param>
        protected virtual IStatusCodeActionResult Error(Exception ex)
        {
            if (ex == null) ex = new Exception("Unknown exception");
            var httpException = ex as HttpExceptionBase;
            if (httpException ==  null) httpException = new HttpException(ex.Message, ex);

            Logger.LogError(ex, ex.Message);

            // throw the exception
            // TODO: how do we want to handle exceptions?
            throw httpException;
            //return new ObjectResult(ex) { StatusCode = (int)httpException.StatusCode };
        }
    }
}
