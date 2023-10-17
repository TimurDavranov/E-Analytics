using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EA.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class remove_ea_products : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ea_system_products",
                schema: "ea");

            migrationBuilder.DropTable(
                name: "ea_products",
                schema: "ea");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SystemName = table.Column<int>(type: "int", nullable: false)
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
    }
}
