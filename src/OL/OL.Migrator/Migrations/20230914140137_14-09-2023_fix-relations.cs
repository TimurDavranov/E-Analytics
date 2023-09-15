using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OL.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class _14092023_fixrelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ol_categories_ol_categories_OLCategoryId",
                schema: "ol",
                table: "ol_categories");

            migrationBuilder.DropIndex(
                name: "IX_ol_categories_OLCategoryId",
                schema: "ol",
                table: "ol_categories");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                schema: "ol",
                table: "ol_categories");

            migrationBuilder.DropColumn(
                name: "OLCategoryId",
                schema: "ol",
                table: "ol_categories");

            migrationBuilder.DropColumn(
                name: "ParrentId",
                schema: "ol",
                table: "ol_categories");

            migrationBuilder.CreateTable(
                name: "OLCategoryOLCategory",
                schema: "ol",
                columns: table => new
                {
                    ChildsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParrentsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OLCategoryOLCategory", x => new { x.ChildsId, x.ParrentsId });
                    table.ForeignKey(
                        name: "FK_OLCategoryOLCategory_ol_categories_ChildsId",
                        column: x => x.ChildsId,
                        principalSchema: "ol",
                        principalTable: "ol_categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OLCategoryOLCategory_ol_categories_ParrentsId",
                        column: x => x.ParrentsId,
                        principalSchema: "ol",
                        principalTable: "ol_categories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OLCategoryOLCategory_ParrentsId",
                schema: "ol",
                table: "OLCategoryOLCategory",
                column: "ParrentsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OLCategoryOLCategory",
                schema: "ol");

            migrationBuilder.AddColumn<long>(
                name: "CategoryId",
                schema: "ol",
                table: "ol_categories",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<Guid>(
                name: "OLCategoryId",
                schema: "ol",
                table: "ol_categories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ParrentId",
                schema: "ol",
                table: "ol_categories",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ol_categories_OLCategoryId",
                schema: "ol",
                table: "ol_categories",
                column: "OLCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ol_categories_ol_categories_OLCategoryId",
                schema: "ol",
                table: "ol_categories",
                column: "OLCategoryId",
                principalSchema: "ol",
                principalTable: "ol_categories",
                principalColumn: "Id");
        }
    }
}
