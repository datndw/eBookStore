using AutoMapper;
using BusinessObject;
using BusinessObject.DTOs;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DataAccess.Repositories
{

    public class AuthorRepository : IAuthorRepository
    {
        private readonly IMapper _mapper;
        public AuthorRepository(IMapper mapper)
        {
            _mapper = mapper;
        }
        public void DeleteAuthor(ApplicationDbContext dbContext, AuthorDTO rawAuthor)
        {
            try
            {
                var a = dbContext.Authors.SingleOrDefault(e => e.Id.Equals(rawAuthor.Id));
                if (a != null)
                {
                    dbContext.Authors.Remove(a);
                }
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public AuthorDTO FindAuthorById(ApplicationDbContext dbContext, int id)
        {
            var author = new AuthorDTO();
            try
            {
                var rawAuthor = dbContext.Authors.AsNoTracking().FirstOrDefault(e => e.Id == id);
                author = _mapper.Map<AuthorDTO>(rawAuthor);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return author;
        }

        public List<AuthorDTO> GetAuthors(ApplicationDbContext dbContext)
        {
            List<AuthorDTO> authors;
            try
            {
                authors = dbContext.Authors.AsNoTracking().Select(author => _mapper.Map<AuthorDTO>(author)).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return authors;
        }

        public void SaveAuthor(ApplicationDbContext dbContext, AuthorDTO rawAuthor)
        {
            try
            {
                Author author = dbContext.Authors.FirstOrDefault(p => p.Id == rawAuthor.Id);
                author = _mapper.Map(rawAuthor, author);
                dbContext.Authors.Add(author);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateAuthor(ApplicationDbContext dbContext, AuthorDTO rawAuthor)
        {
            try
            {
                Author author = dbContext.Authors.FirstOrDefault(p => p.Id == rawAuthor.Id);
                author = _mapper.Map(rawAuthor, author);
                dbContext.Authors.Update(author);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
