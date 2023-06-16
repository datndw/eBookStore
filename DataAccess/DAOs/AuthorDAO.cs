using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Daos
{
    public class AuthorDAO
    {
        private static AuthorDAO instance = null;
        private static readonly object instanceLock = new object();

        public static AuthorDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new AuthorDAO();
                    }
                }
                return instance;
            }
        }

        public static List<Author> GetAuthors(ApplicationDbContext dbContext)
        {
            List<Author> authors;
            try
            {
                authors = dbContext.Authors.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return authors;
        }

        public static Author FindAuthorById(ApplicationDbContext dbContext, int id)
        {
            var author = new Author();
            try
            {
                author = dbContext.Authors.AsNoTracking().FirstOrDefault(e => e.Id == id);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return author;
        }

        public static void SaveAuthor(ApplicationDbContext dbContext, Author author)
        {
            try
            {
                dbContext.Authors.Add(author);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void UpdateAuthor(ApplicationDbContext dbContext, Author author)
        {
            try
            {
                dbContext.Entry(author).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void DeleteAuthor(ApplicationDbContext dbContext, Author author)
        {
            try
            {
                var c = dbContext.Authors.AsNoTracking().SingleOrDefault(e => e.Id.Equals(author.Id));
                if (c != null)
                {
                    dbContext.Authors.Remove(c);
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
