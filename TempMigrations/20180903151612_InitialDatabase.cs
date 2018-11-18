using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CodeProject.SalesOrderManagement.Data.EntityFramework.Migrations
{
    public partial class InitialDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccountId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    AddressLine1 = table.Column<string>(nullable: true),
                    AddressLine2 = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Region = table.Column<string>(nullable: true),
                    PostalCode = table.Column<string>(nullable: true),
                    AmountOrdered = table.Column<double>(nullable: false),
                    AmountShipped = table.Column<double>(nullable: false),
                    CreditLimit = table.Column<double>(nullable: false),
                    DateShipped = table.Column<DateTime>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    AccountId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProductMasterId = table.Column<int>(nullable: false),
                    ProductNumber = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    UnitPrice = table.Column<double>(nullable: false),
                    OnHandQuantity = table.Column<int>(nullable: false),
                    CommittedQuantity = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                });

            migrationBuilder.CreateTable(
                name: "SalesOrderStatuses",
                columns: table => new
                {
                    SalesOrderStatusId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesOrderStatuses", x => x.SalesOrderStatusId);
                });

            migrationBuilder.CreateTable(
                name: "TransactionQueueInbound",
                columns: table => new
                {
                    TransactionQueueInboundId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SenderTransactionQueueId = table.Column<int>(nullable: false),
                    TransactionCode = table.Column<string>(nullable: true),
                    Payload = table.Column<string>(nullable: true),
                    ExchangeName = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionQueueInbound", x => x.TransactionQueueInboundId);
                });

            migrationBuilder.CreateTable(
                name: "TransactionQueueOutbound",
                columns: table => new
                {
                    TransactionQueueOutboundId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TransactionCode = table.Column<string>(nullable: true),
                    Payload = table.Column<string>(nullable: true),
                    ExchangeName = table.Column<string>(nullable: true),
                    SentToExchange = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateSentToExchange = table.Column<DateTime>(nullable: false),
                    DateToResendToExchange = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionQueueOutbound", x => x.TransactionQueueOutboundId);
                });

            migrationBuilder.CreateTable(
                name: "SalesOrders",
                columns: table => new
                {
                    SalesOrderId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SalesOrderNumber = table.Column<int>(nullable: false),
                    AccountId = table.Column<int>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    ShipToAddressLine1 = table.Column<string>(nullable: true),
                    ShipToAddressLine2 = table.Column<string>(nullable: true),
                    ShipToCity = table.Column<string>(nullable: true),
                    ShipToRegion = table.Column<string>(nullable: true),
                    ShipToPostalCode = table.Column<string>(nullable: true),
                    OrderTotal = table.Column<double>(nullable: false),
                    SalesOrderStatusId = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesOrders", x => x.SalesOrderId);
                    table.ForeignKey(
                        name: "FK_SalesOrders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
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
                    SalesOrderId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    UnitPrice = table.Column<double>(nullable: false),
                    OrderQuantity = table.Column<int>(nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductNumber",
                table: "Products",
                column: "ProductNumber");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrderDetails_ProductId",
                table: "SalesOrderDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrderDetails_SalesOrderId",
                table: "SalesOrderDetails",
                column: "SalesOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrders_CustomerId",
                table: "SalesOrders",
                column: "CustomerId");

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
                name: "TransactionQueueInbound");

            migrationBuilder.DropTable(
                name: "TransactionQueueOutbound");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "SalesOrders");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "SalesOrderStatuses");
        }
    }
}
