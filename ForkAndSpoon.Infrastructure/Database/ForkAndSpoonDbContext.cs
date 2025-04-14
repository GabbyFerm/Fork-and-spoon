using ForkAndSpoon.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ForkAndSpoon.Infrastructure.Database
{
    public class ForkAndSpoonDbContext : DbContext
    {
        public ForkAndSpoonDbContext(DbContextOptions<ForkAndSpoonDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
        public DbSet<DietaryPreference> DietaryPreferences { get; set; }
        public DbSet<RecipeDietaryPreference> RecipeDietaryPreferences { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<FavoriteRecipe> FavoriteRecipes { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Many-to-Many: RecipeIngredient (Recipes <-> Ingredients)
            modelBuilder.Entity<RecipeIngredient>()
                .HasKey(recipeIngredient => new { recipeIngredient.RecipeID, recipeIngredient.IngredientID });

            modelBuilder.Entity<RecipeIngredient>()
                .HasOne(recipeIngredient => recipeIngredient.Recipe)
                .WithMany(recipe => recipe.RecipeIngredients)
                .HasForeignKey(recipeIngredient => recipeIngredient.RecipeID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RecipeIngredient>()
                .HasOne(recipeIngredient => recipeIngredient.Ingredient)
                .WithMany(ingredient => ingredient.RecipeIngredients)
                .HasForeignKey(recipeIngredient => recipeIngredient.IngredientID)
                .OnDelete(DeleteBehavior.Cascade);

            // Many-to-Many: Favorite (Users <-> Recipes)
            modelBuilder.Entity<FavoriteRecipe>()
                .ToTable("FavoriteRecipes")
                .HasKey(favorite => new { favorite.UserID, favorite.RecipeID });

            modelBuilder.Entity<FavoriteRecipe>()
                .HasOne(favorite => favorite.User)
                .WithMany(user => user.FavoriteRecipes)
                .HasForeignKey(favorite => favorite.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FavoriteRecipe>()
                .HasOne(favorite => favorite.Recipe)
                .WithMany(recipe => recipe.FavoriteRecipes)
                .HasForeignKey(favorite => favorite.RecipeID)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-Many: Recipe <-> Category
            modelBuilder.Entity<Recipe>()
                .HasOne(recipe => recipe.Category)
                .WithMany(category => category.Recipes)
                .HasForeignKey(recipe => recipe.CategoryID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Recipe>()
                .HasOne(recipe => recipe.CreatedByUser)
                .WithMany(user => user.CreatedRecipes)
                .HasForeignKey(recipe => recipe.CreatedBy)
                .OnDelete(DeleteBehavior.SetNull);

            // Many-to-Many: Ratings (Users <-> Recipes)
            modelBuilder.Entity<Rating>()
                .HasKey(rating => rating.RatingID);

            modelBuilder.Entity<Rating>()
                .HasOne(rating => rating.User)
                .WithMany(user => user.Ratings)
                .HasForeignKey(rating => rating.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Rating>()
                .HasOne(rating => rating.Recipe)
                .WithMany(recipe => recipe.Ratings)
                .HasForeignKey(rating => rating.RecipeID)
                .OnDelete(DeleteBehavior.Restrict);

            // Many-to-Many: Recipe <-> DietaryPreference
            modelBuilder.Entity<RecipeDietaryPreference>()
                .HasKey(rdp => new { rdp.RecipeID, rdp.DietaryPreferenceID });

            modelBuilder.Entity<RecipeDietaryPreference>()
                .HasOne(rdp => rdp.Recipe)
                .WithMany(recipe => recipe.RecipeDietaryPreferences)
                .HasForeignKey(rdp => rdp.RecipeID);

            modelBuilder.Entity<RecipeDietaryPreference>()
                .HasOne(rdp => rdp.DietaryPreference)
                .WithMany(dp => dp.RecipeDietaryPreferences)
                .HasForeignKey(rdp => rdp.DietaryPreferenceID);

            // Default category
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryID = 1, Name = "Uncategorized" }
            );
        }
    }
}