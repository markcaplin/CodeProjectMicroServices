using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CodeProject.LoggingManagement.Data.EntityFramework.Migrations
{
    public partial class changes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MessageQueueLogging");

            migrationBuilder.CreateTable(
                name: "MessagesReceived",
                columns: table => new
                {
                    MessagesReceivedId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SenderTransactionQueueId = table.Column<int>(nullable: false),
                    QueueName = table.Column<string>(nullable: true),
                    TransactionCode = table.Column<string>(nullable: true),
                    ExchangeName = table.Column<string>(nullable: true),
                    Payload = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessagesReceived", x => x.MessagesReceivedId);
                });

            migrationBuilder.CreateTable(
                name: "MessagesSent",
                columns: table => new
                {
                    MessagesSentId = table.Column<int>(nullable: false)
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
                    table.PrimaryKey("PK_MessagesSent", x => x.MessagesSentId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MessagesReceived");

            migrationBuilder.DropTable(
                name: "MessagesSent");

            migrationBuilder.CreateTable(
                name: "MessageQueueLogging",
                columns: table => new
                {
                    MessageQueueLoggingId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AcknowledgementsReceived = table.Column<int>(nullable: false),
                    AcknowledgementsRequired = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    ExchangeName = table.Column<string>(nullable: true),
                    Payload = table.Column<string>(nullable: true),
                    SenderTransactionQueueId = table.Column<int>(nullable: false),
                    TransactionCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageQueueLogging", x => x.MessageQueueLoggingId);
                });
        }
    }
}
