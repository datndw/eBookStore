using BusinessObject;
using DataAccess.Daos;
using DataAccess.Repositories.Interfaces;
using System.Collections.Generic;

namespace DataAccess.Repositories
{
    public class BookRepository : IBookRepository
    {
        public void DeleteBook(ApplicationDbContext dbContext, Book book) => BookDAO.DeleteBook(dbContext, book);

        public Book FindBookById(ApplicationDbContext dbContext, int id) => BookDAO.FindBookById(dbContext, id);

        public List<Book> GetBooks(ApplicationDbContext dbContext) => BookDAO.GetBooks(dbContext);

        public void SaveBook(ApplicationDbContext dbContext, Book book) => BookDAO.SaveBook(dbContext, book);

        public void UpdateBook(ApplicationDbContext dbContext, Book book) => BookDAO.UpdateBook(dbContext, book);
    }
}
