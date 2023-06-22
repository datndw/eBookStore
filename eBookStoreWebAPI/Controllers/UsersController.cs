using AutoMapper;
using BusinessObject;
using BusinessObject.DTOs;
using DataAccess;
using DataAccess.Repositories.Interfaces;
using EBookStoreWebAPI.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace EBookStoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ODataController
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IUserRepository _userRepository;

        public UsersController(ApplicationDbContext dbContext, IUserRepository userRepository)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
            dbContext.ChangeTracker.QueryTrackingBehavior = Microsoft.EntityFrameworkCore.QueryTrackingBehavior.NoTracking;
        }
        [Authorize("Admin")]
        [EnableQuery(PageSize = 5)]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_userRepository.GetUsers(_dbContext));
        }

        [Authorize]
        [EnableQuery]
        [HttpGet("{key:int}")]
        public IActionResult Get([FromODataUri] int key)
        {
            return Ok(_userRepository.FindUserById(_dbContext, key));
        }

        [EnableQuery]
        [HttpPost]
        public IActionResult Post([FromBody] UserForm user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            _userRepository.SaveUser(_dbContext, user);
            return Ok("User created!");
        }

        [Authorize]
        [EnableQuery]
        [HttpPut("{key:int}")]
        public IActionResult Put([FromODataUri] int key, [FromBody] UserDTO user)
        {
            var existedUser = _userRepository.FindUserById(_dbContext, key);
            if (existedUser == null)
            {
                return NotFound();
            }

            _userRepository.UpdateUser(_dbContext, user);
            return Ok("User updated!");
        }

        [Authorize("Admin")]
        [EnableQuery]
        [HttpDelete("{key:int}")]
        public IActionResult Delete([FromODataUri] int key)
        {
            var user = _userRepository.FindUserById(_dbContext, key);
            if (user == null)
            {
                return NotFound();
            }
            _userRepository.DeleteUser(_dbContext, user);
            return Ok("User deleted!");
        }
    }
}
