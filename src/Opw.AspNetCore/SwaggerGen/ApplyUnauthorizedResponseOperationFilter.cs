// TODO: fix and move to HttpExceptions
//using Microsoft.AspNetCore.Mvc.Authorization;
//using Swashbuckle.AspNetCore.Annotations;
//using Swashbuckle.AspNetCore.Swagger;
//using Swashbuckle.AspNetCore.SwaggerGen;
//using System.Linq;
//using System.Net;

//namespace Opw.AspNetCore.SwaggerGen
//{
//    public class ApplyUnauthorizedResponseOperationFilter : OperationFilterBase
//    {
//        public override void Apply(Operation operation, OperationFilterContext context)
//        {
//            var authorizeAttributes = context.ApiDescription.ActionDescriptor.FilterDescriptors.Where(f => f.Filter is AuthorizeFilter).ToList();
//            var allowAnonymousAttributes = context.ApiDescription.ActionDescriptor.FilterDescriptors.Where(f => f.Filter is AllowAnonymousFilter).ToList();
//            if (!authorizeAttributes.Any() || allowAnonymousAttributes.Any())
//                return;

//            string key = ((int)HttpStatusCode.Unauthorized).ToString();
//            var attr = new SwaggerResponseAttribute((int)HttpStatusCode.Unauthorized, nameof(HttpStatusCode.Unauthorized));
//            AddOrMergeDictionaryResponse(operation, key, attr, context);
//        }
//    }
//}
