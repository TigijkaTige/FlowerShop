using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FlowerShop.Models
{
    public class User: IdentityUser
    {
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }
    }
}
