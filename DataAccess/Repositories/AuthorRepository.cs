using BusinessObject;
using DataAccess.Daos;
using DataAccess.Repositories.Interfaces;
using System.Collections.Generic;

namespace DataAccess.Repositories
{

    public class AuthorRepository : IAuthorRepository
    {
        public void DeleteAuthor(ApplicationDbContext dbContext, Author author) => AuthorDAO.DeleteAuthor(dbContext, author);

        public Author FindAuthorById(ApplicationDbContext dbContext, int id) => AuthorDAO.FindAuthorById(dbContext, id);

        public List<Author> GetAuthors(ApplicationDbContext dbContext) => AuthorDAO.GetAuthors(dbContext);

        public void SaveAuthor(ApplicationDbContext dbContext, Author author) => AuthorDAO.SaveAuthor(dbContext, author);

        public void UpdateAuthor(ApplicationDbContext dbContext, Author author) => AuthorDAO.UpdateAuthor(dbContext, author);
    }
}
