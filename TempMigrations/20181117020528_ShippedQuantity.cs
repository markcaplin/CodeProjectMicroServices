using Microsoft.EntityFrameworkCore.Migrations;

namespace CodeProject.SalesOrderManagement.Data.EntityFramework.Migrations
{
    public partial class ShippedQuantity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShippedQuantity",
                table: "SalesOrderDetails",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShippedQuantity",
                table: "SalesOrderDetails");
        }
    }
}
