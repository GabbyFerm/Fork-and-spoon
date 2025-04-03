using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForkAndSpoon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDietaryPreferencesToRecipe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DietaryPreferences",
                table: "Recipes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DietaryPreferences",
                table: "Recipes");
        }
    }
}
