using Shopping.Web.Exceptions;

namespace Shopping.Web.Helpers;

public class SignInAfterRebuildMiddleware(RequestDelegate _next, ILogger<ExceptionHandler> _logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var authService = context.RequestServices.GetRequiredService<IAuthService>();
        var tokenProvider = context.RequestServices.GetRequiredService<ITokenProvider>();

        var token = tokenProvider.GetToken();
        if (string.IsNullOrEmpty(token))
        {
            // validate jwt token
            var principal = context.User;
        }

        await _next(context);
    }
}
