using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EA.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class _14092023_fix_relations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CategoryId",
                schema: "ea",
                table: "ea_translations",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CategoryCategory",
                schema: "ea",
                columns: table => new
                {
                    ChildsId = table.Column<long>(type: "bigint", nullable: false),
                    ParentId = table.Column<long>(type: "bigint", nullable: false)
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
                name: "IX_ea_translations_CategoryId",
                schema: "ea",
                table: "ea_translations",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryCategory_ParentId",
                schema: "ea",
                table: "CategoryCategory",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ea_translations_ea_categories_CategoryId",
                schema: "ea",
                table: "ea_translations",
                column: "CategoryId",
                principalSchema: "ea",
                principalTable: "ea_categories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ea_translations_ea_categories_CategoryId",
                schema: "ea",
                table: "ea_translations");

            migrationBuilder.DropTable(
                name: "CategoryCategory",
                schema: "ea");

            migrationBuilder.DropIndex(
                name: "IX_ea_translations_CategoryId",
                schema: "ea",
                table: "ea_translations");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                schema: "ea",
                table: "ea_translations");
        }
    }
}
