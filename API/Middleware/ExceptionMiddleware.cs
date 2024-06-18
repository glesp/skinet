using System.Net;
using System.Text.Json;
using API.Errors;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, 
        IHostEnvironment env)
        {
            _env = env;
            _logger = logger;
            _next = next;
            
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // if no exception, moves onto next stage
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message); //logs to console
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; //set status code to be internal server error

                var response = _env.IsDevelopment() // if development mode, give more error info
                    ? new ApiException((int)HttpStatusCode.InternalServerError, ex.Message, 
                    ex.StackTrace.ToString()) 
                    : new ApiException((int)HttpStatusCode.InternalServerError);

                var options = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};

                var json = JsonSerializer.Serialize(response, options);

                await context.Response.WriteAsync(json);
            }
        }
    }
}