// TODO: fix and move to HttpExceptions
//using Opw.HttpExceptions;
//using Swashbuckle.AspNetCore.Annotations;
//using System;

//namespace Opw.AspNetCore.SwaggerGen
//{
//    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
//    public class ExceptionResponseAttribute<TResponse> : SwaggerResponseAttribute
//    {
//        public string Response { get; set; }

//        public ExceptionResponseAttribute(Type httpExceptionType)
//            : base((int)((HttpExceptionBase)Activator.CreateInstance(httpExceptionType)).StatusCode,
//                  ((HttpExceptionBase)Activator.CreateInstance(httpExceptionType)).,
//                  typeof(TResponse))
//        {
//            var httpException = ((HttpException)Activator.CreateInstance(httpExceptionType));
//            Response = httpException.Type.ToString();
//        }
//    }
//}
