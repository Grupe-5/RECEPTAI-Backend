using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace receptai.data.Migrations
{
    /// <inheritdoc />
    public partial class RecipeFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Recipes");

            migrationBuilder.AddColumn<int>(
                name: "AggregatedVotes",
                table: "Recipes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AggregatedVotes",
                table: "Recipes");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Recipes",
                type: "TEXT",
                maxLength: 5000,
                nullable: true);
        }
    }
}
