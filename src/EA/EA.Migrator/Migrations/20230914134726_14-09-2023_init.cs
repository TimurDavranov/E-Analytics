using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EA.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class _14092023_init : Migration
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
                name: "ea_translations",
                schema: "ea",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ea_translations", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ea_categories",
                schema: "ea");

            migrationBuilder.DropTable(
                name: "ea_translations",
                schema: "ea");
        }
    }
}
