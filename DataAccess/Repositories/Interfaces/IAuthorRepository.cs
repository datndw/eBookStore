using BusinessObject;
using System.Collections.Generic;

namespace DataAccess.Repositories.Interfaces
{
    public interface IAuthorRepository
    {
        void DeleteAuthor(ApplicationDbContext dbContext, Author author);
        Author FindAuthorById(ApplicationDbContext dbContext, int id);
        List<Author> GetAuthors(ApplicationDbContext dbContext);
        void SaveAuthor(ApplicationDbContext dbContext, Author author);
        void UpdateAuthor(ApplicationDbContext dbContext, Author author);
    }
}
