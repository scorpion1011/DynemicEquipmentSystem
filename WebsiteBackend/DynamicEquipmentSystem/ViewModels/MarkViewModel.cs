using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DynamicEquipmentSystem.ViewModels
{
    public class MarkViewModel
    {
        public int IdMark { get; set; }
        [Required]
        [Display(Name = "Назва маркеру")]
        public string Name { get; set; }
        [Display(Name = "Цей маркер ваш?")]
        public bool IsActive { get; set; }
    }
}
