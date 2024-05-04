using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace receptai.data.Migrations
{
    /// <inheritdoc />
    public partial class DbCorrections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_Subreddits_SubredditId",
                table: "Recipes");

            migrationBuilder.DropTable(
                name: "SubredditUser");

            migrationBuilder.DropTable(
                name: "Subreddits");

            migrationBuilder.RenameColumn(
                name: "SubredditId",
                table: "Recipes",
                newName: "SubfoodditId");

            migrationBuilder.RenameIndex(
                name: "IX_Recipes_SubredditId",
                table: "Recipes",
                newName: "IX_Recipes_SubfoodditId");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Recipes",
                type: "TEXT",
                maxLength: 5000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 5000);

            migrationBuilder.AlterColumn<string>(
                name: "CookingTime",
                table: "Recipes",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "CookingDifficulty",
                table: "Recipes",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.CreateTable(
                name: "Subfooddits",
                columns: table => new
                {
                    SubfoodditId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subfooddits", x => x.SubfoodditId);
                });

            migrationBuilder.CreateTable(
                name: "SubfoodditUser",
                columns: table => new
                {
                    SubfoodditsSubfoodditId = table.Column<int>(type: "INTEGER", nullable: false),
                    UsersUserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubfoodditUser", x => new { x.SubfoodditsSubfoodditId, x.UsersUserId });
                    table.ForeignKey(
                        name: "FK_SubfoodditUser_Subfooddits_SubfoodditsSubfoodditId",
                        column: x => x.SubfoodditsSubfoodditId,
                        principalTable: "Subfooddits",
                        principalColumn: "SubfoodditId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubfoodditUser_Users_UsersUserId",
                        column: x => x.UsersUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubfoodditUser_UsersUserId",
                table: "SubfoodditUser",
                column: "UsersUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_Subfooddits_SubfoodditId",
                table: "Recipes",
                column: "SubfoodditId",
                principalTable: "Subfooddits",
                principalColumn: "SubfoodditId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_Subfooddits_SubfoodditId",
                table: "Recipes");

            migrationBuilder.DropTable(
                name: "SubfoodditUser");

            migrationBuilder.DropTable(
                name: "Subfooddits");

            migrationBuilder.RenameColumn(
                name: "SubfoodditId",
                table: "Recipes",
                newName: "SubredditId");

            migrationBuilder.RenameIndex(
                name: "IX_Recipes_SubfoodditId",
                table: "Recipes",
                newName: "IX_Recipes_SubredditId");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Recipes",
                type: "TEXT",
                maxLength: 5000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 5000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CookingTime",
                table: "Recipes",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CookingDifficulty",
                table: "Recipes",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.CreateTable(
                name: "Subreddits",
                columns: table => new
                {
                    SubredditId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subreddits", x => x.SubredditId);
                });

            migrationBuilder.CreateTable(
                name: "SubredditUser",
                columns: table => new
                {
                    SubredditsSubredditId = table.Column<int>(type: "INTEGER", nullable: false),
                    UsersUserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubredditUser", x => new { x.SubredditsSubredditId, x.UsersUserId });
                    table.ForeignKey(
                        name: "FK_SubredditUser_Subreddits_SubredditsSubredditId",
                        column: x => x.SubredditsSubredditId,
                        principalTable: "Subreddits",
                        principalColumn: "SubredditId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubredditUser_Users_UsersUserId",
                        column: x => x.UsersUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubredditUser_UsersUserId",
                table: "SubredditUser",
                column: "UsersUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_Subreddits_SubredditId",
                table: "Recipes",
                column: "SubredditId",
                principalTable: "Subreddits",
                principalColumn: "SubredditId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
