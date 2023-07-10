using BusinessObject;
using BusinessObject.DTOs;
using System.Collections.Generic;

namespace DataAccess.Repositories.Interfaces
{
    public interface IPublisherRepository
    {
        void DeletePublisher(ApplicationDbContext dbContext, PublisherDTO publisher);
        PublisherDTO FindPublisherById(ApplicationDbContext dbContext, int id);
        List<PublisherDTO> GetPublishers(ApplicationDbContext dbContext);
        void SavePublisher(ApplicationDbContext dbContext, PublisherDTO publisher);
        void UpdatePublisher(ApplicationDbContext dbContext, PublisherDTO publisher);
    }
}
