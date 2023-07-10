using AutoMapper;
using BusinessObject;
using BusinessObject.DTOs;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DataAccess.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly IMapper _mapper;
        public BookRepository(IMapper mapper)
        {
            _mapper = mapper;
        }
        public void DeleteBook(ApplicationDbContext dbContext, BookDTO rawBook)
        {
            try
            {
                var b = dbContext.Books.SingleOrDefault(e => e.Id.Equals(rawBook.Id));
                if (b != null)
                {
                    dbContext.Books.Remove(b);
                }
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public BookDTO FindBookById(ApplicationDbContext dbContext, int id) {
            var book = new BookDTO();
            try
            {
                var rawBook = dbContext.Books.AsNoTracking().FirstOrDefault(e => e.Id == id);
                book = _mapper.Map<BookDTO>(rawBook);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return book;
        }

        public List<BookDTO> GetBooks(ApplicationDbContext dbContext)
        {
            List<BookDTO> books;
            try
            {
                books = dbContext.Books.AsNoTracking().Select(book => _mapper.Map<BookDTO>(book)).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return books;
        }

        public void SaveBook(ApplicationDbContext dbContext, BookDTO rawBook)
        {
            try
            {
                Book book = dbContext.Books.FirstOrDefault(p => p.Id == rawBook.Id);
                book = _mapper.Map(rawBook, book);
                dbContext.Books.Add(book);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public void UpdateBook(ApplicationDbContext dbContext, BookDTO rawBook)
        {
            try
            {
                Book book = dbContext.Books.FirstOrDefault(p => p.Id == rawBook.Id);
                book = _mapper.Map(rawBook, book);
                dbContext.Books.Update(book);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
