using project.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace project.Services.IServices
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetCategoriesAsync();
        Task<Category?> GetByIdAsync(int id);
        Task CreateCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(int id);
        Task<Category> GetCategoryByNameAsync(string categoryName);
    }
}
