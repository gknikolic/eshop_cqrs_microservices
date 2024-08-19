using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Refit;
using System.Net;
using System.Text;

namespace Shopping.Web.Exceptions;

public class ExceptionHandler(RequestDelegate _next, ILogger<ExceptionHandler> _logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ApiException apiEx) when (apiEx.StatusCode == HttpStatusCode.Unauthorized)
        {
            //var context = httpContextAccessor.HttpContext;

            // Get the current URL that caused the 401
            var currentUrl = context.Request.Path + context.Request.QueryString;

            // Redirect to the login page with returnUrl
            var loginUrl = $"/Account/Login?returnUrl={Uri.EscapeDataString(currentUrl)}";
            context.Response.Redirect(loginUrl);
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

        // Check if it is an AJAX request
        if (context.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            // Handle AJAX request
            // For example, you can return a JSON response
            var jsonResponse = new { error = exception.Message };
            var jsonBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(jsonResponse));
            context.Response.ContentType = "application/json";
            context.Response.Body.Write(jsonBytes, 0, jsonBytes.Length);
        }
        return Task.CompletedTask;
    }
}
