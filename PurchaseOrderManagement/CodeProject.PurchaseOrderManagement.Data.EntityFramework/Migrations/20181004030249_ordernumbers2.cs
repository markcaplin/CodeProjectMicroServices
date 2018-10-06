using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CodeProject.PurchaseOrderManagement.Data.EntityFramework.Migrations
{
    public partial class ordernumbers2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "PurchaseOrderNumberSequences",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateUpdated",
                table: "PurchaseOrderNumberSequences",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "PurchaseOrderNumberSequences");

            migrationBuilder.DropColumn(
                name: "DateUpdated",
                table: "PurchaseOrderNumberSequences");
        }
    }
}
