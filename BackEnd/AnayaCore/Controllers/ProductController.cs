using Microsoft.AspNetCore.Mvc;
using AnayaCore.Models.Main;

namespace AnayaCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        // In-memory database (replace with actual database context later)
        private static List<Product> products = new List<Product>
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
            new Product { Id = 27, Name = "Elgato Key Light", Description = "Professional streaming light with variable color temperature", CategoryId = 6, Price = 199, ImageUrl = "https://picsum.photos/300/400?random=27" },
            new Product { Id = 28, Name = "Nanoleaf Essentials", Description = "Modular triangular panels with dynamic effects and music sync", CategoryId = 6, Price = 229, ImageUrl = "https://picsum.photos/300/400?random=28" },
            new Product { Id = 29, Name = "Dyson Lightcycle", Description = "Advanced LED light that adapts to your circadian rhythm", CategoryId = 6, Price = 599, ImageUrl = "https://picsum.photos/300/400?random=29" },
            
            // Category 7: Cables & Connectivity
            new Product { Id = 30, Name = "Belkin Thunderbolt 3 Cable", Description = "High-speed data transfer and charging cable, 6.6 feet", CategoryId = 7, Price = 39, ImageUrl = "https://picsum.photos/300/400?random=30" },
            new Product { Id = 31, Name = "Anker USB-C to Lightning Cable", Description = "Certified durable cable with 10000+ bend test certification", CategoryId = 7, Price = 12, ImageUrl = "https://picsum.photos/300/400?random=31" },
            new Product { Id = 32, Name = "Nylon Braided HDMI 2.1", Description = "High-speed HDMI cable for 8K video and enhanced audio", CategoryId = 7, Price = 25, ImageUrl = "https://picsum.photos/300/400?random=32" },
            new Product { Id = 33, Name = "CAT6A Ethernet Cable Bulk", Description = "100ft network cable for fast and stable internet connection", CategoryId = 7, Price = 49, ImageUrl = "https://picsum.photos/300/400?random=33" }
        };

        /// <summary>
        /// Get all products
        /// </summary>
        /// <returns>List of all products</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Product>> GetAllProducts()
        {
            return Ok(products);
        }

        /// <summary>
        /// Get product by ID
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns>Product details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Product> GetProductById(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound(new { message = $"Product with ID {id} not found" });
            }
            return Ok(product);
        }

        /// <summary>
        /// Create a new product
        /// </summary>
        /// <param name="product">Product data</param>
        /// <returns>Created product</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Product> CreateProduct([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest(new { message = "Product data is required" });
            }

            if (string.IsNullOrWhiteSpace(product.Name))
            {
                return BadRequest(new { message = "Product name is required" });
            }

            // Generate new ID
            product.Id = products.Count > 0 ? products.Max(p => p.Id) + 1 : 1;
            products.Add(product);

            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        /// <summary>
        /// Update an existing product
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <param name="product">Updated product data</param>
        /// <returns>Updated product</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Product> UpdateProduct(int id, [FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest(new { message = "Product data is required" });
            }

            var existingProduct = products.FirstOrDefault(p => p.Id == id);
            if (existingProduct == null)
            {
                return NotFound(new { message = $"Product with ID {id} not found" });
            }

            if (string.IsNullOrWhiteSpace(product.Name))
            {
                return BadRequest(new { message = "Product name is required" });
            }

            // Update properties
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.CategoryId = product.CategoryId;
            existingProduct.Price = product.Price;

            return Ok(existingProduct);
        }

        /// <summary>
        /// Delete a product by ID
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns>Success message</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteProduct(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound(new { message = $"Product with ID {id} not found" });
            }

            products.Remove(product);
            return Ok(new { message = $"Product with ID {id} has been deleted successfully" });
        }

        /// <summary>
        /// Get products by category
        /// </summary>
        /// <param name="categoryId">Category ID</param>
        /// <returns>List of products in the category</returns>
        [HttpGet("category/{categoryId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Product>> GetProductsByCategory(int categoryId)
        {
            var categoryProducts = products.Where(p => p.CategoryId == categoryId).ToList();
            return Ok(categoryProducts);
        }

        /// <summary>
        /// Search products by name
        /// </summary>
        /// <param name="searchTerm">Search term</param>
        /// <returns>List of products matching search criteria</returns>
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Product>> SearchProducts([FromQuery] string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return Ok(products);
            }

            var results = products.Where(p => 
                p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                p.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
            ).ToList();

            return Ok(results);
        }
    }
}
