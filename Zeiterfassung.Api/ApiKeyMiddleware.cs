using Microsoft.AspNetCore.Authorization;
using System.Net;
using Zeiterfassung.Api.Database;

public class ApiKeyMiddleware(AppDbContext db, CallerContext callerContext) : IMiddleware
{
    internal const string ApiKeyHeaderName = "X-Api-Key";

    /// <inheritdoc/>
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var metaData = context.GetEndpoint()?.Metadata;

        if (metaData is not null)
        {
            // Allow all if AllowAnonymousAttribute
            if (!metaData.Any(x => x.GetType() == typeof(AllowAnonymousAttribute)))
            {
                if (!(context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var apiKey) && Guid.TryParse(apiKey, out var parsedApiKey)))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

#if DEBUG
                    await context.Response.WriteAsync("API Key is missing");
#endif

                    return;
                }

                var user = await db.Users.FindAsync(parsedApiKey);

                if (user is null)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

#if DEBUG
                    await context.Response.WriteAsync("Invalid API Key");
#endif

                    return;
                }

                callerContext.User = user;
            }
        }

        await next(context);
    }
}
