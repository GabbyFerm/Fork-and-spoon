using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForkAndSpoon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRecipeModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_Users_CreatedByUserUserID",
                table: "Recipes");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedByUserUserID",
                table: "Recipes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_Users_CreatedByUserUserID",
                table: "Recipes",
                column: "CreatedByUserUserID",
                principalTable: "Users",
                principalColumn: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_Users_CreatedByUserUserID",
                table: "Recipes");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedByUserUserID",
                table: "Recipes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_Users_CreatedByUserUserID",
                table: "Recipes",
                column: "CreatedByUserUserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
