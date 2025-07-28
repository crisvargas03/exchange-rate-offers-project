using Serilog;

namespace OrchestratorMicroService.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Serilog.ILogger _logger; // Change type to ILogger
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
            _logger = new LoggerConfiguration()
               .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.Console()
                .CreateLogger();
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An exception occurred while processing the request.");
                await HandleExceptionAsync(context, ex);
            }
        }
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            var result = new
            {
                error = "An unexpected error occurred.",
                details = exception.Message
            };
            return context.Response.WriteAsJsonAsync(result);
        }
    }
}
