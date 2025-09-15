using project.Models;

namespace project.Repository.IRepository
{
    public interface ICategoryRepository
    {
        Task CreateCategoryAsync(Category category);
        Task DeleteCategoryAsync(int id);
        Task<Category?> GetByIdAsync(int id);
        Task<IEnumerable<Category>> GetCategoriesAsync();
        Task UpdateCategoryAsync(Category category);
        Task<Category> GetCategoryByNameAsync(string categoryName);
        Task<Category> GetByNameAsync(string categoryName);

    }
}
