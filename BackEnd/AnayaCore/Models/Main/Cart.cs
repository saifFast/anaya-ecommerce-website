namespace AnayaCore.Models.Main
{
    public class Cart
    {
        public int Id { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
        public Customer CurrentCustomer { get; set; } = new Customer();
    }
}
