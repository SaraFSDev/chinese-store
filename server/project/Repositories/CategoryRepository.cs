using project.Models;
using project.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace project.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ProjectDbContext _dBContext;
        private readonly ILogger<CategoryRepository> _logger;

        public CategoryRepository(ProjectDbContext dBContext, ILogger<CategoryRepository> logger)
        {
            _dBContext = dBContext;
            _logger = logger;
        }

        public async Task CreateCategoryAsync(Category category)
        {
            if (category == null)
            {
                _logger.LogWarning("Received null category object.");
                throw new ArgumentNullException(nameof(category), "Category cannot be null.");
            }

            try
            {
                _logger.LogInformation($"Adding category with name: {category.name}");

                await _dBContext.Categories.AddAsync(category);
                await _dBContext.SaveChangesAsync();

                _logger.LogInformation($"Category with name {category.name} created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while creating category with name {category.name}: {ex.Message}");
                throw new InvalidOperationException("Error occurred while creating category.", ex);
            }
        }

        public async Task<Category?> GetByNameAsync(string name)
        {
            try
            {
                _logger.LogInformation($"Searching for category with name: {name}");
                var category = await _dBContext.Categories.FirstOrDefaultAsync(c => c.name == name);

                if (category == null)
                {
                    _logger.LogWarning($"No category found with name: {name}");
                }

                return category;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while fetching category by name {name}: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteCategoryAsync(int id)
        {
            try
            {
                Category? category = await GetByIdAsync(id);
                if (category == null)
                {
                    _logger.LogWarning("Category not found for deletion with ID: {Id}", id);
                    throw new ArgumentException("Category not found", nameof(id));
                }

                _dBContext.Categories.Remove(category);
                await _dBContext.SaveChangesAsync();
                _logger.LogInformation("Category deleted with ID: {Id}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting category with ID: {Id}", id);
                throw;
            }
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            try
            {
                return await _dBContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving category with ID: {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            try
            {
                return await _dBContext.Categories.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving categories.");
                throw;
            }
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            try
            {
                _dBContext.Categories.Update(category);
                await _dBContext.SaveChangesAsync();
                _logger.LogInformation("Category updated: {name}", category.name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating category: {name}", category.name);
                throw;
            }
        }
        public async Task<Category> GetCategoryByNameAsync(string categoryName)
        {
            try
            {
                var category = await _dBContext.Categories
                    .FirstOrDefaultAsync(c => c.name == categoryName);

                if (category == null)
                {
                    _logger.LogWarning("Category not found for name: {name}", categoryName);
                    throw new ArgumentException("Category not found", nameof(categoryName));
                }

                _logger.LogInformation("Successfully retrieved category with name: {name}", categoryName);
                return category;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving category by name: {name}", categoryName);
                throw;
            }
        }

    }
}
