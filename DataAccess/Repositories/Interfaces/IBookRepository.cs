using BusinessObject;
using System.Collections.Generic;

namespace DataAccess.Repositories.Interfaces
{
    public interface IBookRepository
    {
        void DeleteBook(ApplicationDbContext dbContext, Book book);
        Book FindBookById(ApplicationDbContext dbContext, int id);
        List<Book> GetBooks(ApplicationDbContext dbContext);
        void SaveBook(ApplicationDbContext dbContext, Book book);
        void UpdateBook(ApplicationDbContext dbContext, Book book);
    }
}
