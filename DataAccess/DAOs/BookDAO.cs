using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Daos
{
    public class BookDAO
    {
        private static BookDAO instance = null;
        private static readonly object instanceLock = new();

        public static BookDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new BookDAO();
                    }
                }
                return instance;
            }
        }

        public static List<Book> GetBooks(ApplicationDbContext dbContext)
        {
            List<Book> books;
            try
            {
                books = dbContext.Books.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return books;
        }

        public static Book FindBookById(ApplicationDbContext dbContext, int id)
        {
            var book = new Book();
            try
            {
                book = dbContext.Books.AsNoTracking().FirstOrDefault(e => e.Id == id);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return book;
        }

        public static void SaveBook(ApplicationDbContext dbContext, Book book)
        {
            try
            {
                dbContext.Books.Add(book);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void UpdateBook(ApplicationDbContext dbContext, Book book)
        {
            try
            {
                dbContext.Entry(book).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void DeleteBook(ApplicationDbContext dbContext, Book book)
        {
            try
            {
                var c = dbContext.Books.SingleOrDefault(e => e.Id.Equals(book.Id));
                if (c != null)
                {
                    dbContext.Books.Remove(c);
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
