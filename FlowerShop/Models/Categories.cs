//using Microsoft.AspNetCore.Authorization;

namespace FlowerShop.Models
{
   // [Authorize(Roles ="Admin")]
    public class Categories
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
