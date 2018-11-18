using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CodeProject.SalesOrderManagement.Data.EntityFramework.Migrations
{
    public partial class SalesOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "SalesOrderStatuses",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateTable(
                name: "SalesOrderNumberSequences",
                columns: table => new
                {
                    SalesOrderNumberSequenceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccountId = table.Column<int>(nullable: false),
                    SalesOrderNumber = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesOrderNumberSequences", x => x.SalesOrderNumberSequenceId);
                });

            migrationBuilder.InsertData(
                table: "SalesOrderStatuses",
                columns: new[] { "SalesOrderStatusId", "Description" },
                values: new object[] { 1, "Open" });

            migrationBuilder.InsertData(
                table: "SalesOrderStatuses",
                columns: new[] { "SalesOrderStatusId", "Description" },
                values: new object[] { 2, "Submitted" });

            migrationBuilder.InsertData(
                table: "SalesOrderStatuses",
                columns: new[] { "SalesOrderStatusId", "Description" },
                values: new object[] { 3, "Completed" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalesOrderNumberSequences");

            migrationBuilder.DeleteData(
                table: "SalesOrderStatuses",
                keyColumn: "SalesOrderStatusId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "SalesOrderStatuses",
                keyColumn: "SalesOrderStatusId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "SalesOrderStatuses",
                keyColumn: "SalesOrderStatusId",
                keyValue: 3);

            migrationBuilder.AlterColumn<int>(
                name: "Description",
                table: "SalesOrderStatuses",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
