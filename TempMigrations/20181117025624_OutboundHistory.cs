using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CodeProject.SalesOrderManagement.Data.EntityFramework.Migrations
{
    public partial class OutboundHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TransactionQueueOutboundHistory",
                columns: table => new
                {
                    TransactionQueueOutboundHistoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TransactionQueueOutboundId = table.Column<int>(nullable: false),
                    TransactionCode = table.Column<string>(nullable: true),
                    Payload = table.Column<string>(nullable: true),
                    ExchangeName = table.Column<string>(nullable: true),
                    SentToExchange = table.Column<bool>(nullable: false),
                    DateOutboundTransactionCreated = table.Column<DateTime>(nullable: false),
                    DateSentToExchange = table.Column<DateTime>(nullable: false),
                    DateToResendToExchange = table.Column<DateTime>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionQueueOutboundHistory", x => x.TransactionQueueOutboundHistoryId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransactionQueueOutboundHistory");
        }
    }
}
