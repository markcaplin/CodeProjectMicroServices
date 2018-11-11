using Microsoft.EntityFrameworkCore.Migrations;

namespace CodeProject.PurchaseOrderManagement.Data.EntityFramework.Migrations
{
    public partial class Add_ReceivedQuantity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReceiviedQuantity",
                table: "PurchaseOrderDetails",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiviedQuantity",
                table: "PurchaseOrderDetails");
        }
    }
}
