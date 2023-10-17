using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EA.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class update_ea_products_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ea_products_ea_categories_CategoryId",
                schema: "ea",
                table: "ea_products");

            migrationBuilder.DropForeignKey(
                name: "FK_ea_system_products_ea_products_ProductId",
                schema: "ea",
                table: "ea_system_products");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                schema: "ea",
                table: "ea_system_products",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                schema: "ea",
                table: "ea_products",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ea_products_ea_categories_CategoryId",
                schema: "ea",
                table: "ea_products",
                column: "CategoryId",
                principalSchema: "ea",
                principalTable: "ea_categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ea_system_products_ea_products_ProductId",
                schema: "ea",
                table: "ea_system_products",
                column: "ProductId",
                principalSchema: "ea",
                principalTable: "ea_products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ea_products_ea_categories_CategoryId",
                schema: "ea",
                table: "ea_products");

            migrationBuilder.DropForeignKey(
                name: "FK_ea_system_products_ea_products_ProductId",
                schema: "ea",
                table: "ea_system_products");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                schema: "ea",
                table: "ea_system_products",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                schema: "ea",
                table: "ea_products",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_ea_products_ea_categories_CategoryId",
                schema: "ea",
                table: "ea_products",
                column: "CategoryId",
                principalSchema: "ea",
                principalTable: "ea_categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ea_system_products_ea_products_ProductId",
                schema: "ea",
                table: "ea_system_products",
                column: "ProductId",
                principalSchema: "ea",
                principalTable: "ea_products",
                principalColumn: "Id");
        }
    }
}
