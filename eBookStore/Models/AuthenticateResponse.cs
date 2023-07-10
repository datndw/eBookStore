using eBookStore.Models.DTOs;

namespace eBookStore.Models
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }

        public AuthenticateResponse()
        {

        }

        public AuthenticateResponse(Credential user, string token)
        {
            Id = user.Id;
            Role = user.Role;
            EmailAddress = user.EmailAddress;
            Token = token;
        }
    }
}
