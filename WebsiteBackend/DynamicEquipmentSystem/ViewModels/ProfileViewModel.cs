using DynamicEquipmentSystem.Validators;
using System;
using System.ComponentModel.DataAnnotations;

namespace DynamicEquipmentSystem.ViewModels
{
    public class ProfileViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public int Year { get; set; }
        [RequiredIfNotEmpty]
        public string Password { get; set; }
        public string IdStation { get; set; }
    }
}