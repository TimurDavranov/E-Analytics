using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OL.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class add_ol_product : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LanguageCode",
                schema: "ol",
                table: "ol_translations",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AddColumn<Guid>(
                name: "OLProductId",
                schema: "ol",
                table: "ol_translations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "ol",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SystemId = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ol_translations_OLProductId",
                schema: "ol",
                table: "ol_translations",
                column: "OLProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ol_translations_Products_OLProductId",
                schema: "ol",
                table: "ol_translations",
                column: "OLProductId",
                principalSchema: "ol",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ol_translations_Products_OLProductId",
                schema: "ol",
                table: "ol_translations");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "ol");

            migrationBuilder.DropIndex(
                name: "IX_ol_translations_OLProductId",
                schema: "ol",
                table: "ol_translations");

            migrationBuilder.DropColumn(
                name: "OLProductId",
                schema: "ol",
                table: "ol_translations");

            migrationBuilder.AlterColumn<string>(
                name: "LanguageCode",
                schema: "ol",
                table: "ol_translations",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
