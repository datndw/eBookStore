using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Daos
{
    public class PublisherDAO
    {
        private static PublisherDAO _instance = null;
        private static readonly object _instanceLock = new();

        public static PublisherDAO Instance
        {
            get
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
                    {
                        _instance = new PublisherDAO();
                    }
                }
                return _instance;
            }
        }

        public static List<Publisher> GetPublishers(ApplicationDbContext dbContext)
        {
            List<Publisher> publishers;
            try
            {
                publishers = dbContext.Publishers.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return publishers;
        }

        public static Publisher FindPublisherById(ApplicationDbContext dbContext, int id)
        {
            var publisher = new Publisher();
            try
            {
                publisher = dbContext.Publishers.AsNoTracking().FirstOrDefault(e => e.Id == id);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return publisher;
        }

        public static void SavePublisher(ApplicationDbContext dbContext, Publisher publisher)
        {
            try
            {
                dbContext.Publishers.Add(publisher);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void UpdatePublisher(ApplicationDbContext dbContext, Publisher publisher)
        {
            try
            {
                dbContext.Entry(publisher).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void DeletePublisher(ApplicationDbContext dbContext, Publisher publisher)
        {
            try
            {
                var c = dbContext.Publishers.SingleOrDefault(e => e.Id.Equals(publisher.Id));
                if (c != null)
                {
                    dbContext.Publishers.Remove(c);
                }
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
