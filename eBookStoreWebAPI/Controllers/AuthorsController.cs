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
    [Authorize("Admin")]
    public class AuthorsController : ODataController
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IAuthorRepository _authorRepository;

        public AuthorsController(ApplicationDbContext dbContext, IAuthorRepository authorRepository)
        {
            _dbContext = dbContext;
            _authorRepository = authorRepository;
            dbContext.ChangeTracker.QueryTrackingBehavior = Microsoft.EntityFrameworkCore.QueryTrackingBehavior.NoTracking;
        }

        [EnableQuery(PageSize = 5)]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_authorRepository.GetAuthors(_dbContext));
        }

        [EnableQuery]
        [HttpGet("{key:int}")]
        public IActionResult Get([FromODataUri] int key)
        {
            return Ok(_authorRepository.FindAuthorById(_dbContext, key));
        }

        [EnableQuery]
        [HttpPost]
        public IActionResult Post([FromBody] AuthorDTO author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            _authorRepository.SaveAuthor(_dbContext, author);
            return Created(author);
        }

        [EnableQuery]
        [HttpPut("{key:int}")]
        public IActionResult Put([FromODataUri] int key, [FromBody] AuthorDTO author)
        {
            var existedAuthor = _authorRepository.FindAuthorById(_dbContext, key);
            if (existedAuthor == null)
            {
                return NotFound();
            }

            _authorRepository.UpdateAuthor(_dbContext, author);
            return Ok();
        }

        [EnableQuery]
        [HttpDelete("{key:int}")]
        public IActionResult Delete([FromODataUri] int key)
        {
            var author = _authorRepository.FindAuthorById(_dbContext, key);
            if (author == null)
            {
                return NotFound();
            }
            _authorRepository.DeleteAuthor(_dbContext, author);
            return Ok();
        }
    }
}
