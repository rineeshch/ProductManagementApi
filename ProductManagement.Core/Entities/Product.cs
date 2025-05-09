namespace ProductManagement.Core.Entities
{
    public class Product
    {
        public int Id { get; set; }  // 6-digit unique identifier
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockAvailable { get; set; }  // Required by specification
    }
}
