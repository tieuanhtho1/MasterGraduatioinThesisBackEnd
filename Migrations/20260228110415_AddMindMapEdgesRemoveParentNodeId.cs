using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddMindMapEdgesRemoveParentNodeId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MindMapNodes_MindMapNodes_ParentNodeId",
                table: "MindMapNodes");

            migrationBuilder.CreateTable(
                name: "MindMapEdges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SourceNodeId = table.Column<int>(type: "int", nullable: false),
                    TargetNodeId = table.Column<int>(type: "int", nullable: false),
                    SourceHandle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TargetHandle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MindMapId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MindMapEdges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MindMapEdges_MindMapNodes_SourceNodeId",
                        column: x => x.SourceNodeId,
                        principalTable: "MindMapNodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MindMapEdges_MindMapNodes_TargetNodeId",
                        column: x => x.TargetNodeId,
                        principalTable: "MindMapNodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MindMapEdges_MindMaps_MindMapId",
                        column: x => x.MindMapId,
                        principalTable: "MindMaps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Migrate existing parent-child relationships to edges
            migrationBuilder.Sql(@"
                INSERT INTO MindMapEdges (SourceNodeId, TargetNodeId, SourceHandle, TargetHandle, MindMapId)
                SELECT ParentNodeId, Id, 'bottom', 'top', MindMapId
                FROM MindMapNodes
                WHERE ParentNodeId IS NOT NULL;
            ");

            migrationBuilder.DropIndex(
                name: "IX_MindMapNodes_ParentNodeId",
                table: "MindMapNodes");

            migrationBuilder.DropColumn(
                name: "ParentNodeId",
                table: "MindMapNodes");

            migrationBuilder.CreateIndex(
                name: "IX_MindMapEdges_MindMapId",
                table: "MindMapEdges",
                column: "MindMapId");

            migrationBuilder.CreateIndex(
                name: "IX_MindMapEdges_SourceNodeId",
                table: "MindMapEdges",
                column: "SourceNodeId");

            migrationBuilder.CreateIndex(
                name: "IX_MindMapEdges_TargetNodeId",
                table: "MindMapEdges",
                column: "TargetNodeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MindMapEdges");

            migrationBuilder.AddColumn<int>(
                name: "ParentNodeId",
                table: "MindMapNodes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MindMapNodes_ParentNodeId",
                table: "MindMapNodes",
                column: "ParentNodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_MindMapNodes_MindMapNodes_ParentNodeId",
                table: "MindMapNodes",
                column: "ParentNodeId",
                principalTable: "MindMapNodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
