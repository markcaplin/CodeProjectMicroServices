using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CodeProject.InventoryManagement.Data.EntityFramework.Migrations
{
    public partial class Add_PurchaseOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PurchaseOrderStatuses",
                columns: table => new
                {
                    PurchaseOrderStatusId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrderStatuses", x => x.PurchaseOrderStatusId);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrders",
                columns: table => new
                {
                    PurchaseOrderId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MasterPurchaseOrderId = table.Column<int>(nullable: false),
                    PurchaseOrderNumber = table.Column<int>(nullable: false),
                    AccountId = table.Column<int>(nullable: false),
                    SupplierName = table.Column<string>(nullable: true),
                    AddressLine1 = table.Column<string>(nullable: true),
                    AddressLine2 = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Region = table.Column<string>(nullable: true),
                    PostalCode = table.Column<string>(nullable: true),
                    OrderTotal = table.Column<double>(nullable: false),
                    PurchaseOrderStatusId = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrders", x => x.PurchaseOrderId);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_PurchaseOrderStatuses_PurchaseOrderStatusId",
                        column: x => x.PurchaseOrderStatusId,
                        principalTable: "PurchaseOrderStatuses",
                        principalColumn: "PurchaseOrderStatusId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrderDetails",
                columns: table => new
                {
                    PurchaseOrderDetailId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccountId = table.Column<int>(nullable: false),
                    MasterPurchaseOrderDetailId = table.Column<int>(nullable: false),
                    PurchaseOrderId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    ProductNumber = table.Column<string>(nullable: true),
                    ProductDescription = table.Column<string>(nullable: true),
                    UnitPrice = table.Column<double>(nullable: false),
                    OrderQuantity = table.Column<int>(nullable: false),
                    ReceivedQuantity = table.Column<int>(nullable: false),
                    OrderTotal = table.Column<double>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrderDetails", x => x.PurchaseOrderDetailId);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderDetails_PurchaseOrders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalTable: "PurchaseOrders",
                        principalColumn: "PurchaseOrderId",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderDetails_ProductId",
                table: "PurchaseOrderDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderDetails_PurchaseOrderId",
                table: "PurchaseOrderDetails",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_PurchaseOrderStatusId",
                table: "PurchaseOrders",
                column: "PurchaseOrderStatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchaseOrderDetails");

            migrationBuilder.DropTable(
                name: "PurchaseOrders");

            migrationBuilder.DropTable(
                name: "PurchaseOrderStatuses");
        }
    }
}
