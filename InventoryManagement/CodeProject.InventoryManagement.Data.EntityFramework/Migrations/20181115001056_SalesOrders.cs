using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CodeProject.InventoryManagement.Data.EntityFramework.Migrations
{
    public partial class SalesOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SalesOrderStatuses",
                columns: table => new
                {
                    SalesOrderStatusId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesOrderStatuses", x => x.SalesOrderStatusId);
                });

            migrationBuilder.CreateTable(
                name: "SalesOrders",
                columns: table => new
                {
                    SalesOrderId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MasterSalesOrderId = table.Column<int>(nullable: false),
                    SalesOrderNumber = table.Column<int>(nullable: false),
                    AccountId = table.Column<int>(nullable: false),
                    CustomerName = table.Column<string>(nullable: true),
                    AddressLine1 = table.Column<string>(nullable: true),
                    AddressLine2 = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Region = table.Column<string>(nullable: true),
                    PostalCode = table.Column<string>(nullable: true),
                    OrderTotal = table.Column<double>(nullable: false),
                    SalesOrderStatusId = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesOrders", x => x.SalesOrderId);
                    table.ForeignKey(
                        name: "FK_SalesOrders_SalesOrderStatuses_SalesOrderStatusId",
                        column: x => x.SalesOrderStatusId,
                        principalTable: "SalesOrderStatuses",
                        principalColumn: "SalesOrderStatusId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalesOrderDetails",
                columns: table => new
                {
                    SalesOrderDetailId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccountId = table.Column<int>(nullable: false),
                    MasterSalesOrderDetailId = table.Column<int>(nullable: false),
                    SalesOrderId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    ProductNumber = table.Column<string>(nullable: true),
                    ProductDescription = table.Column<string>(nullable: true),
                    UnitPrice = table.Column<double>(nullable: false),
                    OrderQuantity = table.Column<int>(nullable: false),
                    ShippedQuantity = table.Column<int>(nullable: false),
                    OrderTotal = table.Column<double>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesOrderDetails", x => x.SalesOrderDetailId);
                    table.ForeignKey(
                        name: "FK_SalesOrderDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SalesOrderDetails_SalesOrders_SalesOrderId",
                        column: x => x.SalesOrderId,
                        principalTable: "SalesOrders",
                        principalColumn: "SalesOrderId",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrderDetails_ProductId",
                table: "SalesOrderDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrderDetails_SalesOrderId",
                table: "SalesOrderDetails",
                column: "SalesOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrders_SalesOrderStatusId",
                table: "SalesOrders",
                column: "SalesOrderStatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalesOrderDetails");

            migrationBuilder.DropTable(
                name: "SalesOrders");

            migrationBuilder.DropTable(
                name: "SalesOrderStatuses");
        }
    }
}
