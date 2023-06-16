using BusinessObject;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<BookAuthor>(entity =>
            {
                entity.HasKey(e => new { e.AuthorId, e.BookId });
                entity.HasOne(e => e.Author).WithMany(e => e.BookAuthors).HasForeignKey(e => e.AuthorId);
                entity.HasOne(e => e.Book).WithMany(e => e.BookAuthors).HasForeignKey(e => e.BookId);
            });



            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Publisher).WithMany(e => e.Books).HasForeignKey(e => e.PublisherId);
            });



            modelBuilder.Entity<Author>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Address).HasMaxLength(1000);
            });



            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id);
            });



            modelBuilder.Entity<Publisher>(entity =>
            {
                entity.HasKey(e => e.Id);
            });



            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Publisher).WithMany(e => e.Users).HasForeignKey(e => e.PublisherId);
                entity.HasOne(e => e.Role).WithMany(e => e.Users).HasForeignKey(e => e.RoleId);
            });
        }
    }
}
