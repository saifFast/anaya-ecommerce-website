using AnayaCore.Models.Main;

namespace AnayaCore.Repositories
{
    /// <summary>
    /// In-memory implementation of ICategoryRepository
    /// </summary>
    public class CategoryRepository : ICategoryRepository
    {
        private static List<Category> _categories = new List<Category>
        {
            new Category { Id = 1, Name = "Electronics", Description = "Computers, laptops, tablets and smart devices" },
            new Category { Id = 2, Name = "Accessories", Description = "Mice, keyboards, cables and other peripherals" },
            new Category { Id = 3, Name = "Audio", Description = "Headphones, speakers, and audio equipment" },
            new Category { Id = 4, Name = "Monitors & Displays", Description = "Computer monitors and display devices" },
            new Category { Id = 5, Name = "Furniture & Workspace", Description = "Desks, chairs, and workspace solutions" },
            new Category { Id = 6, Name = "Lighting & Ambiance", Description = "Smart lights and ambient lighting solutions" }
        };

        private int _nextId = 7;

        /// <summary>
        /// Get all categories
        /// </summary>
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await Task.FromResult(_categories.AsReadOnly());
        }

        /// <summary>
        /// Get category by ID
        /// </summary>
        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await Task.FromResult(_categories.FirstOrDefault(c => c.Id == id));
        }

        /// <summary>
        /// Create a new category
        /// </summary>
        public async Task<Category> CreateCategoryAsync(Category category)
        {
            category.Id = _nextId++;
            _categories.Add(category);
            return await Task.FromResult(category);
        }

        /// <summary>
        /// Update an existing category
        /// </summary>
        public async Task<Category?> UpdateCategoryAsync(int id, Category category)
        {
            var existingCategory = _categories.FirstOrDefault(c => c.Id == id);
            if (existingCategory == null)
                return null;

            existingCategory.Name = category.Name;
            existingCategory.Description = category.Description;

            return await Task.FromResult(existingCategory);
        }

        /// <summary>
        /// Delete a category by ID
        /// </summary>
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = _categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
                return false;

            _categories.Remove(category);
            return await Task.FromResult(true);
        }
    }
}
