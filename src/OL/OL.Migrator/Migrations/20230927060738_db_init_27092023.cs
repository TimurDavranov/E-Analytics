using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OL.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class db_init_27092023 : Migration
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
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ol_categories", x => x.Id);
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
                name: "IX_ol_translations_OLCategoryId",
                schema: "ol",
                table: "ol_translations",
                column: "OLCategoryId");

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
                name: "ol_translations",
                schema: "ol");

            migrationBuilder.DropTable(
                name: "OLCategoryOLCategory",
                schema: "ol");

            migrationBuilder.DropTable(
                name: "ol_categories",
                schema: "ol");
        }
    }
}
