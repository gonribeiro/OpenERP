using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace OpenERP
{
    public class Swagger : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var hasAuthorizeAttribute = context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any()
                                         || context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();

            if (!hasAuthorizeAttribute) return;

            operation.Security ??= new List<OpenApiSecurityRequirement>();

            var authAttributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>()
                .Union(context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>());

            foreach (var authAttribute in authAttributes)
            {
                var authPolicy = !string.IsNullOrEmpty(authAttribute.Policy) ? authAttribute.Policy : "default";
                var securitySchemeName = !string.IsNullOrEmpty(authAttribute.AuthenticationSchemes) ? authAttribute.AuthenticationSchemes : "Bearer";
                operation.Security.Add(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = securitySchemeName } },
                    new string[] { authPolicy }
                }
            });
            }
        }
    }
}
