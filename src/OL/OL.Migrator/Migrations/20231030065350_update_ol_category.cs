using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OL.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class update_ol_category : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OLCategoryOLCategory",
                schema: "ol");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                schema: "ol",
                table: "ol_categories",
                newName: "Enabled");

            migrationBuilder.AddColumn<long>(
                name: "ParrentId",
                schema: "ol",
                table: "ol_categories",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SystemId",
                schema: "ol",
                table: "ol_categories",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParrentId",
                schema: "ol",
                table: "ol_categories");

            migrationBuilder.DropColumn(
                name: "SystemId",
                schema: "ol",
                table: "ol_categories");

            migrationBuilder.RenameColumn(
                name: "Enabled",
                schema: "ol",
                table: "ol_categories",
                newName: "IsActive");

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
    }
}
