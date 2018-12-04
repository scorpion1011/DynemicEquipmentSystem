using Microsoft.AspNetCore.Identity;

namespace DynamicEquipmentSystem.Models
{
    public class User : IdentityUser
    {
        public int Year { get; set; }
    }
}