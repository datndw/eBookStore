using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess
{
    public static class DataSource
    {
        private static IList<Publisher> _pulishers { get; set; }
        private static IList<Book> _books { get; set; }
        private static IList<Author> _authors { get; set; }
        private static IList<User> _users { get; set; }
        private static IList<Role> _roles { get; set; }

        public static IList<User> GetUsers()
        {
            if (_users == null)
            {
                _users = new List<User>()
                {
                    new User()
                    {
                        EmailAddress = "datndhe163390@fpt.edu.vn",
                        Password = "123",
                        Source = null,
                        FirstName = "Dat",
                        MiddleName = null,
                        LastName = "Nguyen",
                        RoleId = 1,
                        PublisherId = 1,
                        HireDate = null
                    },
                    new User()
                    {
                        EmailAddress = "datnd27@fpt.com",
                        Password = "123",
                        Source = null,
                        FirstName = "Dat",
                        MiddleName = null,
                        LastName = "Nguyen",
                        RoleId = 1,
                        PublisherId = 1,
                        HireDate = null
                    }
                };
            }

            return _users;
        }

        private static IList<Role> GetRoles()
        {
            if (_roles == null)
            {
                _roles = new List<Role>()
                {
                    new Role()
                    {
                        Desc = "Admin"
                    },

                    new Role()
                    {
                        Desc = "Publisher"
                    },
                    new Role()
                    {
                        Desc = "User"
                    }
                };
            }

            return _roles;
        }

        private static IList<Publisher> GetPublishers()
        {
            if (_pulishers == null)
            {
                _pulishers = new List<Publisher>()
                {
                    new Publisher()
                    {
                        Name = "Dat",
                        City = "Ha Noi",
                        Country = "Viet Nam"
                    },
                    new Publisher()
                    {
                        Name = "John",
                        City = "London",
                        Country = "England"
                    }
                };
            }
            return _pulishers;
        }

        private static IList<Author> GetAuthors()
        {
            if (_authors == null)
            {
                _authors = new List<Author>()
                {
                    new Author {
                        LastName = "Newton",
                        FirstName = "Beau",
                        Phone = "(783) 628-1033",
                        Address = null,
                        City = null,
                        Zip = 451618,
                        EmailAddress = null
                    },
                    new Author {
                        LastName = "Baldwin",
                        FirstName = "Angela",
                        Phone = "(302) 177-2378",
                        Address = null,
                        City = null,
                        Zip = 23858,
                        EmailAddress = null
                    },
                    new Author {
                        LastName = "Hunt",
                        FirstName = "Brenna",
                        Phone = "(768) 699-5335",
                        Address = null,
                        City = null,
                        Zip = 446163,
                        EmailAddress = null
                    },
                    new Author {
                        LastName = "Mcgowan",
                        FirstName = "Diana",
                        Phone = "1-424-372-2387",
                        Address = null,
                        City = null,
                        Zip = 4133,
                        EmailAddress = null
                    },
                    new Author {
                        LastName = "Atkinson",
                        FirstName = "Amos",
                        Phone = "1-714-657-0902",
                        Address = null,
                        City = null,
                        Zip = 12093-168,
                        EmailAddress = null
                    }
                };
            }
            return _authors;
        }

        private static IList<Book> GetBooks()
        {
            if (_books == null)
            {
                _books = new List<Book>()
                {
                    new Book()
                    {
                        Title = "IT ENDS WITH US",
                        Type = "Fiction",
                        PublisherId = 1,
                        Price = 200000,
                        Advance = 50000,
                        Royalty = 0,
                        YtdSales = 18000000,
                        Notes = null,
                        PublishedDate = DateTime.Now.Date
                    },
                    new Book()
                    {
                        Title = "ATOMIC HABITS",
                        Type = "Miscellaneous",
                        PublisherId = 2,
                        Price = 225000,
                        Advance = 120000,
                        Royalty = 80000,
                        YtdSales = 25760000,
                        Notes = null,
                        PublishedDate = DateTime.Now.Date
                    },
                    new Book()
                    {
                        Title = "Wonder",
                        Type = "Children",
                        PublisherId = 1,
                        Price = 70000,
                        Advance = 300000,
                        Royalty = 20000,
                        YtdSales = 11521000,
                        Notes = null,
                        PublishedDate = DateTime.Now.Date
                    },
                    new Book()
                    {
                        Title = "HAPPY-GO-LUCKY",
                        Type = "Nonfiction",
                        PublisherId = 1,
                        Price = 360000,
                        Advance = 5000000,
                        Royalty = 180000,
                        YtdSales = 36458000,
                        Notes = null,
                        PublishedDate = DateTime.Now.Date
                    },
                    new Book()
                    {
                        Title = "KILLING THE KILLERS",
                        Type = "Nonfiction",
                        PublisherId = 2,
                        Price = 360000,
                        Advance = 5000000,
                        Royalty = 180000,
                        YtdSales = 36458000,
                        Notes = null,
                        PublishedDate = DateTime.Now.Date
                    },
                };
            }
            return _books;
        }

        public static void MigrateData(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new NullReferenceException();
            }

            if (dbContext.Publishers.Count() == 0)
            {
                foreach (var publisher in GetPublishers())
                {
                    dbContext.Publishers.Add(publisher);
                }
                dbContext.SaveChanges();
            }

            if (dbContext.Roles.Count() == 0)
            {
                foreach (var role in GetRoles())
                {
                    dbContext.Roles.Add(role);
                }
                dbContext.SaveChanges();
            }

            if (dbContext.Users.Count() == 0)
            {
                foreach (var user in GetUsers())
                {
                    dbContext.Users.Add(user);
                }
                dbContext.SaveChanges();
            }

            if (dbContext.Authors.Count() == 0)
            {
                foreach (var author in GetAuthors())
                {
                    dbContext.Authors.Add(author);
                }
                dbContext.SaveChanges();
            }

            if (dbContext.Books.Count() == 0)
            {
                foreach (var book in GetBooks())
                {
                    dbContext.Books.Add(book);
                }
                dbContext.SaveChanges();
            }
        }

    }
}
