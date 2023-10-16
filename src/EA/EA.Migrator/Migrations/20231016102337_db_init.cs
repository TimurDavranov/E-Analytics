using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EA.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class db_init : Migration
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
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "ea_category_translations",
                schema: "ea",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ea_category_translations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ea_category_translations_ea_categories_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "ea",
                        principalTable: "ea_categories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ea_products",
                schema: "ea",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ea_products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ea_products_ea_categories_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "ea",
                        principalTable: "ea_categories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ea_system_products",
                schema: "ea",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SystemName = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ea_system_products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ea_system_products_ea_products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "ea",
                        principalTable: "ea_products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryCategory_ParentId",
                schema: "ea",
                table: "CategoryCategory",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ea_category_translations_CategoryId",
                schema: "ea",
                table: "ea_category_translations",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ea_products_CategoryId",
                schema: "ea",
                table: "ea_products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ea_system_products_ProductId",
                schema: "ea",
                table: "ea_system_products",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryCategory",
                schema: "ea");

            migrationBuilder.DropTable(
                name: "ea_category_translations",
                schema: "ea");

            migrationBuilder.DropTable(
                name: "ea_system_products",
                schema: "ea");

            migrationBuilder.DropTable(
                name: "ea_products",
                schema: "ea");

            migrationBuilder.DropTable(
                name: "ea_categories",
                schema: "ea");
        }
    }
}
