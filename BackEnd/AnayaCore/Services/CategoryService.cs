using AnayaCore.Models.Main;
using AnayaCore.Repositories;

namespace AnayaCore.Services
{
    /// <summary>
    /// Category Service Implementation
    /// Handles business logic for category operations
    /// </summary>
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(ICategoryRepository categoryRepository, ILogger<CategoryService> logger)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
        }

        /// <summary>
        /// Get all categories
        /// </summary>
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving all categories");
                return await _categoryRepository.GetAllCategoriesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving categories: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Get category by ID
        /// </summary>
        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Retrieving category with ID: {id}");
                return await _categoryRepository.GetCategoryByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving category {id}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Create a new category
        /// </summary>
        public async Task<Category> CreateCategoryAsync(Category category)
        {
            try
            {
                _logger.LogInformation($"Creating category: {category.Name}");
                
                if (string.IsNullOrWhiteSpace(category.Name))
                    throw new ArgumentException("Category name is required");

                return await _categoryRepository.CreateCategoryAsync(category);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating category: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Update an existing category
        /// </summary>
        public async Task<Category?> UpdateCategoryAsync(int id, Category category)
        {
            try
            {
                _logger.LogInformation($"Updating category with ID: {id}");
                
                if (string.IsNullOrWhiteSpace(category.Name))
                    throw new ArgumentException("Category name is required");

                return await _categoryRepository.UpdateCategoryAsync(id, category);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating category {id}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Delete a category
        /// </summary>
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting category with ID: {id}");
                return await _categoryRepository.DeleteCategoryAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting category {id}: {ex.Message}");
                throw;
            }
        }
    }
}
