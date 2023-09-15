using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OL.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class _14092023_init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ol");

            migrationBuilder.CreateTable(
                name: "ol_categories",
                schema: "ol",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ParrentId = table.Column<long>(type: "bigint", nullable: true),
                    OLCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ol_categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ol_categories_ol_categories_OLCategoryId",
                        column: x => x.OLCategoryId,
                        principalSchema: "ol",
                        principalTable: "ol_categories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ol_translations",
                schema: "ol",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OLCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ol_translations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ol_translations_ol_categories_OLCategoryId",
                        column: x => x.OLCategoryId,
                        principalSchema: "ol",
                        principalTable: "ol_categories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ol_categories_OLCategoryId",
                schema: "ol",
                table: "ol_categories",
                column: "OLCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ol_translations_OLCategoryId",
                schema: "ol",
                table: "ol_translations",
                column: "OLCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ol_translations",
                schema: "ol");

            migrationBuilder.DropTable(
                name: "ol_categories",
                schema: "ol");
        }
    }
}
