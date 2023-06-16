using BusinessObject;
using DataAccess.Daos;
using DataAccess.Repositories.Interfaces;
using System.Collections.Generic;

namespace DataAccess.Repositories
{
    public class PublisherRepository : IPublisherRepository
    {
        public void DeletePublisher(ApplicationDbContext dbContext, Publisher publisher) => PublisherDAO.DeletePublisher(dbContext, publisher);

        public Publisher FindPublisherById(ApplicationDbContext dbContext, int id) => PublisherDAO.FindPublisherById(dbContext, id);

        public List<Publisher> GetPublishers(ApplicationDbContext dbContext) => PublisherDAO.GetPublishers(dbContext);

        public void SavePublisher(ApplicationDbContext dbContext, Publisher publisher) => PublisherDAO.SavePublisher(dbContext, publisher);

        public void UpdatePublisher(ApplicationDbContext dbContext, Publisher publisher) => PublisherDAO.UpdatePublisher(dbContext, publisher);
    }
}
