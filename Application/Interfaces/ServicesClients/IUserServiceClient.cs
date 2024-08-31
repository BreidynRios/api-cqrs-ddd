using Application.Commons.Settings;

namespace Application.Interfaces.ServicesClients
{
    public interface IUserServiceClient
    {
        UserIdentityModel? GetUserIdentity();
    }
}
