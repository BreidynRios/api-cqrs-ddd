using Application.Commons.Settings;
using Application.Commons.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace WebApi.Middlewares.Security
{
    public class ApiKeyAuthenticationHandler : CustomAuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly ICollection<AuthorizedApiKey> _authorizedApiKey;

        public ApiKeyAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            IOptions<SecuritySettings> optionsSecurity)
            : base(options, logger, encoder)
        {
            _authorizedApiKey = optionsSecurity.Value.AuthorizedApiKeys;
        }

        protected override async Task<(bool, ClaimsPrincipal?)> ValidateAsync(string? credential)
        {
            if (string.IsNullOrWhiteSpace(credential))
                return (false, null);

            var client = _authorizedApiKey.FirstOrDefault(a => a.ApiKey == credential);
            if (client is null) return (false, null);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, client.ApiKey),
                new Claim(JwtRegisteredClaimNames.Name, client.Application),
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);

            return await Task.FromResult((true, principal));
        }

        protected override string GetHeaderName()
        {
            return GeneralConstants.X_API_KEY;
        }
    }
}
