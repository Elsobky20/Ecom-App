namespace Ecom.core.Entites
{
    public class BasketItem
    {
        public int Id { get; set; }
        public string Name { set; get; }

        public string Description { set; get; }
        public string Image { get; set; }
        public int Quentity { get; set; }
        public decimal Price { set; get; }
        public string Category { get; set; }
    }
}