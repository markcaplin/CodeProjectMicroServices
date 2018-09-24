using Microsoft.EntityFrameworkCore.Migrations;

namespace CodeProject.PurchaseOrderManagement.Data.EntityFramework.Migrations
{
    public partial class master : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductMasterId",
                table: "Products",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductMasterId",
                table: "Products");
        }
    }
}
