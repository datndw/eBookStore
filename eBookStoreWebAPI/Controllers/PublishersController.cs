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
    public class PublishersController : ODataController
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IPublisherRepository _publisherRepository;

        public PublishersController(ApplicationDbContext dbContext, IPublisherRepository publisherRepository)
        {
            _dbContext = dbContext;
            _publisherRepository = publisherRepository;
            dbContext.ChangeTracker.QueryTrackingBehavior = Microsoft.EntityFrameworkCore.QueryTrackingBehavior.NoTracking;
        }

        [EnableQuery(PageSize = 5)]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_publisherRepository.GetPublishers(_dbContext));
        }

        [EnableQuery]
        [HttpGet("{key:int}")]
        public IActionResult Get([FromODataUri] int key, string version)
        {
            return Ok(_publisherRepository.FindPublisherById(_dbContext, key));
        }

        [EnableQuery]
        [HttpPost]
        public IActionResult Post([FromBody] Publisher publisher)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            _publisherRepository.SavePublisher(_dbContext, publisher);
            return Created(publisher);
        }

        [EnableQuery]
        [HttpPut("{key:int}")]
        public IActionResult Put([FromODataUri] int key, [FromBody] Publisher publisher)
        {
            var existedPublisher = _publisherRepository.FindPublisherById(_dbContext, key);
            if (existedPublisher == null)
            {
                return NotFound();
            }

            _publisherRepository.UpdatePublisher(_dbContext, publisher);
            return Ok();
        }

        [EnableQuery]
        [HttpDelete("{key:int}")]
        public IActionResult Delete([FromODataUri] int key)
        {
            var publisher = _publisherRepository.FindPublisherById(_dbContext, key);
            if (publisher == null)
            {
                return NotFound();
            }
            _publisherRepository.DeletePublisher(_dbContext, publisher);
            return Ok();
        }
    }
}
