using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BooksWebAPI.Data;
using BooksWebAPI.Data.DbModels;
using BooksWebAPI.Models;
using BooksWebAPI.Models.Author;
using Microsoft.EntityFrameworkCore;

namespace BooksWebAPI.Services.Implementation
{
    public class AuthorService : IAuthorService
    {
        private readonly BooksDbContext dbContext;

        public AuthorService(BooksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<AuthorResultModel> CreateAuthor(string name, string imageUrl, string website, string description, IEnumerable<GenericComboBox> books)
        {
            AuthorResultModel result = new AuthorResultModel();

            if (await this.dbContext.Authors.AnyAsync(p => p.Name.ToLower().Trim() == name.ToLower().Trim()))
            {
                result.Success = false;
                result.Errors = new[] { "Author with this name already exist." };
                return result;
            }

            Author author = new Author
            {
                Id = Guid.NewGuid(),
                Name = name.Trim(),
                Description = description.Trim(),
                ImageUrl = imageUrl.Trim(),
                Website = website.Trim(),
            };

            await this.dbContext.Authors.AddAsync(author);

            if (books != null && books.Any())
            {
                IEnumerable<Guid> bookIds = books.Where(x => x != null && x.Id != default).Select(x => x.Id);

                IEnumerable<Book> booksForUpdate = await this.dbContext.Books.Where(x => bookIds.Contains(x.Id)).ToListAsync();

                this.dbContext.Books.AttachRange(booksForUpdate);
                foreach (Book book in booksForUpdate)
                {
                    book.AuthorId = author.Id;
                }
            }

            await this.dbContext.SaveChangesAsync();

            result.Name = author.Name;
            result.Success = true;

            return result;
        }

        public async Task<AuthorResultModel> UpdateAuthor(Guid id, string name, string imageUrl, string website, string description, IEnumerable<GenericComboBox> books)
        {
            AuthorResultModel result = new AuthorResultModel();

            Author author = await this.dbContext.Authors.Include(x => x.Books).FirstOrDefaultAsync(p => p.Id == id);

            if (author == null)
            {
                result.Success = false;
                result.Errors = new[] { "Author with this Id not exist." };
                return result;
            }

            if (author.Name.ToLower().Trim() != name.ToLower().Trim() && await this.dbContext.Categories.AnyAsync(p => p.Name.ToLower().Trim() == name.ToLower().Trim() && p.Id != id))
            {
                result.Success = false;
                result.Errors = new[] { "Author with this name already exist." };
                return result;
            }

            this.dbContext.Authors.Attach(author);

            author.Name = name.Trim();
            author.Description = description.Trim();
            author.ImageUrl = imageUrl.Trim();
            author.Website = website.Trim();

            await this.UpdateBooksAsync(books, author);

            await this.dbContext.SaveChangesAsync();

            result.Name = author.Name;
            result.Success = true;

            return result;
        }

        public async Task<AuthorResultModel> DeleteAuthor(Guid id)
        {
            AuthorResultModel result = new AuthorResultModel();

            Author author = await this.dbContext.Authors.FirstOrDefaultAsync(p => p.Id == id);

            if (author == null)
            {
                result.Success = false;
                result.Errors = new[] { "Author with this Id not exist." };
                return result;
            }

            this.dbContext.Authors.Remove(author);

            await this.dbContext.SaveChangesAsync();

            result.Name = author.Name;
            result.Success = true;

            return result;
        }

        public async Task<AuthorEditModel> GetAuthor(Guid id)
            => await this.dbContext
                      .Authors
                      .Where(x => x.Id == id)
                      .Select(x => new AuthorEditModel()
                      {
                          Id = x.Id,
                          Name = x.Name,
                          Description = x.Description,
                          ImageUrl = x.ImageUrl,
                          Website = x.Website,
                          Books = x.Books.Select(y => new GenericComboBox() 
                          {
                              Id = y.Id,
                              Name = y.Name
                          })
                      })
                      .FirstOrDefaultAsync();

        public async Task<IEnumerable<AuthorEditModel>> GetAuthors()
            => await this.dbContext
                      .Authors
                      .Select(x => new AuthorEditModel()
                      {
                          Id = x.Id,
                          Name = x.Name,
                          Description = x.Description,
                          ImageUrl = x.ImageUrl,
                          Website = x.Website,
                          Books = x.Books.Select(y => new GenericComboBox()
                          {
                              Id = y.Id,
                              Name = y.Name
                          })
                      })
                      .ToListAsync();

        public async Task<IEnumerable<GenericComboBox>> GetAuthorsInfo()
            => await this.dbContext
                         .Authors
                         .Select(x => new GenericComboBox()
                         {
                             Id = x.Id,
                             Name = x.Name,
                         })
                         .ToListAsync();

        private async Task UpdateBooksAsync(IEnumerable<GenericComboBox> books, Author author)
        {
            IEnumerable<Book> booksInDb = author.Books;
            IEnumerable<GenericComboBox> booksForUpdate = books.Where(x => x != null && x.Id != default);

            { // Insert
                if (booksForUpdate != null && booksForUpdate.Any())
                {
                    IEnumerable<Guid> booksForInsert = booksForUpdate.Where(x => booksInDb != null
                                                                                && booksInDb.Any()
                                                                                && !booksInDb.Select(y => y.Id).Contains(x.Id))
                                                                     .Select(x => x.Id);

                    IEnumerable<Book> dbObjsForUpdate = await this.dbContext.Books.Where(x => booksForInsert.Contains(x.Id)).ToListAsync();

                    this.dbContext.Books.AttachRange(dbObjsForUpdate);
                    foreach (Book book in dbObjsForUpdate)
                    {
                        book.AuthorId = author.Id;
                    }
                }
            }

            { // Delete
                if (booksInDb != null && booksInDb.Any())
                {
                    IEnumerable<Guid> booksForDelete = booksInDb.Where(x => booksForUpdate != null
                                                                           && booksForUpdate.Any()
                                                                           && !booksForUpdate.Select(y => y.Id).Contains(x.Id))
                                                                .Select(x => x.Id);

                    IEnumerable<Book> dbObjsForUpdate = booksInDb.Where(x => booksForDelete.Contains(x.Id));

                    this.dbContext.Books.AttachRange(dbObjsForUpdate);
                    foreach (Book book in dbObjsForUpdate)
                    {
                        this.dbContext.Books.Attach(book);
                        book.AuthorId = null;
                    }
                }
            }
        }
    }
}
