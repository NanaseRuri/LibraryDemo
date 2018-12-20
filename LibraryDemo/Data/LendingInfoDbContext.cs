using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryDemo.Models.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace LibraryDemo.Data
{
    public class LendingInfoDbContext:DbContext
    {
        public LendingInfoDbContext(DbContextOptions<LendingInfoDbContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<BookDetails> BooksDetail { get; set; }
        public DbSet<Bookshelf> Bookshelves { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<RecommendedBook> RecommendedBooks { get; set; }
        public DbSet<BookMiddle> BookMiddles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookMiddle>().HasKey(t => new {t.BookId, t.BookshelfId});
        }
    }
}
