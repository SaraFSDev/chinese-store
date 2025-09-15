using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project.Models;
using project.Models; // וודא שקטגוריית המודל היבטתו כאן

using project.Services;
using project.Services.IServices;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoriesAsync()
        {
            try
            {
                var categories = await _categoryService.GetCategoriesAsync();
                _logger.LogInformation("Successfully retrieved categories.");
                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve categories.");
                return StatusCode(500, "Internal server error.");
            }
            
        }
        [Authorize(Roles = "manager")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid category ID: {Id}", id);
                return BadRequest("Invalid category ID.");
            }

            try
            {
                var category = await _categoryService.GetByIdAsync(id);
                if (category == null)
                {
                    _logger.LogWarning("Category not found for ID: {Id}", id);
                    return NotFound("Category not found.");
                }

                return Ok(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving category with ID: {Id}", id);
                return StatusCode(500, "Internal server error.");
            }
        }
        [Authorize(Roles = "manager")]
        [HttpPost]
        public async Task<IActionResult> CreateCategoryAsync(Category category)
        {
            if (category == null)
            {
                _logger.LogWarning("Invalid category data (null).");
                return BadRequest("Invalid category data.");
            }

            try
            {
                await _categoryService.CreateCategoryAsync(category);
                _logger.LogInformation("Category created successfully: {category.name}", category.name);
                return CreatedAtAction(nameof(GetByIdAsync), new { id = category.Id }, category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create category.");
                return StatusCode(500, "Internal server error.");
            }
        }
        [Authorize(Roles = "manager")]
        [HttpPut]
        public async Task<IActionResult> UpdateCategoryAsync(Category category)
        {
            if (category == null)
            {
                _logger.LogWarning("Invalid category data (null) for update.");
                return BadRequest("Invalid category data.");
            }

            try
            {
                await _categoryService.UpdateCategoryAsync(category);
                _logger.LogInformation("Category updated successfully: {category.name}", category.name);
                return Ok(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update category.");
                return StatusCode(500, "Internal server error.");
            }
        }
        [Authorize(Roles = "manager")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoryAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid category ID: {Id} for deletion.", id);
                return BadRequest("Invalid category ID.");
            }

            try
            {
                await _categoryService.DeleteCategoryAsync(id);
                _logger.LogInformation("Category deleted successfully: {Id}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete category with ID: {Id}", id);
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("by-name/{categoryName}")]
        public async Task<IActionResult> GetCategoryByNameAsync(string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName))
            {
                _logger.LogWarning("Invalid category name: {CategoryName}", categoryName);
                return BadRequest("Category name cannot be empty or null.");
            }

            try
            {
                var category = await _categoryService.GetCategoryByNameAsync(categoryName);
                return Ok(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve category with name: {CategoryName}", categoryName);
                return StatusCode(500, "Internal server error.");
            }
        }

    }
}
