using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class addFlashCardAndCollection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FlashCardCollection",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlashCardCollection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlashCardCollection_FlashCardCollection_ParentId",
                        column: x => x.ParentId,
                        principalTable: "FlashCardCollection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FlashCard",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Term = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Definition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FlashCardCollectionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlashCard", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlashCard_FlashCardCollection_FlashCardCollectionId",
                        column: x => x.FlashCardCollectionId,
                        principalTable: "FlashCardCollection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FlashCard_FlashCardCollectionId",
                table: "FlashCard",
                column: "FlashCardCollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_FlashCardCollection_ParentId",
                table: "FlashCardCollection",
                column: "ParentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlashCard");

            migrationBuilder.DropTable(
                name: "FlashCardCollection");
        }
    }
}
