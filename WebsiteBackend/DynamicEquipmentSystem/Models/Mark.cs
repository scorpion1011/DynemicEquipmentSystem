using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicEquipmentSystem.Models
{
    public class Mark
    {
        public int IdMark { get; set; }
        public string Name { get; set; }
        public string IdStation { get; set; }
        public bool IsActive { get; set; }
        public bool IsGotten { get; set; }
    }
}
