using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicEquipmentSystem.Extentions
{
    public static class Alerts
    {
        public static IActionResult WithSuccess(this IActionResult result, string title)
        {
            return Alert(result, "success", title, string.Empty);
        }

        private static IActionResult Alert(IActionResult result, string type, string title, string body)
        {
            return new AlertDecoratorResult(result, type, title, body);
        }
    }
}
