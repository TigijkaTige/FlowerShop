namespace FlowerShop.Models
{
    public class Orders
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User{ get; set; }
        public string Address { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliverDate { get; set; }
        public bool Delivered { get; set; }
        public double Summa { get; set; }
        public List<OrderItem> items { get; set; }
    }
}
