using AnayaCore.Models.Main;

namespace AnayaCore.Models.Helper
{
    /// <summary>
    /// Request model for adding product to cart
    /// </summary>
    public class AddToCartRequest
    {
        public Product Product { get; set; } = new Product();
        public int Quantity { get; set; } = 1;
    }
}
