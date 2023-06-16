using EBookStoreWebAPI.Models;

namespace EBookStoreWebAPI.Services
{
    public interface IAuthenticationService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
    }
}
