using System;
using System.ComponentModel.DataAnnotations;

namespace DynamicEquipmentSystem.ViewModels
{
    public class ProfileViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public int Year { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string IdStation { get; set; }
    }
}