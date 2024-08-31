using Application.Commons.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace WebApi.Middlewares.Security
{
    public class BearerTokenAuthenticationHandler : CustomAuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly TokenValidationParameters _tokenValidationParameters;

        public BearerTokenAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            TokenValidationParameters tokenValidationParameters)
            : base(options, logger, encoder)
        {
            _tokenValidationParameters = tokenValidationParameters;
        }

        protected override async Task<(bool, ClaimsPrincipal?)> ValidateAsync(string? credential)
        {
            try
            {
                var token = credential?["Bearer ".Length..].Trim();
                if (string.IsNullOrWhiteSpace(token))
                    return (false, null);

                var tokenHandler = new JwtSecurityTokenHandler();

                var principal = tokenHandler.ValidateToken(token, 
                    _tokenValidationParameters, out var securityToken);

                var success = securityToken is JwtSecurityToken;
                return await Task.FromResult((success, principal));
            }
            catch (Exception)
            {
                return (false, null);
            }
        }

        protected override string GetHeaderName()
        {
            return GeneralConstants.AUTHORIZATION;
        }
    }
}
