// TODO: fix and move to HttpExceptions
//using Swashbuckle.AspNetCore.Annotations;
//using Swashbuckle.AspNetCore.Swagger;
//using Swashbuckle.AspNetCore.SwaggerGen;
//using System.Net;

//namespace Opw.AspNetCore.SwaggerGen
//{
//    public class ApplyInternalServerErrorResponseOperationFilter<TResponse> : OperationFilterBase
//    {
//        public override void Apply(Operation operation, OperationFilterContext context)
//        {
//            string key = ((int)HttpStatusCode.InternalServerError).ToString();
//            var attr = new SwaggerResponseAttribute((int)HttpStatusCode.InternalServerError, nameof(HttpStatusCode.InternalServerError), typeof(TResponse));
//            AddOrMergeDictionaryResponse(operation, key, attr, context);
//        }
//    }
//}
