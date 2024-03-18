using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OL.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class addtool_productcategory_id : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SystemCategoryId",
                schema: "ol",
                table: "ol_product",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SystemCategoryId",
                schema: "ol",
                table: "ol_product");
        }
    }
}
