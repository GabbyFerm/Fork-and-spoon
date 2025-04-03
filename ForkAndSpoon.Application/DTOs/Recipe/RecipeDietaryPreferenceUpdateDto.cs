using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForkAndSpoon.Application.DTOs.Recipe
{
    public class RecipeDietaryPreferenceUpdateDto
    {
        public List<string> DietaryPreferences { get; set; } = new();
    }
}
