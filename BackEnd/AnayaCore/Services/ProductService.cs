using AnayaCore.Models.Main;
using AnayaCore.Repositories;

namespace AnayaCore.Services
{
    /// <summary>
    /// Product Service Implementation
    /// Handles business logic for product operations
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IProductRepository productRepository, ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        /// <summary>
        /// Get all products
        /// </summary>
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving all products");
                return await _productRepository.GetAllProductsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving products: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Get product by ID
        /// </summary>
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Retrieving product with ID: {id}");
                return await _productRepository.GetProductByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving product {id}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Get products by category
        /// </summary>
        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            try
            {
                _logger.LogInformation($"Retrieving products for category: {categoryId}");
                return await _productRepository.GetProductsByCategoryAsync(categoryId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving products for category {categoryId}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Search products by term
        /// </summary>
        public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
        {
            try
            {
                _logger.LogInformation($"Searching products with term: {searchTerm}");
                return await _productRepository.SearchProductsAsync(searchTerm);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error searching products: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Create a new product
        /// </summary>
        public async Task<Product> CreateProductAsync(Product product)
        {
            try
            {
                _logger.LogInformation($"Creating product: {product.Name}");
                
                // Validate product
                if (string.IsNullOrWhiteSpace(product.Name))
                    throw new ArgumentException("Product name is required");
                
                if (product.Price < 0)
                    throw new ArgumentException("Product price cannot be negative");

                return await _productRepository.CreateProductAsync(product);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating product: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Update an existing product
        /// </summary>
        public async Task<Product?> UpdateProductAsync(int id, Product product)
        {
            try
            {
                _logger.LogInformation($"Updating product with ID: {id}");
                
                // Validate product
                if (string.IsNullOrWhiteSpace(product.Name))
                    throw new ArgumentException("Product name is required");
                
                if (product.Price < 0)
                    throw new ArgumentException("Product price cannot be negative");

                return await _productRepository.UpdateProductAsync(id, product);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating product {id}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Delete a product
        /// </summary>
        public async Task<bool> DeleteProductAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting product with ID: {id}");
                return await _productRepository.DeleteProductAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting product {id}: {ex.Message}");
                throw;
            }
        }
    }
}
