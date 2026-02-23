using Microsoft.AspNetCore.Mvc;
using AnayaCore.Services;
using AnayaCore.Models.Main;

namespace AnayaCore.Controllers
{
    /// <summary>
    /// Category Controller (Refactored)
    /// Handles HTTP requests related to categories
    /// Delegates business logic to CategoryService
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryControllerRefactored : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryControllerRefactored> _logger;

        public CategoryControllerRefactored(ICategoryService categoryService, ILogger<CategoryControllerRefactored> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        /// <summary>
        /// Get all categories
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetAllCategories()
        {
            try
            {
                var categories = await _categoryService.GetAllCategoriesAsync();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetAllCategories: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get category by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategoryById(int id)
        {
            try
            {
                var category = await _categoryService.GetCategoryByIdAsync(id);
                if (category == null)
                    return NotFound($"Category with ID {id} not found");

                return Ok(category);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetCategoryById: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Create a new category
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory([FromBody] Category category)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var createdCategory = await _categoryService.CreateCategoryAsync(category);
                return CreatedAtAction(nameof(GetCategoryById), new { id = createdCategory.Id }, createdCategory);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in CreateCategory: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Update an existing category
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<Category>> UpdateCategory(int id, [FromBody] Category category)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var updatedCategory = await _categoryService.UpdateCategoryAsync(id, category);
                if (updatedCategory == null)
                    return NotFound($"Category with ID {id} not found");

                return Ok(updatedCategory);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in UpdateCategory: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Delete a category
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            try
            {
                var success = await _categoryService.DeleteCategoryAsync(id);
                if (!success)
                    return NotFound($"Category with ID {id} not found");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in DeleteCategory: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
