namespace FlowerShop.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }  
        public Product ? Product { get; set; }
        public int OrdersId { get; set; }
        public int CountPr { get; set; }
    }
}
