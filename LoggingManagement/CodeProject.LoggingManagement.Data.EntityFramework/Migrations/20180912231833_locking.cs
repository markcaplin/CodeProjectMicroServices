using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CodeProject.LoggingManagement.Data.EntityFramework.Migrations
{
    public partial class locking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TransactionQueueSemaphores",
                columns: table => new
                {
                    TransactionQueueSemaphoreId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SemaphoreKey = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionQueueSemaphores", x => x.TransactionQueueSemaphoreId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransactionQueueSemaphores_SemaphoreKey",
                table: "TransactionQueueSemaphores",
                column: "SemaphoreKey",
                unique: true,
                filter: "[SemaphoreKey] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransactionQueueSemaphores");
        }
    }
}
