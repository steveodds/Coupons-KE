using Microsoft.EntityFrameworkCore.Migrations;

namespace CoupnsKE.Data.Migrations
{
    public partial class ProductModelModificationStoreLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StoreLink",
                table: "Product",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StoreLink",
                table: "Product");
        }
    }
}
