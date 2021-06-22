using Entitis.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbServices.Model
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) 
            : base(options)
        {

        }

        public DbSet<College> Colleges { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }
        public DbSet<PublishingHouse> publishingHouses { get; set; }
        public DbSet<BorrowTheBook> BorrowTheBooks { get; set; }
        public DbSet<FeedBack> FeedBacks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<BookAuthor>().HasKey(b => new { b.Book_id, b.User_id});

            builder.Entity<BookAuthor>().
                HasOne<User>(u => u.User).
                WithMany(u => u.Book_authors).
                HasForeignKey(u => u.User_id);

            builder.Entity<BookAuthor>().
                HasOne<Book>(b => b._Book).
                WithMany(b => b.Book_authors).
                HasForeignKey(b => b.Book_id);

        }
    }
}
