using System;
using System.Collections.Generic;
using System.Text;
using BooksWebAPI.Data.DbModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BooksWebAPI.Data
{
    public class BooksDbContext : IdentityDbContext
    {
        public BooksDbContext(DbContextOptions<BooksDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<XRefBookCategory> XRefBookCategories { get; set; }
        public DbSet<XRefUserBook> XRefUserBooks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<XRefBookCategory>()
                .HasOne(bc => bc.Book)
                .WithMany(b => b.BookCategories)
                .HasForeignKey(bc => bc.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<XRefBookCategory>()
                .HasOne(bc => bc.Category)
                .WithMany(c => c.BookCategories)
                .HasForeignKey(bc => bc.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Author>()
                .HasMany(a => a.Books)
                .WithOne(a => a.Author)
                .HasForeignKey(a => a.AuthorId);

            builder.Entity<Book>()
                .HasOne(b => b.Author)
                .WithMany(b => b.Books)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<XRefUserBook>()
               .HasOne(bc => bc.Book)
               .WithMany(b => b.UserBooks)
               .HasForeignKey(bc => bc.BookId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<XRefUserBook>()
               .HasOne(bc => bc.User)
               .WithMany(b => b.UserBooks)
               .HasForeignKey(bc => bc.UserId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
