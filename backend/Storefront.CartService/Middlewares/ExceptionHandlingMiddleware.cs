using Storefront.CartService.Behaviours;

namespace Storefront.CartService.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");

                var statusCode = ex switch
                {
                    ArgumentNullException => StatusCodes.Status400BadRequest,
                    UnidentifiedUserException => StatusCodes.Status401Unauthorized,
                    UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                    _ => StatusCodes.Status500InternalServerError
                };

                var response = new
                {
                    StatusCode = statusCode,
                    Message = ex.Message,
                    ErrorType = ex.GetType().Name
                };

                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }

}
