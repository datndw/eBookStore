using DataAccess.Repositories.Interfaces;
using DataAccess;
using EBookStoreWebAPI.Helpers;
using EBookStoreWebAPI.Models;
using EBookStoreWebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EBookStoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly AuthenticationService _authenticationService;

        public AuthenticationController(ApplicationDbContext dbContext,
            IUserRepository userRepository,
            IOptions<AppSettings> appSettings,
            IOptions<DefaultAccount> adminAccount)
        {
            _authenticationService = new AuthenticationService(dbContext,userRepository,appSettings, adminAccount);
        }

        [HttpPost]
        public IActionResult Authenticate([FromBody] AuthenticateRequest model)
        {
            var response = _authenticationService.Authenticate(model);

            if (response == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            return Ok(response);
        }
    }
}
