using BusinessObject.DTOs;

namespace DataAccess.Repositories.Interfaces
{
    public interface IBookRepository
    {
        void DeleteBook(ApplicationDbContext dbContext, BookDTO book);
        BookDTO FindBookById(ApplicationDbContext dbContext, int id);
        List<BookDTO> GetBooks(ApplicationDbContext dbContext);
        void SaveBook(ApplicationDbContext dbContext, BookDTO book);
        void UpdateBook(ApplicationDbContext dbContext, BookDTO book);
    }
}
