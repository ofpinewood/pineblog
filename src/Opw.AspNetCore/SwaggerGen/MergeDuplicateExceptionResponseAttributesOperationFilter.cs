// TODO: fix
//using System.Linq;
//using Swashbuckle.AspNetCore.Swagger;
//using Swashbuckle.AspNetCore.SwaggerGen;

//namespace Opw.AspNetCore.SwaggerGen
//{
//    public class MergeDuplicateExceptionResponseAttributesOperationFilter : OperationFilterBase
//    {
//        public override void Apply(Operation operation, OperationFilterContext context)
//        {
//            var responseAttributes = context.ApiDescription.ActionDescriptor.FilterDescriptors
//                .Where(f => f.Filter is ExceptionResponseAttribute)
//                .Select(f => (ExceptionResponseAttribute)f.Filter).ToList();

//            foreach (var attr in responseAttributes)
//            {
//                var key = attr.StatusCode.ToString();
//                AddOrMergeDictionaryResponse(operation, key, attr, context);
//            }
//        }
//    }
//}
