using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForkAndSpoon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDietaryPreferenceSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DietaryPreferences",
                table: "Recipes");

            migrationBuilder.CreateTable(
                name: "DietaryPreferences",
                columns: table => new
                {
                    DietaryPreferenceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DietaryPreferences", x => x.DietaryPreferenceID);
                });

            migrationBuilder.CreateTable(
                name: "RecipeDietaryPreferences",
                columns: table => new
                {
                    RecipeID = table.Column<int>(type: "int", nullable: false),
                    DietaryPreferenceID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeDietaryPreferences", x => new { x.RecipeID, x.DietaryPreferenceID });
                    table.ForeignKey(
                        name: "FK_RecipeDietaryPreferences_DietaryPreferences_DietaryPreferenceID",
                        column: x => x.DietaryPreferenceID,
                        principalTable: "DietaryPreferences",
                        principalColumn: "DietaryPreferenceID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeDietaryPreferences_Recipes_RecipeID",
                        column: x => x.RecipeID,
                        principalTable: "Recipes",
                        principalColumn: "RecipeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecipeDietaryPreferences_DietaryPreferenceID",
                table: "RecipeDietaryPreferences",
                column: "DietaryPreferenceID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecipeDietaryPreferences");

            migrationBuilder.DropTable(
                name: "DietaryPreferences");

            migrationBuilder.AddColumn<string>(
                name: "DietaryPreferences",
                table: "Recipes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
