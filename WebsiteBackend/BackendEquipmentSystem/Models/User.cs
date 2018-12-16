using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace DynamicEquipmentSystem.Models
{
    public class User : IdentityUser
    {
        public int Year { get; set; }
    }
}