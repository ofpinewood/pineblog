using System;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Opw.AspNetCore.SwaggerGen
{
    public abstract class OperationFilterBase : IOperationFilter
    {
        public abstract void Apply(Operation operation, OperationFilterContext context);

        protected void AddOrMergeDictionaryResponse(Operation operation, string key, SwaggerResponseAttribute attr, OperationFilterContext context)
        {
            operation.Responses = operation.Responses ?? new Dictionary<string, Response>();

            if (operation.Responses.ContainsKey(key) && operation.Responses[key].Schema != null)
            {
                if (!operation.Responses[key].Description.Contains(attr.Description))
                {
                    operation.Responses[key].Description += $", {attr.Description}";
                }
                if (operation.Responses[key].Schema.Properties == null)
                    operation.Responses[key].Schema.Properties = new Dictionary<string, Schema>();

                operation.Responses[key].Schema.Properties.Add(attr.Description, GetSchema(attr.Type, context));
            }
            else if (operation.Responses.ContainsKey(key))
            {
                operation.Responses[key] = new Response
                {
                    Description = attr.Description,
                    Schema = GetSchema(attr.Type, context)
                };
            }
            else
            {
                operation.Responses.Add(key, new Response
                {
                    Description = attr.Description,
                    Schema = GetSchema(attr.Type, context)
                });
            }
        }

        protected Schema GetSchema(Type type, OperationFilterContext context)
        {
            return context.SchemaRegistry.GetOrRegister(type);
        }
    }
}
