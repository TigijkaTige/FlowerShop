namespace FlowerShop.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int ProductId { get; set; }    
        public Product ? Product { get; set; }
        public string UserId { get; set; }
        public User ? User{ get; set; }
        public int CountPr { get; set; }
    }
}
