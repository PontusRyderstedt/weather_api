namespace WeatherApi.Services
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate next;
        private const string ApiKeyHeaderName = "X-Api-Key";
        private const string SuperSecretApiKey = "SuperSecret123"; // In real applications, store this securely

        public ApiKeyMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context, IConfiguration config)
        {
            if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedKey))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("API Key is missing.");
                return;
            }

            if (!string.Equals(extractedKey, SuperSecretApiKey))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Invalid API Key.");
                return;
            }

            await next(context);
        }
    }
}