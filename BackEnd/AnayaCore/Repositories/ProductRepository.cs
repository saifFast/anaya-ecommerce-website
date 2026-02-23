using AnayaCore.Models;

namespace AnayaCore.Repositories
{
    /// <summary>
    /// In-memory implementation of IProductRepository
    /// Stores products in memory for demonstration purposes
    /// In production, this would use a real database
    /// </summary>
    public class ProductRepository : IProductRepository
    {
        private static List<Product> _products = new List<Product>
        {
            // Category 1: Electronics
            new Product { Id = 1, Name = "MacBook Pro 14\"", Description = "Powerful laptop with M3 Pro chip, 16GB RAM, and stunning Retina display", CategoryId = 1, Price = 1999, ImageUrl = "https://picsum.photos/300/400?random=1" },
            new Product { Id = 2, Name = "Dell XPS 15", Description = "Premium ultrabook with Intel i7, RTX 4060, perfect for creators", CategoryId = 1, Price = 1799, ImageUrl = "https://picsum.photos/300/400?random=2" },
            new Product { Id = 3, Name = "ThinkPad X1 Carbon", Description = "Business laptop with legendary keyboard and all-day battery", CategoryId = 1, Price = 1500, ImageUrl = "https://picsum.photos/300/400?random=3" },
            new Product { Id = 4, Name = "iPad Pro 12.9\"", Description = "Versatile tablet with M2 chip and stunning OLED display", CategoryId = 1, Price = 1099, ImageUrl = "https://picsum.photos/300/400?random=4" },
            new Product { Id = 5, Name = "Samsung Smart TV 55\"", Description = "4K QLED TV with HDR and smart apps built-in", CategoryId = 1, Price = 1299, ImageUrl = "https://picsum.photos/300/400?random=5" },
            
            // Category 2: Accessories
            new Product { Id = 6, Name = "Logitech MX Master 3S", Description = "Premium wireless mouse with advanced tracking and customization", CategoryId = 2, Price = 99, ImageUrl = "https://picsum.photos/300/400?random=6" },
            new Product { Id = 7, Name = "Apple Magic Mouse", Description = "Multi-touch surface mouse with minimalist design", CategoryId = 2, Price = 79, ImageUrl = "https://picsum.photos/300/400?random=7" },
            new Product { Id = 8, Name = "Razer DeathAdder V3", Description = "Gaming mouse with Focus Pro sensor and wireless connection", CategoryId = 2, Price = 69, ImageUrl = "https://picsum.photos/300/400?random=8" },
            new Product { Id = 9, Name = "Keychron K8 Pro", Description = "Compact mechanical keyboard with backlit keys and wireless", CategoryId = 2, Price = 129, ImageUrl = "https://picsum.photos/300/400?random=9" },
            new Product { Id = 10, Name = "Cherry MX Board 8.0", Description = "Professional mechanical keyboard with premium build quality", CategoryId = 2, Price = 159, ImageUrl = "https://picsum.photos/300/400?random=10" },
            new Product { Id = 11, Name = "Corsair MM700 Extended", Description = "Large gaming mousepad with RGB lighting", CategoryId = 2, Price = 49, ImageUrl = "https://picsum.photos/300/400?random=11" },
            new Product { Id = 12, Name = "USB-C Hub 7-in-1", Description = "Multi-port hub with HDMI, USB 3.0, and SD card reader", CategoryId = 2, Price = 35, ImageUrl = "https://picsum.photos/300/400?random=12" },
            
            // Category 3: Audio
            new Product { Id = 13, Name = "Sony WH-1000XM5", Description = "Industry-leading noise-canceling headphones with 30-hour battery", CategoryId = 3, Price = 399, ImageUrl = "https://picsum.photos/300/400?random=13" },
            new Product { Id = 14, Name = "Apple AirPods Pro", Description = "Premium wireless earbuds with active noise cancellation", CategoryId = 3, Price = 249, ImageUrl = "https://picsum.photos/300/400?random=14" },
            new Product { Id = 15, Name = "Bose QuietComfort 45", Description = "Comfortable over-ear headphones with world-class noise cancellation", CategoryId = 3, Price = 379, ImageUrl = "https://picsum.photos/300/400?random=15" },
            new Product { Id = 16, Name = "Anker Soundcore Liberty 4", Description = "Budget-friendly earbuds with great sound quality", CategoryId = 3, Price = 79, ImageUrl = "https://picsum.photos/300/400?random=16" },
            new Product { Id = 17, Name = "JBL Flip 6", Description = "Portable waterproof speaker with 360° sound", CategoryId = 3, Price = 129, ImageUrl = "https://picsum.photos/300/400?random=17" },
            
            // Category 4: Monitors & Displays
            new Product { Id = 18, Name = "LG UltraWide 34\" curved", Description = "21:9 ultrawide monitor perfect for productivity and gaming", CategoryId = 4, Price = 599, ImageUrl = "https://picsum.photos/300/400?random=18" },
            new Product { Id = 19, Name = "Dell S2722DGM", Description = "1440p gaming monitor with 165Hz and HDR support", CategoryId = 4, Price = 449, ImageUrl = "https://picsum.photos/300/400?random=19" },
            new Product { Id = 20, Name = "ASUS VP28UQG", Description = "28\" 4K monitor with 1ms response time for gaming", CategoryId = 4, Price = 349, ImageUrl = "https://picsum.photos/300/400?random=20" },
            new Product { Id = 21, Name = "BenQ SW240", Description = "Professional color-accurate monitor for designers and photo editors", CategoryId = 4, Price = 699, ImageUrl = "https://picsum.photos/300/400?random=21" },
            
            // Category 5: Furniture & Workspace
            new Product { Id = 22, Name = "Herman Miller Aeron Chair", Description = "Ergonomic office chair with adjustable lumbar support", CategoryId = 5, Price = 1395, ImageUrl = "https://picsum.photos/300/400?random=22" },
            new Product { Id = 23, Name = "Flexispot E7", Description = "Electric standing desk with memory presets and smooth movement", CategoryId = 5, Price = 599, ImageUrl = "https://picsum.photos/300/400?random=23" },
            new Product { Id = 24, Name = "Autonomous Standing Desk", Description = "Motorized desk with dual motor for stability and quick adjustment", CategoryId = 5, Price = 799, ImageUrl = "https://picsum.photos/300/400?random=24" },
            new Product { Id = 25, Name = "IKEA Strandtorp Desk", Description = "Solid wood desk with classic design and ample workspace", CategoryId = 5, Price = 299, ImageUrl = "https://picsum.photos/300/400?random=25" },
            
            // Category 6: Lighting & Ambiance
            new Product { Id = 26, Name = "Philips Hue Light Strip Plus", Description = "Smart LED strip with 16M colors and voice control", CategoryId = 6, Price = 89, ImageUrl = "https://picsum.photos/300/400?random=26" },
            new Product { Id = 27, Name = "Elgato Key Light", Description = "Professional streaming light with variable color temperature", CategoryId = 6, Price = 199, ImageUrl = "https://picsum.photos/300/400?random=27" }
        };

        private int _nextId = 28;

        /// <summary>
        /// Get all products
        /// </summary>
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await Task.FromResult(_products.AsReadOnly());
        }

        /// <summary>
        /// Get product by ID
        /// </summary>
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await Task.FromResult(_products.FirstOrDefault(p => p.Id == id));
        }

        /// <summary>
        /// Get products by category ID
        /// </summary>
        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            return await Task.FromResult(_products.Where(p => p.CategoryId == categoryId));
        }

        /// <summary>
        /// Search products by search term (searches name and description)
        /// </summary>
        public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await Task.FromResult(Array.Empty<Product>());

            var term = searchTerm.ToLower();
            return await Task.FromResult(_products
                .Where(p => p.Name.ToLower().Contains(term) || p.Description.ToLower().Contains(term))
                );
        }

        /// <summary>
        /// Create a new product
        /// </summary>
        public async Task<Product> CreateProductAsync(Product product)
        {
            product.Id = _nextId++;
            _products.Add(product);
            return await Task.FromResult(product);
        }

        /// <summary>
        /// Update an existing product
        /// </summary>
        public async Task<Product?> UpdateProductAsync(int id, Product product)
        {
            var existingProduct = _products.FirstOrDefault(p => p.Id == id);
            if (existingProduct == null)
                return null;

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.CategoryId = product.CategoryId;
            existingProduct.ImageUrl = product.ImageUrl;
            existingProduct.Quantity = product.Quantity;

            return await Task.FromResult(existingProduct);
        }

        /// <summary>
        /// Delete a product by ID
        /// </summary>
        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return false;

            _products.Remove(product);
            return await Task.FromResult(true);
        }
    }
}
