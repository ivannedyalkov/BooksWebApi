using BooksWebAPI.Models;
using BooksWebAPI.Models.Author;
using BooksWebAPI.Models.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksWebAPI.Services
{
    public interface IAuthorService 
    {
        Task<AuthorResultModel> CreateAuthor(string name, string imageUrl, string website, string description, IEnumerable<GenericComboBox> books);

        Task<AuthorResultModel> UpdateAuthor(Guid id, string name, string imageUrl, string website, string description, IEnumerable<GenericComboBox> books);

        Task<AuthorResultModel> DeleteAuthor(Guid id);

        Task<AuthorEditModel> GetAuthor(Guid id);

        Task<IEnumerable<AuthorEditModel>> GetAuthors();

        Task<IEnumerable<GenericComboBox>> GetAuthorsInfo();
    }
}
