using BusinessObject;
using System.Collections.Generic;

namespace DataAccess.Repositories.Interfaces
{
    public interface IPublisherRepository
    {
        void DeletePublisher(ApplicationDbContext dbContext, Publisher publisher);
        Publisher FindPublisherById(ApplicationDbContext dbContext, int id);
        List<Publisher> GetPublishers(ApplicationDbContext dbContext);
        void SavePublisher(ApplicationDbContext dbContext, Publisher publisher);
        void UpdatePublisher(ApplicationDbContext dbContext, Publisher publisher);
    }
}
