using FlowerShop.Models;

namespace FlowerShop.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Cost { get; set; }
        public string? FileName { get; set; }
        public byte[]? Picture { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
    }
}
