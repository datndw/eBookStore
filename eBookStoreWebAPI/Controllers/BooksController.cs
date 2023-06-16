using BusinessObject;
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
    public class BooksController : ODataController
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IBookRepository _bookRepository;

        public BooksController(ApplicationDbContext dbContext, IBookRepository bookRepository)
        {
            _dbContext = dbContext;
            _bookRepository = bookRepository;
            dbContext.ChangeTracker.QueryTrackingBehavior = Microsoft.EntityFrameworkCore.QueryTrackingBehavior.NoTracking;
        }

        [EnableQuery(PageSize = 5)]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_bookRepository.GetBooks(_dbContext));
        }

        [EnableQuery]
        [HttpGet("{key:int}")]
        public IActionResult Get([FromODataUri] int key, string version)
        {
            return Ok(_bookRepository.FindBookById(_dbContext, key));
        }

        [EnableQuery]
        [HttpPost]
        public IActionResult Post([FromBody] Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            _bookRepository.SaveBook(_dbContext, book);
            return Created(book);
        }

        [EnableQuery]
        [HttpPut("{key:int}")]
        public IActionResult Put([FromODataUri] int key, [FromBody] Book book)
        {
            var existedBook = _bookRepository.FindBookById(_dbContext, key);
            if (existedBook == null)
            {
                return NotFound();
            }

            _bookRepository.UpdateBook(_dbContext, book);
            return Ok();
        }

        [EnableQuery]
        [HttpDelete("{key:int}")]
        public IActionResult Delete([FromODataUri] int key)
        {
            var book = _bookRepository.FindBookById(_dbContext, key);
            if (book == null)
            {
                return NotFound();
            }
            _bookRepository.DeleteBook(_dbContext, book);
            return Ok();
        }
    }
}
