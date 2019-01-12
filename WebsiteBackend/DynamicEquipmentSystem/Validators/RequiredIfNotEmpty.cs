using DynamicEquipmentSystem.Models;
using DynamicEquipmentSystem.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicEquipmentSystem.Validators
{
    public class RequiredIfNotEmpty : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ProfileViewModel profile = (ProfileViewModel)validationContext.ObjectInstance;

            if (profile.Password != null && profile.Password.Length > 0 && profile.Password.Length < 5)
            {
                return new ValidationResult("Passwords must be at least 5 characters.");
            }

            return ValidationResult.Success;
        }
    }
}
