using Application.Commons.Settings;
using Application.Commons.Utils;
using Application.Interfaces.ServicesClients;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Infrastructure.ServicesClients
{
    public class UserServiceClient : IUserServiceClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserServiceClient(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public UserIdentityModel? GetUserIdentity()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user is null) return null;

            return new UserIdentityModel
            {
                Id = user.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value,
                Name = user.FindFirst(JwtRegisteredClaimNames.Name)?.Value,
                FirstName = user.FindFirst(ClaimTypes.GivenName)?.Value,
                Surname = user.FindFirst(ClaimTypes.Surname)?.Value,
                Email = user.FindFirst(ClaimTypes.Email)?.Value,
                Roles = user.FindAll(ClaimTypes.Role)?.Select(c => c.Value).ToArray(),
                IsEmployee = bool.TryParse(user.FindFirst(
                    GeneralConstants.CLAIM_IS_EMPLOYEE)?.Value, out var isEmployee) && isEmployee
            };
        }
    }
}
