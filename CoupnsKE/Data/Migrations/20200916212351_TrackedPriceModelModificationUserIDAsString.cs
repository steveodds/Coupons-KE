using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoupnsKE.Data.Migrations
{
    public partial class TrackedPriceModelModificationUserIDAsString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "TrackedPrice",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "UserID",
                table: "TrackedPrice",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
