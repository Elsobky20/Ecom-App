namespace Ecom.core.Entites.Order
{
    public class OrderItem : BaseEntity<int>
    {
        public OrderItem()
        {
            
        }
        public OrderItem(int productItemId, string productName, string mainImage, int quntity, decimal price)
        {
            ProductItemId = productItemId;
            ProductName = productName;
            MainImage = mainImage;
            Quntity = quntity;
            Price = price;
        }

        public int ProductItemId { get; set; }
        public string ProductName { get; set; }
        public string MainImage { get; set; }
        public int Quntity { get; set; }
        public decimal Price { get; set; }
    }
}