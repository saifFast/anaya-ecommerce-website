using Microsoft.AspNetCore.Mvc;
using AnayaCore.Models;
using AnayaCore.Services;

namespace AnayaCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        /// <summary>
        /// Get cart by ID
        /// GET: api/cart/{id}
        /// </summary>
        [HttpGet("{id}")]
        public ActionResult<Cart> GetCart(int id)
        {
            try
            {
                var cart = _cartService.GetCart(id);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error retrieving cart", error = ex.Message });
            }
        }

        /// <summary>
        /// Add product to cart
        /// POST: api/cart/{cartId}/add
        /// </summary>
        [HttpPost("{cartId}/add")]
        public ActionResult<Cart> AddProductToCart(int cartId, [FromBody] AddToCartRequest request)
        {
            if (request == null || request.Product == null || request.Quantity <= 0)
            {
                return BadRequest(new { message = "Invalid product or quantity" });
            }

            try
            {
                var cart = _cartService.AddProductToCart(cartId, request.Product, request.Quantity);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error adding product to cart", error = ex.Message });
            }
        }

        /// <summary>
        /// Remove product from cart
        /// DELETE: api/cart/{cartId}/remove/{productId}
        /// </summary>
        [HttpDelete("{cartId}/remove/{productId}")]
        public ActionResult<Cart> RemoveProductFromCart(int cartId, int productId)
        {
            try
            {
                var cart = _cartService.RemoveProductFromCart(cartId, productId);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error removing product from cart", error = ex.Message });
            }
        }

        /// <summary>
        /// Update product quantity in cart
        /// PUT: api/cart/{cartId}/update/{productId}
        /// </summary>
        [HttpPut("{cartId}/update/{productId}")]
        public ActionResult<Cart> UpdateProductQuantity(int cartId, int productId, [FromBody] QuantityUpdateRequest request)
        {
            if (request == null || request.Quantity <= 0)
            {
                return BadRequest(new { message = "Invalid quantity" });
            }

            try
            {
                var cart = _cartService.UpdateProductQuantity(cartId, productId, request.Quantity);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error updating product quantity", error = ex.Message });
            }
        }

        /// <summary>
        /// Get cart total price
        /// GET: api/cart/{cartId}/total
        /// </summary>
        [HttpGet("{cartId}/total")]
        public ActionResult<CartTotalResponse> GetCartTotal(int cartId)
        {
            try
            {
                var total = _cartService.GetCartTotal(cartId);
                return Ok(new CartTotalResponse { Total = total });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error calculating cart total", error = ex.Message });
            }
        }

        /// <summary>
        /// Get cart items count
        /// GET: api/cart/{cartId}/count
        /// </summary>
        [HttpGet("{cartId}/count")]
        public ActionResult<CartCountResponse> GetCartItemsCount(int cartId)
        {
            try
            {
                var count = _cartService.GetCartItemsCount(cartId);
                return Ok(new CartCountResponse { ItemsCount = count });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error getting cart items count", error = ex.Message });
            }
        }

        /// <summary>
        /// Clear all products from cart
        /// DELETE: api/cart/{cartId}/clear
        /// </summary>
        [HttpDelete("{cartId}/clear")]
        public ActionResult<Cart> ClearCart(int cartId)
        {
            try
            {
                var cart = _cartService.ClearCart(cartId);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error clearing cart", error = ex.Message });
            }
        }

        /// <summary>
        /// Set customer information for the cart
        /// POST: api/cart/{cartId}/customer
        /// </summary>
        [HttpPost("{cartId}/customer")]
        public ActionResult<Cart> SetCustomerInfo(int cartId, [FromBody] Customer customer)
        {
            if (customer == null)
            {
                return BadRequest(new { message = "Invalid customer information" });
            }

            try
            {
                var cart = _cartService.SetCustomerInfo(cartId, customer);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error setting customer information", error = ex.Message });
            }
        }

        /// <summary>
        /// Create a new cart
        /// POST: api/cart/create
        /// </summary>
        [HttpPost("create")]
        public ActionResult<Cart> CreateNewCart()
        {
            try
            {
                var cart = _cartService.CreateNewCart();
                return CreatedAtAction(nameof(GetCart), new { id = cart.Id }, cart);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error creating new cart", error = ex.Message });
            }
        }

        /// <summary>
        /// Get all carts (admin only)
        /// GET: api/cart/all
        /// </summary>
        [HttpGet("all")]
        public ActionResult<List<Cart>> GetAllCarts()
        {
            try
            {
                var carts = _cartService.GetAllCarts();
                return Ok(carts);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error retrieving carts", error = ex.Message });
            }
        }
    }

    /// <summary>
    /// Request model for adding product to cart
    /// </summary>
    public class AddToCartRequest
    {
        public Product Product { get; set; } = new Product();
        public int Quantity { get; set; } = 1;
    }

    /// <summary>
    /// Request model for updating product quantity
    /// </summary>
    public class QuantityUpdateRequest
    {
        public int Quantity { get; set; }
    }

    /// <summary>
    /// Response model for cart total
    /// </summary>
    public class CartTotalResponse
    {
        public decimal Total { get; set; }
    }

    /// <summary>
    /// Response model for cart items count
    /// </summary>
    public class CartCountResponse
    {
        public int ItemsCount { get; set; }
    }
}
