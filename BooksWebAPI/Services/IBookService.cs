using BooksWebAPI.Models;
using BooksWebAPI.Models.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksWebAPI.Services
{
    public interface IBookService
    {
        Task<BookResultModel> CreateBook(string name, string description, string imageUrl, GenericComboBox author, DateTime? releaseDate, IEnumerable<GenericComboBox> categories);

        Task<BookResultModel> UpdateBook(Guid id, string name, string description, string imageUrl, GenericComboBox author, DateTime? releaseDate, IEnumerable<GenericComboBox> categories);

        Task<BookResultModel> DeleteBook(Guid id);

        Task<BookEditModel> GetBook(Guid id);

        Task<IEnumerable<BookEditModel>> GetBooks();

        Task<IEnumerable<GenericComboBox>> GetBooksInfo();
    }
}
