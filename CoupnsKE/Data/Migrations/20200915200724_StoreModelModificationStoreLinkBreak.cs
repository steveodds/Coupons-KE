using Microsoft.EntityFrameworkCore.Migrations;

namespace CoupnsKE.Data.Migrations
{
    public partial class StoreModelModificationStoreLinkBreak : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StoreReflink",
                table: "Store");

            migrationBuilder.AddColumn<string>(
                name: "StoreReflinkEnd",
                table: "Store",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StoreReflinkStart",
                table: "Store",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StoreReflinkEnd",
                table: "Store");

            migrationBuilder.DropColumn(
                name: "StoreReflinkStart",
                table: "Store");

            migrationBuilder.AddColumn<string>(
                name: "StoreReflink",
                table: "Store",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
