using BusinessObject;
using DataAccess;
using DataAccess.Repositories.Interfaces;
using EBookStoreWebAPI.Helpers;
using EBookStoreWebAPI.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EBookStoreWebAPI.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IUserRepository _userRepository;
        private readonly AppSettings _appSettings;
        private readonly DefaultAccount _adminAccount;

        public AuthenticationService(
            ApplicationDbContext dbContext,
            IUserRepository userRepository,
            IOptions<AppSettings> appSettings,
            IOptions<DefaultAccount> adminAccount)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
            _appSettings = appSettings.Value;
            _adminAccount = adminAccount.Value;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            User user;

            if (model.EmailAddress.Equals(_adminAccount.EmailAddress) && model.Password.Equals(_adminAccount.Password))
            {
                user = new User()
                {
                    Id = 0,
                    EmailAddress = _adminAccount.EmailAddress,
                    Password = _adminAccount.Password,
                    Role = new Role() { Id = 1, Desc = "Admin" }
                };
            }
            else
            {
                user = _userRepository.Login(_dbContext, model.EmailAddress, model.Password);
            }

            if (user == null)
            {
                return null;
            }

            string token = GenerateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()),
                    new Claim("role", user.Role.Desc),
                    new Claim("email", user.EmailAddress) }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
