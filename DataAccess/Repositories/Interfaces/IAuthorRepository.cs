using BusinessObject.DTOs;

namespace DataAccess.Repositories.Interfaces
{
    public interface IAuthorRepository
    {
        void DeleteAuthor(ApplicationDbContext dbContext, AuthorDTO author);
        AuthorDTO FindAuthorById(ApplicationDbContext dbContext, int id);
        List<AuthorDTO> GetAuthors(ApplicationDbContext dbContext);
        void SaveAuthor(ApplicationDbContext dbContext, AuthorDTO author);
        void UpdateAuthor(ApplicationDbContext dbContext, AuthorDTO author);
    }
}
