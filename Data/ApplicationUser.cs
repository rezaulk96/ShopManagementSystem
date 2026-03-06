using Microsoft.AspNetCore.Identity;

namespace ShopManagementSystem.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; } = default!;
        public string Phone { get; set; } = default!;
    }
}
