using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace WebApi.Middlewares.Security
{
    public abstract class CustomAuthenticationHandler<TOptions> : AuthenticationHandler<TOptions>
        where TOptions : AuthenticationSchemeOptions, new()
    {
        protected CustomAuthenticationHandler(
            IOptionsMonitor<TOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder)
            : base(options, logger, encoder)
        {
        }

        protected abstract string GetHeaderName();
        protected abstract Task<(bool, ClaimsPrincipal?)> ValidateAsync(string? credential);

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(GetHeaderName(), out var credential))
                return AuthenticateResult.Fail("Authentication header missing.");

            var (success, principalClaim) = await ValidateAsync(credential);
            if (!success)
                return AuthenticateResult.Fail("Invalid credentials.");

            return AuthenticateResult.Success(new AuthenticationTicket(principalClaim!, Scheme.Name));
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = StatusCodes.Status401Unauthorized;
            Response.ContentType = "application/json";
            var response = new { error = "Authentication credentials were not provided." };
            await Response.WriteAsJsonAsync(response);
        }

        protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = StatusCodes.Status403Forbidden;
            Response.ContentType = "application/json";
            var response = new { error = "You do not have permission to access this resource." };
            await Response.WriteAsJsonAsync(response);
        }
    }

}
