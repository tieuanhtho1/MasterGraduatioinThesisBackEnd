using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddMindMapAndNodesTables : Migration
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
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    FlashCardCollectionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MindMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MindMaps_FlashCardCollections_FlashCardCollectionId",
                        column: x => x.FlashCardCollectionId,
                        principalTable: "FlashCardCollections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    PositionX = table.Column<double>(type: "float", nullable: false),
                    PositionY = table.Column<double>(type: "float", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HideChildren = table.Column<bool>(type: "bit", nullable: false),
                    ParentNodeId = table.Column<int>(type: "int", nullable: true),
                    MindMapId = table.Column<int>(type: "int", nullable: false),
                    FlashCardId = table.Column<int>(type: "int", nullable: false)
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
                name: "IX_MindMaps_FlashCardCollectionId",
                table: "MindMaps",
                column: "FlashCardCollectionId");

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
