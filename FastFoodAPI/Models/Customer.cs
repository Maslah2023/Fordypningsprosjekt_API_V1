using Microsoft.AspNetCore.Identity;

namespace FastFoodHouse_API.Models
{
    public class Customer : IdentityUser
    {
        public string Name { get; set; }
    }
}
