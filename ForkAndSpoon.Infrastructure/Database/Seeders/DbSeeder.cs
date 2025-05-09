using Bogus;
using ForkAndSpoon.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ForkAndSpoon.Infrastructure.Database.Seeders
{
    public class DbSeeder
    {
        private readonly ForkAndSpoonDbContext _context;

        public DbSeeder(ForkAndSpoonDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            // Ensure Admin user exists
            if (!await _context.Users.AnyAsync(user => user.Email == "admin@forkandspoon.com"))
            {
                var adminUser = new User
                {
                    UserName = "admin",
                    Email = "admin@forkandspoon.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("Admin123!"), // use a test password
                    Role = "Admin"
                };
                _context.Users.Add(adminUser);
            }

            // Seed Users — only if fewer than 5 exist
            if (await _context.Users.CountAsync() < 5)
            {
                var userFaker = new Faker<User>()
                    .RuleFor(user => user.UserName, fake => fake.Internet.UserName())
                    .RuleFor(user => user.Email, fake => fake.Internet.Email())
                    .RuleFor(user => user.Password, fake => BCrypt.Net.BCrypt.HashPassword("Password123"))
                    .RuleFor(user => user.Role, _ => "User");

                var fakeUsers = userFaker.Generate(3);
                _context.Users.AddRange(fakeUsers);
                await _context.SaveChangesAsync();
            }

            // Seed Categories if missing
            var defaultCategories = new List<string> { "Dinner", "Salads", "Soups" };

            foreach (var name in defaultCategories)
            {
                if (!await _context.Categories.AnyAsync(c => c.Name == name))
                {
                    _context.Categories.Add(new Category { Name = name });
                }
            }

            await _context.SaveChangesAsync();

            // Seed Recipes — only if fewer than 2 exist
            if (await _context.Recipes.CountAsync() < 2)
            {
                var existingUser = await _context.Users.FirstOrDefaultAsync();
                var existingCategory = await _context.Categories.FirstOrDefaultAsync();

                if (existingUser != null && existingCategory != null)
                {
                    var recipes = new List<Recipe>
                    {
                        new()
                        {
                            Title = "Spaghetti Bolognese",
                            Steps = "Cook pasta. Make meat sauce. Combine and serve.",
                            CategoryID = existingCategory.CategoryID,
                            ImageUrl = "https://example.com/spaghetti.jpg",
                            CreatedBy = existingUser.UserID,
                            RecipeIngredients = new List<RecipeIngredient>
                            {
                                new() { Ingredient = new Ingredient { Name = "Spaghetti" }, Quantity = "200g" },
                                new() { Ingredient = new Ingredient { Name = "Minced meat" }, Quantity = "300g" },
                                new() { Ingredient = new Ingredient { Name = "Tomato sauce" }, Quantity = "1 cup" }
                            }
                        },
                        new()
                        {
                            Title = "Avocado Toast",
                            Steps = "Toast bread. Smash avocado. Add toppings. Enjoy.",
                            CategoryID = existingCategory.CategoryID,
                            ImageUrl = "https://example.com/avocado.jpg",
                            CreatedBy = existingUser.UserID,
                            RecipeIngredients = new List<RecipeIngredient>
                            {
                                new() { Ingredient = new Ingredient { Name = "Bread" }, Quantity = "2 slices" },
                                new() { Ingredient = new Ingredient { Name = "Avocado" }, Quantity = "1 whole" },
                                new() { Ingredient = new Ingredient { Name = "Salt" }, Quantity = "To taste" }
                            }
                        }
                    };

                    _context.Recipes.AddRange(recipes);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }

}