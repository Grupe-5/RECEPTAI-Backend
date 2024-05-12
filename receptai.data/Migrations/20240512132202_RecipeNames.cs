using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace receptai.data.Migrations
{
    /// <inheritdoc />
    public partial class RecipeNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SubfoodditName",
                table: "Recipes",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Recipes",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubfoodditName",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Recipes");
        }
    }
}
