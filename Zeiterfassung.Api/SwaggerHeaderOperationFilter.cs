using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
/// <summary>
/// https://stackoverflow.com/questions/41493130/web-api-how-to-add-a-header-parameter-for-all-api-in-swagger
/// </summary>
public class SwaggerHeaderOperationFilter : IOperationFilter
{
    /// <inheritdoc/>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters ??= [];

        var allowAnonymous = context.MethodInfo.GetCustomAttribute<AllowAnonymousAttribute>();

        if (allowAnonymous is not null)
            return;

        var existing = operation.Parameters.FirstOrDefault(p => p.In == ParameterLocation.Header && p.Name == ApiKeyMiddleware.ApiKeyHeaderName);
        // remove description from [FromHeader] argument attribute
        if (existing is not null)
        {
            operation.Parameters.Remove(existing);
        }

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = ApiKeyMiddleware.ApiKeyHeaderName,
            In = ParameterLocation.Header,
            Required = true,
            Schema = new OpenApiSchema
            {
                Type = "string",
                Default = null,
            }
        });
    }
}