using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Daos
{
    public class BookAuthorDAO
    {
        private static BookAuthorDAO _instance = null;
        private static readonly object _instanceLock = new();

        public static BookAuthorDAO Instance
        {
            get
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
                    {
                        _instance = new BookAuthorDAO();
                    }
                }
                return _instance;
            }
        }

        public static List<BookAuthor> GetBookAuthors(ApplicationDbContext dbContext)
        {
            List<BookAuthor> bookAuthors;
            try
            {
                bookAuthors = dbContext.BookAuthors.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return bookAuthors;
        }

        public static BookAuthor FindBookAuthorById(ApplicationDbContext dbContext, int authorId, int bookId)
        {
            var bookAuthor = new BookAuthor();
            try
            {
                bookAuthor = dbContext.BookAuthors.AsNoTracking().FirstOrDefault(e => e.AuthorId == authorId && e.BookId == bookId);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return bookAuthor;
        }

        public static void SaveBookAuthor(ApplicationDbContext dbContext, BookAuthor bookAuthor)
        {
            try
            {
                dbContext.BookAuthors.Add(bookAuthor);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void UpdateBookAuthor(ApplicationDbContext dbContext, BookAuthor bookAuthor)
        {
            try
            {
                dbContext.Entry(bookAuthor).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void DeleteBookAuthor(ApplicationDbContext dbContext, BookAuthor bookAuthor)
        {
            try
            {
                var c = dbContext.BookAuthors.SingleOrDefault(e => e.AuthorId.Equals(bookAuthor.AuthorId) && e.BookId.Equals(bookAuthor.BookId));
                if (c != null)
                {
                    dbContext.BookAuthors.Remove(c);
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
