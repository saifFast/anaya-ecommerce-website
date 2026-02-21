using Microsoft.AspNetCore.Mvc;
using AnayaCore.Models;

namespace AnayaCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        // In-memory database (replace with actual database context later)
        private static List<Category> categories = new List<Category>
        {
            new Category { Id = 1, Name = "Electronics", Description = "Laptops, tablets, smart TVs and other electronic devices", CreatedOn = DateTime.UtcNow.AddDays(-45) },
            new Category { Id = 2, Name = "Accessories", Description = "Mice, keyboards, hubs, and computer peripherals", CreatedOn = DateTime.UtcNow.AddDays(-40) },
            new Category { Id = 3, Name = "Audio", Description = "Headphones, earbuds, speakers and audio equipment", CreatedOn = DateTime.UtcNow.AddDays(-35) },
            new Category { Id = 4, Name = "Monitors & Displays", Description = "Computer monitors and display panels", CreatedOn = DateTime.UtcNow.AddDays(-30) },
            new Category { Id = 5, Name = "Furniture & Workspace", Description = "Desks, chairs, and ergonomic office furniture", CreatedOn = DateTime.UtcNow.AddDays(-25) },
            new Category { Id = 6, Name = "Lighting & Ambiance", Description = "Smart lights, LED strips, and ambient lighting solutions", CreatedOn = DateTime.UtcNow.AddDays(-20) },
            new Category { Id = 7, Name = "Cables & Connectivity", Description = "Cables, adapters, and network connectivity products", CreatedOn = DateTime.UtcNow.AddDays(-15) }
        };

        /// <summary>
        /// Get all categories
        /// </summary>
        /// <returns>List of all categories</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Category>> GetAllCategories()
        {
            return Ok(categories);
        }

        /// <summary>
        /// Get category by ID
        /// </summary>
        /// <param name="id">Category ID</param>
        /// <returns>Category details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Category> GetCategoryById(int id)
        {
            var category = categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound(new { message = $"Category with ID {id} not found" });
            }
            return Ok(category);
        }

        /// <summary>
        /// Create a new category
        /// </summary>
        /// <param name="category">Category data</param>
        /// <returns>Created category</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Category> CreateCategory([FromBody] Category category)
        {
            if (category == null)
            {
                return BadRequest(new { message = "Category data is required" });
            }

            if (string.IsNullOrWhiteSpace(category.Name))
            {
                return BadRequest(new { message = "Category name is required" });
            }

            // Generate new ID
            category.Id = categories.Count > 0 ? categories.Max(c => c.Id) + 1 : 1;
            category.CreatedOn = DateTime.UtcNow;
            categories.Add(category);

            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
        }

        /// <summary>
        /// Update an existing category
        /// </summary>
        /// <param name="id">Category ID</param>
        /// <param name="category">Updated category data</param>
        /// <returns>Updated category</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Category> UpdateCategory(int id, [FromBody] Category category)
        {
            if (category == null)
            {
                return BadRequest(new { message = "Category data is required" });
            }

            var existingCategory = categories.FirstOrDefault(c => c.Id == id);
            if (existingCategory == null)
            {
                return NotFound(new { message = $"Category with ID {id} not found" });
            }

            if (string.IsNullOrWhiteSpace(category.Name))
            {
                return BadRequest(new { message = "Category name is required" });
            }

            // Update properties
            existingCategory.Name = category.Name;
            existingCategory.Description = category.Description;

            return Ok(existingCategory);
        }

        /// <summary>
        /// Delete a category by ID
        /// </summary>
        /// <param name="id">Category ID</param>
        /// <returns>Success message</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteCategory(int id)
        {
            var category = categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound(new { message = $"Category with ID {id} not found" });
            }

            categories.Remove(category);
            return Ok(new { message = $"Category with ID {id} has been deleted successfully" });
        }

        /// <summary>
        /// Search categories by name
        /// </summary>
        /// <param name="searchTerm">Search term</param>
        /// <returns>List of categories matching search criteria</returns>
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Category>> SearchCategories([FromQuery] string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return Ok(categories);
            }

            var results = categories.Where(c => 
                c.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                c.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
            ).ToList();

            return Ok(results);
        }

        /// <summary>
        /// Get categories sorted by creation date
        /// </summary>
        /// <param name="ascending">Sort in ascending order (default: false)</param>
        /// <returns>List of categories sorted by creation date</returns>
        [HttpGet("sorted")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Category>> GetCategoriesSorted([FromQuery] bool ascending = false)
        {
            var sorted = ascending 
                ? categories.OrderBy(c => c.CreatedOn).ToList()
                : categories.OrderByDescending(c => c.CreatedOn).ToList();

            return Ok(sorted);
        }
    }
}
