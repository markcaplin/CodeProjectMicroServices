using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CodeProject.LoggingManagement.Data.EntityFramework.Migrations
{
    public partial class InitialDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MessageQueueLogging",
                columns: table => new
                {
                    MessageQueueLoggingId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SenderTransactionQueueId = table.Column<int>(nullable: false),
                    TransactionCode = table.Column<string>(nullable: true),
                    ExchangeName = table.Column<string>(nullable: true),
                    Payload = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    AcknowledgementsRequired = table.Column<int>(nullable: false),
                    AcknowledgementsReceived = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageQueueLogging", x => x.MessageQueueLoggingId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MessageQueueLogging");
        }
    }
}
