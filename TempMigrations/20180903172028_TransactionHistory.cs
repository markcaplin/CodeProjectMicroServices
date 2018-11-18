using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CodeProject.SalesOrderManagement.Data.EntityFramework.Migrations
{
    public partial class TransactionHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TransactionQueueInboundHistory",
                columns: table => new
                {
                    TransactionQueueInboundHistoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TransactionQueueInboundId = table.Column<int>(nullable: false),
                    SenderTransactionQueueId = table.Column<int>(nullable: false),
                    TransactionCode = table.Column<string>(nullable: true),
                    Payload = table.Column<string>(nullable: true),
                    ExchangeName = table.Column<string>(nullable: true),
                    ProcessedSuccessfully = table.Column<bool>(nullable: false),
                    DuplicateMessage = table.Column<bool>(nullable: false),
                    ErrorMessage = table.Column<string>(nullable: true),
                    DateCreatedInbound = table.Column<DateTime>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionQueueInboundHistory", x => x.TransactionQueueInboundHistoryId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransactionQueueInboundHistory");
        }
    }
}
