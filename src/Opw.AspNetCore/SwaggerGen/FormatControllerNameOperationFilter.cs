using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Text;
using Swashbuckle.AspNetCore.Swagger;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Opw.AspNetCore.SwaggerGen
{
    public class FormatControllerNameOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (!(context.ApiDescription.ActionDescriptor is ControllerActionDescriptor)) return;

            var controllerName = ((ControllerActionDescriptor)context.ApiDescription.ActionDescriptor).ControllerName;
            controllerName = Regex.Replace(controllerName, "[A-Z]", " $0").Trim();
            controllerName = controllerName.ToLowerInvariant();

            operation.Tags[operation.Tags.IndexOf(((ControllerActionDescriptor)context.ApiDescription.ActionDescriptor).ControllerName)] = controllerName;
        }
    }
}
