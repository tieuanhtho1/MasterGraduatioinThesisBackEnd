using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class updateFlashCardCollectionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlashCard_FlashCardCollection_FlashCardCollectionId",
                table: "FlashCard");

            migrationBuilder.DropForeignKey(
                name: "FK_FlashCardCollection_FlashCardCollection_ParentId",
                table: "FlashCardCollection");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FlashCardCollection",
                table: "FlashCardCollection");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FlashCard",
                table: "FlashCard");

            migrationBuilder.RenameTable(
                name: "FlashCardCollection",
                newName: "FlashCardCollections");

            migrationBuilder.RenameTable(
                name: "FlashCard",
                newName: "FlashCards");

            migrationBuilder.RenameIndex(
                name: "IX_FlashCardCollection_ParentId",
                table: "FlashCardCollections",
                newName: "IX_FlashCardCollections_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_FlashCard_FlashCardCollectionId",
                table: "FlashCards",
                newName: "IX_FlashCards_FlashCardCollectionId");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "FlashCardCollections",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FlashCardCollections",
                table: "FlashCardCollections",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FlashCards",
                table: "FlashCards",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_FlashCardCollections_UserId",
                table: "FlashCardCollections",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FlashCardCollections_FlashCardCollections_ParentId",
                table: "FlashCardCollections",
                column: "ParentId",
                principalTable: "FlashCardCollections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FlashCardCollections_Users_UserId",
                table: "FlashCardCollections",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FlashCards_FlashCardCollections_FlashCardCollectionId",
                table: "FlashCards",
                column: "FlashCardCollectionId",
                principalTable: "FlashCardCollections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlashCardCollections_FlashCardCollections_ParentId",
                table: "FlashCardCollections");

            migrationBuilder.DropForeignKey(
                name: "FK_FlashCardCollections_Users_UserId",
                table: "FlashCardCollections");

            migrationBuilder.DropForeignKey(
                name: "FK_FlashCards_FlashCardCollections_FlashCardCollectionId",
                table: "FlashCards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FlashCards",
                table: "FlashCards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FlashCardCollections",
                table: "FlashCardCollections");

            migrationBuilder.DropIndex(
                name: "IX_FlashCardCollections_UserId",
                table: "FlashCardCollections");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "FlashCardCollections");

            migrationBuilder.RenameTable(
                name: "FlashCards",
                newName: "FlashCard");

            migrationBuilder.RenameTable(
                name: "FlashCardCollections",
                newName: "FlashCardCollection");

            migrationBuilder.RenameIndex(
                name: "IX_FlashCards_FlashCardCollectionId",
                table: "FlashCard",
                newName: "IX_FlashCard_FlashCardCollectionId");

            migrationBuilder.RenameIndex(
                name: "IX_FlashCardCollections_ParentId",
                table: "FlashCardCollection",
                newName: "IX_FlashCardCollection_ParentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FlashCard",
                table: "FlashCard",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FlashCardCollection",
                table: "FlashCardCollection",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FlashCard_FlashCardCollection_FlashCardCollectionId",
                table: "FlashCard",
                column: "FlashCardCollectionId",
                principalTable: "FlashCardCollection",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FlashCardCollection_FlashCardCollection_ParentId",
                table: "FlashCardCollection",
                column: "ParentId",
                principalTable: "FlashCardCollection",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
