using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForkAndSpoon.Domain.Models
{
    public class User
    {
        public int UserID { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }

        public string Role { get; set; } = "User"; // Default role to those who registers

        // Navigation properties
        public ICollection<Recipe> CreatedRecipes { get; set; } = new List<Recipe>();
        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    }
}
