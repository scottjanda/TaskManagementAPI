namespace TaskManagementAPI.Middleware
{
    public class ApiKeyMiddleware
    {

        private readonly string _testSecret;

        private readonly RequestDelegate _next;
        private
        const string APIKEY = "XApiKey";
        public ApiKeyMiddleware(RequestDelegate next, string testSecret)
        {
            _next = next;
            _testSecret = testSecret;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(APIKEY, out
                    var extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Api Key was not provided ");
                return;
            }
            var appSettings = context.RequestServices.GetRequiredService<IConfiguration>();
            var apiKey = _testSecret;
            if (!apiKey.Equals(extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized client");
                return;
            }
            await _next(context);
        }
    }
}
