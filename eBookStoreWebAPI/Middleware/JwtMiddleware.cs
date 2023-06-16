using BusinessObject;
using DataAccess;
using DataAccess.Repositories.Interfaces;
using EBookStoreWebAPI.Helpers;
using EBookStoreWebAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookStoreWebAPI.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly DefaultAccount _adminAccount;
        private readonly AppSettings _appSettings;

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings, IOptions<DefaultAccount> adminAccount)
        {
            _next = next;
            _adminAccount = adminAccount.Value;
            _appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context, IAuthenticationService authenticationService, ApplicationDbContext dbContext, IUserRepository userRepository)
        {
            string token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                AttachUserToContext(context, authenticationService, dbContext, userRepository, token);
            }

            await _next(context);
        }

        private void AttachUserToContext(HttpContext context, IAuthenticationService userService, ApplicationDbContext dbContext, IUserRepository userRepository, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                byte[] key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out var validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                int userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
                string email = jwtToken.Claims.First(x => x.Type == "email").Value;
                // attach user to context on successful jwt validation
                if (userId == 0 && email.Equals(_adminAccount.EmailAddress))
                {
                    context.Items["User"] = new User()
                    {
                        Id = 0,
                        RoleId = 1,
                        Role = new Role() {Id = 1, Desc = "Admin" },
                        EmailAddress = _adminAccount.EmailAddress,
                        Password = _adminAccount.Password
                    };
                    return;
                }
                if (userId > 0)
                {
                    context.Items["User"] = userRepository.FindUserById(dbContext, userId);
                    return;
                }
            }
            catch
            {

            }
        }
    }
}
