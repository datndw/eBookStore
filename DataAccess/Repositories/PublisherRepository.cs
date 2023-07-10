using AutoMapper;
using BusinessObject;
using BusinessObject.DTOs;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DataAccess.Repositories
{
    public class PublisherRepository : IPublisherRepository
    {
        private readonly IMapper _mapper;
        public PublisherRepository(IMapper mapper)
        {
            _mapper = mapper;
        }
        public void DeletePublisher(ApplicationDbContext dbContext, PublisherDTO rawPublisher)
        {
            try
            {
                var p = dbContext.Publishers.SingleOrDefault(e => e.Id.Equals(rawPublisher.Id));
                if (p != null)
                {
                    dbContext.Publishers.Remove(p);
                }
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public PublisherDTO FindPublisherById(ApplicationDbContext dbContext, int id)
        {
            var publisher = new PublisherDTO();
            try
            {
                var rawPublisher = dbContext.Publishers.AsNoTracking().FirstOrDefault(e => e.Id == id);
                publisher = _mapper.Map<PublisherDTO>(rawPublisher);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return publisher;
        }

        public List<PublisherDTO> GetPublishers(ApplicationDbContext dbContext)
        {
            List<PublisherDTO> publishers;
            try
            {
                publishers = dbContext.Publishers.AsNoTracking().Select(publisher => _mapper.Map<PublisherDTO>(publisher)).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return publishers;
        }

        public void SavePublisher(ApplicationDbContext dbContext, PublisherDTO rawPublisher)
        {
            try
            {
                Publisher publisher = dbContext.Publishers.FirstOrDefault(p => p.Id == rawPublisher.Id);
                publisher = _mapper.Map(rawPublisher, publisher);
                dbContext.Publishers.Add(publisher);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdatePublisher(ApplicationDbContext dbContext, PublisherDTO rawPublisher)
        {
            try
            {
                Publisher publisher = dbContext.Publishers.FirstOrDefault(p => p.Id == rawPublisher.Id);
                publisher = _mapper.Map(rawPublisher, publisher);
                dbContext.Publishers.Update(publisher);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
