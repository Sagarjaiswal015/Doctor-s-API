namespace AppointmentBooking.Middleware;

public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
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
            _logger.LogError(ex, "Unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = new
        {
            message = "An error occurred processing your request",
            details = exception.Message
        };

        if (exception is UnauthorizedAccessException)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            response = new { message = "Unauthorized", details = exception.Message };
        }
        else if (exception is KeyNotFoundException)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            response = new { message = "Not found", details = exception.Message };
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        }

        return context.Response.WriteAsJsonAsync(response);
    }
}
