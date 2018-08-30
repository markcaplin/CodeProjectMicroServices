using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CodeProject.InventoryManagement.Data.EntityFramework.Migrations
{
    public partial class InitialDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    AccountId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProductNumber = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    BinLocation = table.Column<string>(nullable: true),
                    UnitPrice = table.Column<double>(nullable: false),
                    AverageCost = table.Column<double>(nullable: false),
                    OnHandQuantity = table.Column<int>(nullable: false),
                    OnOrderQuantity = table.Column<int>(nullable: false),
                    CommittedQuantity = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                });

            migrationBuilder.CreateTable(
                name: "TransactionQueue",
                columns: table => new
                {
                    TransactionQueueId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TransactionCode = table.Column<string>(nullable: true),
                    Payload = table.Column<string>(nullable: true),
                    SentToExchange = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateSentToExchange = table.Column<DateTime>(nullable: false),
                    DateToResendToExchange = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionQueue", x => x.TransactionQueueId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductNumber",
                table: "Products",
                column: "ProductNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "TransactionQueue");
        }
    }
}
