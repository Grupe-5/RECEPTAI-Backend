using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace receptai.data.Migrations
{
    /// <inheritdoc />
    public partial class CommentVoteFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AggregatedVotes",
                table: "Comments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AggregatedVotes",
                table: "Comments");
        }
    }
}
