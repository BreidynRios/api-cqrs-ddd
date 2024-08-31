using Application.Commons.Settings;
using Application.Commons.Utils;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Features.Securities.Queries.GetBearerToken
{
    public class GetBearerTokenQueryHandler : IRequestHandler<GetBearerTokenQuery, string>
    {
        private readonly JwtConfig _jwtConfig;

        public GetBearerTokenQueryHandler(IOptions<SecuritySettings> options)
        {
            _jwtConfig = options.Value.JwtConfig;
        }

        public async Task<string> Handle(GetBearerTokenQuery request, CancellationToken cancellationToken)
        {
            var token = GenerateToken(request.ClientId);
            return await Task.FromResult(token);
        }

        protected internal virtual string GenerateToken(string clientId)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, "12345"),
                new(JwtRegisteredClaimNames.Name, "Alan Breidyn Rios Briceño"),
                new(ClaimTypes.GivenName, "Alan Breidyn"),
                new(ClaimTypes.Surname, "Rios Briceño"),
                new(ClaimTypes.Email, "alanr92@gmail.com"),
                new(ClaimTypes.Role, "RolOne,RolTwo"),
                new(GeneralConstants.CLAIM_IS_EMPLOYEE, "true")
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtConfig.ExpiryInMinutes),
                Issuer = _jwtConfig.Issuer,
                Audience = clientId,
                EncryptingCredentials = new EncryptingCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtConfig.EncryptionKey)),
                    JwtConstants.DirectKeyUseAlg,
                    SecurityAlgorithms.Aes256CbcHmacSha512),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtConfig.Key)),
                    SecurityAlgorithms.HmacSha256Signature)                
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
