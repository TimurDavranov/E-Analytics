using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EA.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class CategoryRelation1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryCategory",
                schema: "ea");

            migrationBuilder.CreateTable(
                name: "ea_categories_relations",
                schema: "ea",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ea_categories_relations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ea_categories_relations_ea_categories_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "ea",
                        principalTable: "ea_categories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ea_categories_relations_ea_categories_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "ea",
                        principalTable: "ea_categories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ea_categories_relations_CategoryId",
                schema: "ea",
                table: "ea_categories_relations",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ea_categories_relations_ParentId",
                schema: "ea",
                table: "ea_categories_relations",
                column: "ParentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ea_categories_relations",
                schema: "ea");

            migrationBuilder.CreateTable(
                name: "CategoryCategory",
                schema: "ea",
                columns: table => new
                {
                    ChildsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryCategory", x => new { x.ChildsId, x.ParentId });
                    table.ForeignKey(
                        name: "FK_CategoryCategory_ea_categories_ChildsId",
                        column: x => x.ChildsId,
                        principalSchema: "ea",
                        principalTable: "ea_categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryCategory_ea_categories_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "ea",
                        principalTable: "ea_categories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryCategory_ParentId",
                schema: "ea",
                table: "CategoryCategory",
                column: "ParentId");
        }
    }
}
