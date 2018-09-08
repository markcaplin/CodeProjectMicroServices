using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CodeProject.LoggingManagement.Data.EntityFramework.Migrations
{
    public partial class AcknowledgementsQueue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AcknowledgementsQueue",
                columns: table => new
                {
                    AcknowledgementsQueueId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SenderTransactionQueueId = table.Column<int>(nullable: false),
                    TransactionCode = table.Column<string>(nullable: true),
                    ExchangeName = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcknowledgementsQueue", x => x.AcknowledgementsQueueId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AcknowledgementsQueue");
        }
    }
}
