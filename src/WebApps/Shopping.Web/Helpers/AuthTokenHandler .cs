using Refit;
using System.Net;
using System.Net.Http.Headers;

namespace Shopping.Web.Helpers;

public class AuthTokenPassingHandler(ITokenProvider tokenProvider, IHttpContextAccessor httpContextAccessor)
    : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
        {
            var token = tokenProvider.GetToken();
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        return await base.SendAsync(request, cancellationToken);

    }
}
