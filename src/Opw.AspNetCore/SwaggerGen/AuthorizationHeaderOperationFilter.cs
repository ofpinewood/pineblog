using Microsoft.AspNetCore.Mvc.Authorization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Opw.AspNetCore.SwaggerGen
{
    public class AuthorizationHeaderOperationFilter : IOperationFilter
    {
        private readonly string _httpHeader;
        private readonly string _authenticationScheme;
        private readonly string _valueName;

        public AuthorizationHeaderOperationFilter(string httpHeader, string authenticationScheme, string valueName)
        {
            _httpHeader = httpHeader;
            _authenticationScheme = authenticationScheme;
            _valueName = valueName;
        }

        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation == null) return;
            if (operation.Parameters == null)
                operation.Parameters = new List<IParameter>();

            var authorizeAttributes = context.ApiDescription.ActionDescriptor.FilterDescriptors.Where(f => f.Filter is AuthorizeFilter).ToList();
            var allowAnonymousAttributes = context.ApiDescription.ActionDescriptor.FilterDescriptors.Where(f => f.Filter is AllowAnonymousFilter).ToList();
            if (!authorizeAttributes.Any() || allowAnonymousAttributes.Any())
                return;

            // Provide a Security Requirement Object to the operation to indicate which scheme is applicable
            operation.Security = new List<IDictionary<string, IEnumerable<string>>>()
            {
                new Dictionary<string, IEnumerable<string>>()
                {
                    { _authenticationScheme, new string[]{ } }
                }
            };

            var parameter = new NonBodyParameter
            {
                Description = $"The HTTP {_httpHeader} request header contains the credentials to authenticate a user agent " +
                "with a server, usually after the server has responded with a 401 Unauthorized status. " +
                $"Syntax: \"{_httpHeader}: {_authenticationScheme} [{_valueName}]\".",
                In = "header",
                Name = _httpHeader,
                Required = true,
                Type = "string",
                Format = $"{_httpHeader}: {_authenticationScheme} [{_valueName}]"
            };

            operation.Parameters.Add(parameter);
        }
    }
}
