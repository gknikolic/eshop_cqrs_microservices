namespace Shopping.Web.Exceptions;

public class ExceptionHandler(RequestDelegate _next, ILogger<ExceptionHandler> _logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = StatusCodes.Status500InternalServerError;

        // Log and prepare for redirection to error page
        var errorPageUrl = $"/Error?message={Uri.EscapeDataString(exception.Message)}";

        context.Response.Redirect(errorPageUrl);
        return Task.CompletedTask;
    }
}
