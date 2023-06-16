using BusinessObject;

namespace EBookStoreWebAPI.Models
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public Role Role { get; set; }
        public string Token { get; set; }

        public AuthenticateResponse()
        {

        }

        public AuthenticateResponse(User user, string token)
        {
            Id = user.Id;
            Role = user.Role;
            EmailAddress = user.EmailAddress;
            Token = token;
        }
    }
}
