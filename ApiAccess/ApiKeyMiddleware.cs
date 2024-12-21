    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;

    namespace JuniorDotNetTestTaskServiceHostingProcessEquipment.ApiAccess
    {
        public class ApiKeyMiddleware
        {
            private readonly RequestDelegate _next;
            private readonly string _apiKey;

            public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
            {
                _next = next;
                _apiKey = configuration["ApiKey"] ?? throw new ArgumentNullException("ApiKey is not configured.");
            }

            public async Task InvokeAsync(HttpContext context)
            {
                if (context.Request.Path.StartsWithSegments("/api/GetAppSettings"))
                {
                    await _next(context);
                    return;
                }

                const string ApiKeyHeaderName = "ApiKey";

                if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("API key is missing.");
                    return;
                }

                if (!string.Equals(extractedApiKey, _apiKey, StringComparison.Ordinal))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Invalid API key.");
                    return;
                }

                await _next(context);
            }
        }
    }



