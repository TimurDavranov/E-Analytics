using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OL.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class change_ol_product : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InstalmentMaxMouth",
                schema: "ol",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "InstalmentMonthlyRepayment",
                schema: "ol",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                schema: "ol",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InstalmentMaxMouth",
                schema: "ol",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "InstalmentMonthlyRepayment",
                schema: "ol",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Price",
                schema: "ol",
                table: "Products");
        }
    }
}
