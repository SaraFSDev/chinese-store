using project.Models;
using project.Repository.IRepository;
using project.Services.IServices;
using Microsoft.Extensions.Logging;

namespace project.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(ICategoryRepository categoryRepository, ILogger<CategoryService> logger)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
        }

        public async Task CreateCategoryAsync(Category category)
        {
            if (category == null)
            {
                _logger.LogWarning("Received null category object.");
                throw new ArgumentException("Category cannot be null.");
            }

            try
            {
                _logger.LogInformation($"Processing creation of category with name: {category.name}");

                var existingCategory = await _categoryRepository.GetByNameAsync(category.name);
                if (existingCategory != null)
                {
                    _logger.LogWarning($"Category with name {category.name} already exists.");
                    throw new InvalidOperationException($"Category with name {category.name} already exists.");
                }

                await _categoryRepository.CreateCategoryAsync(category);

                _logger.LogInformation($"Category with name {category.name} created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while creating category {category.name}: {ex.Message}");
                throw new InvalidOperationException("Error occurred while creating category.", ex);
            }
        }

        public async Task DeleteCategoryAsync(int id)
        {
            try
            {
                var category = await _categoryRepository.GetByIdAsync(id);
                if (category == null)
                {
                    _logger.LogWarning("Category not found for deletion with ID: {Id}", id);
                    throw new ArgumentException("Category not found", nameof(id));
                }

                await _categoryRepository.DeleteCategoryAsync(id);
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
                return await _categoryRepository.GetByIdAsync(id);
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
                return await _categoryRepository.GetCategoriesAsync();
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
                await _categoryRepository.UpdateCategoryAsync(category);
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
            if (string.IsNullOrEmpty(categoryName))
            {
                _logger.LogWarning("Invalid category name: {CategoryName}", categoryName);
                throw new ArgumentException("Category name cannot be empty or null", nameof(categoryName));
            }

            try
            {
                var category = await _categoryRepository.GetCategoryByNameAsync(categoryName);
                return category;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving category by name: {CategoryName}", categoryName);
                throw;
            }
        }

    }
}
