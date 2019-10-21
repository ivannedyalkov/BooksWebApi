using BooksWebAPI.Models;
using BooksWebAPI.Models.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksWebAPI.Services
{
    public interface ICategoryService
    {
        Task<CategoryResultModel> CreateCategory(string name, string description);

        Task<CategoryResultModel> UpdateCategory(Guid id, string name, string description);

        Task<CategoryResultModel> DeleteCategory(Guid id);

        Task<CategoryEditModel> GetCategory(Guid id);

        Task<IEnumerable<CategoryEditModel>> GetCategories();

        Task<IEnumerable<GenericComboBox>> GetCategoriesInfo();


    }
}
