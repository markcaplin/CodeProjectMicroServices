using Microsoft.EntityFrameworkCore.Migrations;

namespace CodeProject.LoggingManagement.Data.EntityFramework.Migrations
{
    public partial class ackqueue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AcknowledgementQueue",
                table: "AcknowledgementsQueue",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcknowledgementQueue",
                table: "AcknowledgementsQueue");
        }
    }
}
