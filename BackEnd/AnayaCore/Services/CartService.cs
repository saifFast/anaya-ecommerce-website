using AnayaCore.Models;

namespace AnayaCore.Services
{
    public interface ICartService
    {
        Cart GetCart(int cartId);
        Cart AddProductToCart(int cartId, Product product, int quantity);
        Cart RemoveProductFromCart(int cartId, int productId);
        Cart UpdateProductQuantity(int cartId, int productId, int quantity);
        decimal GetCartTotal(int cartId);
        int GetCartItemsCount(int cartId);
        Cart ClearCart(int cartId);
        Cart SetCustomerInfo(int cartId, Customer customer);
        Cart CreateNewCart();
        List<Cart> GetAllCarts();
    }

    public class CartService : ICartService
    {
        // In-memory storage for carts (replace with database in production)
        private static Dictionary<int, Cart> _carts = new Dictionary<int, Cart>();
        private static int _cartCounter = 1;

        public CartService()
        {
            // Initialize with a default cart if empty
            if (_carts.Count == 0)
            {
                _carts[1] = new Cart { Id = 1, Products = new List<Product>(), CurrentCustomer = new Customer() };
                _cartCounter = 2;
            }
        }

        /// <summary>
        /// Get cart by ID
        /// </summary>
        public Cart GetCart(int cartId)
        {
            if (_carts.ContainsKey(cartId))
            {
                return _carts[cartId];
            }

            // If cart doesn't exist, create a new one
            var newCart = new Cart { Id = cartId, Products = new List<Product>(), CurrentCustomer = new Customer() };
            _carts[cartId] = newCart;
            return newCart;
        }

        /// <summary>
        /// Add product to cart with quantity
        /// </summary>
        public Cart AddProductToCart(int cartId, Product product, int quantity)
        {
            var cart = GetCart(cartId);

            var existingProduct = cart.Products.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct != null)
            {
                // If product already in cart, increase quantity would be handled on client side
                // For now, we'll just return the cart
                return cart;
            }

            // Add product to cart
            product.Quantity = quantity; // Assuming Product has a Quantity property or we can track it differently
            cart.Products.Add(product);

            return cart;
        }

        /// <summary>
        /// Remove product from cart
        /// </summary>
        public Cart RemoveProductFromCart(int cartId, int productId)
        {
            var cart = GetCart(cartId);
            var product = cart.Products.FirstOrDefault(p => p.Id == productId);

            if (product != null)
            {
                cart.Products.Remove(product);
            }

            return cart;
        }

        /// <summary>
        /// Update product quantity in cart
        /// </summary>
        public Cart UpdateProductQuantity(int cartId, int productId, int quantity)
        {
            var cart = GetCart(cartId);
            var product = cart.Products.FirstOrDefault(p => p.Id == productId);

            if (product != null && quantity > 0)
            {
                product.Quantity = quantity;
            }
            else if (product != null && quantity <= 0)
            {
                // Remove product if quantity is 0 or less
                cart.Products.Remove(product);
            }

            return cart;
        }

        /// <summary>
        /// Calculate total price of cart
        /// </summary>
        public decimal GetCartTotal(int cartId)
        {
            var cart = GetCart(cartId);
            return cart.Products.Sum(p => p.Price * (p.Quantity > 0 ? p.Quantity : 1));
        }

        /// <summary>
        /// Get total number of items in cart
        /// </summary>
        public int GetCartItemsCount(int cartId)
        {
            var cart = GetCart(cartId);
            return cart.Products.Sum(p => p.Quantity > 0 ? p.Quantity : 1);
        }

        /// <summary>
        /// Clear all products from cart
        /// </summary>
        public Cart ClearCart(int cartId)
        {
            var cart = GetCart(cartId);
            cart.Products.Clear();
            return cart;
        }

        /// <summary>
        /// Set customer info for the cart
        /// </summary>
        public Cart SetCustomerInfo(int cartId, Customer customer)
        {
            var cart = GetCart(cartId);
            cart.CurrentCustomer = customer;
            return cart;
        }

        /// <summary>
        /// Create a new cart
        /// </summary>
        public Cart CreateNewCart()
        {
            var cartId = _cartCounter++;
            var newCart = new Cart { Id = cartId, Products = new List<Product>(), CurrentCustomer = new Customer() };
            _carts[cartId] = newCart;
            return newCart;
        }

        /// <summary>
        /// Get all carts (for admin purposes)
        /// </summary>
        public List<Cart> GetAllCarts()
        {
            return _carts.Values.ToList();
        }
    }
}
