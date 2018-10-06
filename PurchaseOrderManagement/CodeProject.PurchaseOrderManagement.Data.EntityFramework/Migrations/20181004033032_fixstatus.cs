using Microsoft.EntityFrameworkCore.Migrations;

namespace CodeProject.PurchaseOrderManagement.Data.EntityFramework.Migrations
{
    public partial class fixstatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "PurchaseOrderStatuses",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.InsertData(
                table: "PurchaseOrderStatuses",
                columns: new[] { "PurchaseOrderStatusId", "Description" },
                values: new object[] { 1, "Open" });

            migrationBuilder.InsertData(
                table: "PurchaseOrderStatuses",
                columns: new[] { "PurchaseOrderStatusId", "Description" },
                values: new object[] { 2, "Submitted" });

            migrationBuilder.InsertData(
                table: "PurchaseOrderStatuses",
                columns: new[] { "PurchaseOrderStatusId", "Description" },
                values: new object[] { 3, "Completed" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PurchaseOrderStatuses",
                keyColumn: "PurchaseOrderStatusId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PurchaseOrderStatuses",
                keyColumn: "PurchaseOrderStatusId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PurchaseOrderStatuses",
                keyColumn: "PurchaseOrderStatusId",
                keyValue: 3);

            migrationBuilder.AlterColumn<int>(
                name: "Description",
                table: "PurchaseOrderStatuses",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
