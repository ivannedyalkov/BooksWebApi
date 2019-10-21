using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BooksWebAPI.Data;
using BooksWebAPI.Data.DbModels;
using BooksWebAPI.Models;
using BooksWebAPI.Models.Book;
using Microsoft.EntityFrameworkCore;

namespace BooksWebAPI.Services.Implementation
{
    public class BookService : IBookService
    {
        private readonly BooksDbContext dbContext;

        public BookService(BooksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<BookResultModel> CreateBook(string name, string description, string imageUrl, GenericComboBox author, DateTime? releaseDate, IEnumerable<GenericComboBox> categories)
        {
            BookResultModel result = new BookResultModel();

            if (await this.dbContext.Books.AnyAsync(p => p.Name.ToLower().Trim() == name.ToLower().Trim()))
            {
                result.Success = false;
                result.Errors = new[] { "Book with this name already exist." };
                return result;
            }

            Book book = new Book
            {
                Id = Guid.NewGuid(),
                Name = name.Trim(),
                Description = description.Trim(),
                ImageUrl = imageUrl.Trim(),
                ReleaseDate = releaseDate,
            };

            if (author != null && author.Id != default)
            {
                book.AuthorId = author.Id;
            }

            await this.dbContext.Books.AddAsync(book);

            if (categories != null && categories.Any())
            {
                IEnumerable<Guid> categoryIds = categories.Where(x => x != null && x.Id != default).Select(x => x.Id);

                IEnumerable<Guid> categoriesInDB = await this.dbContext.Categories.Where(x => categoryIds.Contains(x.Id)).Select(x => x.Id).ToListAsync();
                foreach (Guid categoryId in categoriesInDB)
                {
                    XRefBookCategory xRefForInsert = new XRefBookCategory()
                    {
                        Id = Guid.NewGuid(),
                        BookId = book.Id,
                        CategoryId = categoryId
                    };

                    this.dbContext.XRefBookCategories.Add(xRefForInsert);
                }
            }

            await this.dbContext.SaveChangesAsync();

            result.Name = book.Name;
            result.Success = true;

            return result;
        }

        public async Task<BookResultModel> UpdateBook(Guid id, string name, string description, string imageUrl, GenericComboBox author, DateTime? releaseDate, IEnumerable<GenericComboBox> categories)
        {
            BookResultModel result = new BookResultModel();

            Book book = await this.dbContext.Books.Include(x => x.BookCategories).FirstOrDefaultAsync(p => p.Id == id);

            if (book == null)
            {
                result.Success = false;
                result.Errors = new[] { "Book with this Id not exist." };
                return result;
            }

            if (book.Name.ToLower().Trim() != name.ToLower().Trim() && await this.dbContext.Categories.AnyAsync(p => p.Name.ToLower().Trim() == name.ToLower().Trim() && p.Id != id))
            {
                result.Success = false;
                result.Errors = new[] { "Book with this name already exist." };
                return result;
            }

            this.dbContext.Books.Attach(book);

            book.Name = name.Trim();
            book.Description = description.Trim();
            book.ImageUrl = imageUrl.Trim();
            book.ReleaseDate = releaseDate;

            if (author != null && author.Id != default)
            {
                book.AuthorId = author.Id;
            }
            else
            {
                book.AuthorId = null;
            }

            await this.UpdateBookCategoriesAsync(categories, book);

            await this.dbContext.SaveChangesAsync();

            result.Name = book.Name;
            result.Success = true;

            return result;
        }

        public async Task<BookResultModel> DeleteBook(Guid id)
        {
            BookResultModel result = new BookResultModel();

            Book book = await this.dbContext.Books.FirstOrDefaultAsync(p => p.Id == id);

            if (book == null)
            {
                result.Success = false;
                result.Errors = new[] { "Book with this Id not exist." };
                return result;
            }

            this.dbContext.Books.Remove(book);

            await this.dbContext.SaveChangesAsync();

            result.Name = book.Name;
            result.Success = true;

            return result;
        }

        public async Task<BookEditModel> GetBook(Guid id)
            => await this.dbContext
                         .Books
                         .Where(x => x.Id == id)
                         .Select(x => new BookEditModel()
                         {
                             Id = x.Id,
                             Name = x.Name,
                             Description = x.Description,
                             ImageUrl = x.ImageUrl,
                             ReleaseDate = x.ReleaseDate,
                             Author = x.AuthorId.HasValue ? new GenericComboBox() { Id = x.AuthorId.Value, Name = x.Author.Name } : null,
                             Categories = x.BookCategories.Select(y => new GenericComboBox()
                             {
                                 Id = y.CategoryId,
                                 Name = y.Category.Name
                             }),
                         })
                         .FirstOrDefaultAsync();

        public async Task<IEnumerable<BookEditModel>> GetBooks()
           => await this.dbContext
                        .Books
                        .Select(x => new BookEditModel()
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Description = x.Description,
                            ImageUrl = x.ImageUrl,
                            ReleaseDate = x.ReleaseDate,
                            Author = x.AuthorId.HasValue ? new GenericComboBox() { Id = x.AuthorId.Value, Name = x.Author.Name } : null,
                            Categories = x.BookCategories.Select(y => new GenericComboBox()
                            {
                                Id = y.CategoryId,
                                Name = y.Category.Name
                            }),
                        })
                        .ToListAsync();

        public async Task<IEnumerable<GenericComboBox>> GetBooksInfo()
            => await this.dbContext
                         .Books
                         .Select(x => new GenericComboBox()
                         {
                             Id = x.Id,
                             Name = x.Name,
                         })
                         .ToListAsync();

        private async Task UpdateBookCategoriesAsync(IEnumerable<GenericComboBox> categories, Book book)
        {
            IEnumerable<XRefBookCategory> bookCategoriesInDb = book.BookCategories;
            IEnumerable<GenericComboBox> bookCategoriesForUpdate = categories.Where(x => x != null && x.Id != default);

            { // Insert
                if (bookCategoriesForUpdate != null && bookCategoriesForUpdate.Any())
                {
                    IEnumerable<Guid> booksForInsert = bookCategoriesForUpdate.Where(x => bookCategoriesInDb != null
                                                                                        && bookCategoriesInDb.Any()
                                                                                        && !bookCategoriesInDb.Select(y => y.CategoryId).Contains(x.Id))
                                                                             .Select(x => x.Id);

                    IEnumerable<Guid> dbObjsForUpdate = await this.dbContext.Categories.Where(x => booksForInsert.Contains(x.Id)).Select(x => x.Id).ToListAsync();
                    foreach (Guid categoryId in dbObjsForUpdate)
                    {
                        XRefBookCategory xRefForInsert = new XRefBookCategory()
                        {
                            Id = Guid.NewGuid(),
                            BookId = book.Id,
                            CategoryId = categoryId
                        };

                        this.dbContext.XRefBookCategories.Add(xRefForInsert);
                    }
                }
            }

            { // Delete
                if (bookCategoriesInDb != null && bookCategoriesInDb.Any())
                {
                    IEnumerable<XRefBookCategory> booksForDelete = bookCategoriesInDb.Where(x => bookCategoriesForUpdate != null
                                                                                               && bookCategoriesForUpdate.Any()
                                                                                               && !bookCategoriesForUpdate.Select(y => y.Id).Contains(x.CategoryId));

                    this.dbContext.XRefBookCategories.RemoveRange(booksForDelete);
                }
            }
        }
    }
}
