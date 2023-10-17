using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EA.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class update_ea_products_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Url",
                schema: "ea",
                table: "ea_system_products",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                schema: "ea",
                table: "ea_system_products");
        }
    }
}
