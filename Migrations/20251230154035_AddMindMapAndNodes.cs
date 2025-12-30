using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddMindMapAndNodes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MindMaps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MindMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MindMaps_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MindMapNodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MindMapId = table.Column<int>(type: "int", nullable: false),
                    FlashCardId = table.Column<int>(type: "int", nullable: false),
                    ParentNodeId = table.Column<int>(type: "int", nullable: true),
                    PositionX = table.Column<double>(type: "float", nullable: false),
                    PositionY = table.Column<double>(type: "float", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HideChildren = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MindMapNodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MindMapNodes_FlashCards_FlashCardId",
                        column: x => x.FlashCardId,
                        principalTable: "FlashCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MindMapNodes_MindMapNodes_ParentNodeId",
                        column: x => x.ParentNodeId,
                        principalTable: "MindMapNodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MindMapNodes_MindMaps_MindMapId",
                        column: x => x.MindMapId,
                        principalTable: "MindMaps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MindMapNodes_FlashCardId",
                table: "MindMapNodes",
                column: "FlashCardId");

            migrationBuilder.CreateIndex(
                name: "IX_MindMapNodes_MindMapId",
                table: "MindMapNodes",
                column: "MindMapId");

            migrationBuilder.CreateIndex(
                name: "IX_MindMapNodes_ParentNodeId",
                table: "MindMapNodes",
                column: "ParentNodeId");

            migrationBuilder.CreateIndex(
                name: "IX_MindMaps_UserId",
                table: "MindMaps",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MindMapNodes");

            migrationBuilder.DropTable(
                name: "MindMaps");
        }
    }
}
