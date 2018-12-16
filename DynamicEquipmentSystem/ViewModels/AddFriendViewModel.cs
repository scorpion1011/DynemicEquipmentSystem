using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace DynamicEquipmentSystem.ViewModels
{
    public class AddFriendViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
