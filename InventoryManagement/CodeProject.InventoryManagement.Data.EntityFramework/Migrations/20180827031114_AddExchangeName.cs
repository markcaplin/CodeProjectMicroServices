using Microsoft.EntityFrameworkCore.Migrations;

namespace CodeProject.InventoryManagement.Data.EntityFramework.Migrations
{
    public partial class AddExchangeName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExchangeName",
                table: "TransactionQueue",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExchangeName",
                table: "TransactionQueue");
        }
    }
}
