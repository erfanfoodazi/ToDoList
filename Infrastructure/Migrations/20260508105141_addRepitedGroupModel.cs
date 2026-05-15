using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addRepitedGroupModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupRepetitionId",
                table: "TodoItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepitedGroupId",
                table: "TodoItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RepitedGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    RepetitionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepitedGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepitedGroups_GroupItems_GroupId",
                        column: x => x.GroupId,
                        principalTable: "GroupItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TodoItems_RepitedGroupId",
                table: "TodoItems",
                column: "RepitedGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_RepitedGroups_GroupId",
                table: "RepitedGroups",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoItems_RepitedGroups_RepitedGroupId",
                table: "TodoItems",
                column: "RepitedGroupId",
                principalTable: "RepitedGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoItems_RepitedGroups_RepitedGroupId",
                table: "TodoItems");

            migrationBuilder.DropTable(
                name: "RepitedGroups");

            migrationBuilder.DropIndex(
                name: "IX_TodoItems_RepitedGroupId",
                table: "TodoItems");

            migrationBuilder.DropColumn(
                name: "GroupRepetitionId",
                table: "TodoItems");

            migrationBuilder.DropColumn(
                name: "RepitedGroupId",
                table: "TodoItems");
        }
    }
}
