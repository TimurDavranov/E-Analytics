using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OL.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class add_ol_product_price_history : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ol_translations_Products_OLProductId",
                schema: "ol",
                table: "ol_translations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                schema: "ol",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Price",
                schema: "ol",
                table: "Products");

            migrationBuilder.RenameTable(
                name: "Products",
                schema: "ol",
                newName: "ol_product",
                newSchema: "ol");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ol_product",
                schema: "ol",
                table: "ol_product",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ol_product_price_history",
                schema: "ol",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OLProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ol_product_price_history", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ol_product_price_history_ol_product_OLProductId",
                        column: x => x.OLProductId,
                        principalSchema: "ol",
                        principalTable: "ol_product",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ol_product_price_history_OLProductId",
                schema: "ol",
                table: "ol_product_price_history",
                column: "OLProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ol_translations_ol_product_OLProductId",
                schema: "ol",
                table: "ol_translations",
                column: "OLProductId",
                principalSchema: "ol",
                principalTable: "ol_product",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ol_translations_ol_product_OLProductId",
                schema: "ol",
                table: "ol_translations");

            migrationBuilder.DropTable(
                name: "ol_product_price_history",
                schema: "ol");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ol_product",
                schema: "ol",
                table: "ol_product");

            migrationBuilder.RenameTable(
                name: "ol_product",
                schema: "ol",
                newName: "Products",
                newSchema: "ol");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                schema: "ol",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                schema: "ol",
                table: "Products",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ol_translations_Products_OLProductId",
                schema: "ol",
                table: "ol_translations",
                column: "OLProductId",
                principalSchema: "ol",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
