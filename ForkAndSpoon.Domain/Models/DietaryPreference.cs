using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForkAndSpoon.Domain.Models
{
    public class DietaryPreference
    {
        public int DietaryPreferenceID { get; set; }
        public string Name { get; set; } = null!;

        public ICollection<RecipeDietaryPreference> RecipeDietaryPreferences { get; set; } = new List<RecipeDietaryPreference>();
    }
}
