using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EA.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class db_init_27092023 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ea");

            migrationBuilder.CreateTable(
                name: "ea_categories",
                schema: "ea",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ea_categories", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "ea_translations",
                schema: "ea",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ea_translations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ea_translations_ea_categories_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "ea",
                        principalTable: "ea_categories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryCategory_ParentId",
                schema: "ea",
                table: "CategoryCategory",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ea_translations_CategoryId",
                schema: "ea",
                table: "ea_translations",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryCategory",
                schema: "ea");

            migrationBuilder.DropTable(
                name: "ea_translations",
                schema: "ea");

            migrationBuilder.DropTable(
                name: "ea_categories",
                schema: "ea");
        }
    }
}
