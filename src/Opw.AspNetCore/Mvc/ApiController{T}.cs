//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Infrastructure;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Options;
//using Opw.HttpExceptions.AspNetCore;
//using System;

//namespace Opw.AspNetCore.Mvc
//{
//    [ApiController]
//    [Route("api/v{version:apiVersion}/[controller]")]
//    [Produces("application/json")]
//    public abstract class ApiController<TController> : ControllerBase
//    {
//        protected readonly ILogger Logger;

//        public ApiController(ILogger<TController> logger)
//        {
//            Logger = logger;
//        }

//        /// <summary>
//        /// Creates an Microsoft.AspNetCore.Mvc.OkObjectResult object that produces an
//        /// Microsoft.AspNetCore.Http.StatusCodes.Status200OK response based on the result of the attempt.
//        /// </summary>
//        /// <typeparam name="T">Type of the attempt result.</typeparam>
//        /// <param name="result">The result to create an action result for.</param>
//        /// <returns>The created Microsoft.AspNetCore.Mvc.OkObjectResult or Opw.AspNetCore.Mvc.ExceptionResult for the response.</returns>
//        [NonAction]
//        protected IActionResult Ok<T>(Result<T> result)
//        {
//            if (!result.IsSuccess) return Exception(result);
//            return Ok(result.Value);
//        }

//        /// <summary>
//        /// Creates an Microsoft.AspNetCore.Mvc.NoContentResult object that produces an
//        /// Microsoft.AspNetCore.Http.StatusCodes.Status204NoContent response based on the result of the attempt.
//        /// </summary>
//        /// <typeparam name="T">Type of the attempt result.</typeparam>
//        /// <param name="result">The result to create an action result for.</param>
//        /// <returns>The created Microsoft.AspNetCore.Mvc.NoContentResult or Opw.AspNetCore.Mvc.ExceptionResult for the response.</returns>
//        [NonAction]
//        protected IActionResult NoContent<T>(Result<T> result)
//        {
//            if (!result.IsSuccess) return Exception(result);
//            return NoContent();
//        }

//        /// <summary>
//        /// Creates a Microsoft.AspNetCore.Mvc.CreatedAtActionResult object that produces
//        /// a Microsoft.AspNetCore.Http.StatusCodes.Status201Created response based on the result of the attempt.
//        /// </summary>
//        /// <typeparam name="T">Type of the attempt result.</typeparam>
//        /// <param name="result">The result to create an action result for.</param>
//        /// <param name="actionName">The name of the action to use for generating the URL.</param>
//        /// <returns>The created Microsoft.AspNetCore.Mvc.CreatedAtActionResult or Opw.AspNetCore.Mvc.ExceptionResult for the response.</returns>
//        [NonAction]
//        protected IActionResult CreatedAtAction<T>(string actionName, Func<Result<T>, object> routeValuesBuilder, Result<T> result)
//        {
//            if (!result.IsSuccess) return Exception(result);
//            return CreatedAtAction(actionName, routeValuesBuilder(result), result.Value);
//        }

//        /// <summary>
//        /// Creates a Opw.AspNetCore.Mvc.ExceptionResult result based on the exception of the attempt.
//        /// </summary>
//        /// <typeparam name="T">Type of the attempt result</typeparam>
//        /// <param name="result">The result to create an action result for</param>
//        /// <returns>The created Opw.AspNetCore.Mvc.ExceptionResult for the response.</returns>
//        [NonAction]
//        protected IStatusCodeActionResult Exception<T>(Result<T> result)
//        {
//            if (result.IsSuccess)
//                throw new ArgumentOutOfRangeException("The attempt was successful, we can not create an exception result for it.");

//            string resultMessage = null;
//            if (result.Value != null && result.Value.GetType() == typeof(string))
//                resultMessage = result.Value.ToString();

//            return Exception(result.Exception, resultMessage);
//        }

//        /// <summary>
//        /// Creates a Opw.AspNetCore.Mvc.ExceptionResult from the exception.
//        /// </summary>
//        /// <param name="ex">The exception to create a result for.</param>
//        /// <param name="result">Optional result string</param>
//        [NonAction]
//        public virtual IStatusCodeActionResult Exception(Exception ex, string result = null)
//        {
//            if (ex == null) ex = new Exception("Unknown exception");

//            Logger.LogError(ex, ex.Message);

//            IStatusCodeActionResult actionResult = default;
//            var httpExceptionsOptions = HttpContext.RequestServices.GetRequiredService<IOptions<HttpExceptionsOptions>>();
//            foreach (var mapper in httpExceptionsOptions.Value.ExceptionMappers)
//            {
//                if (mapper.TryMap(ex, HttpContext, out actionResult))
//                    break;
//            }

//            return actionResult;
//        }
//    }
//}
