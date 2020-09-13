using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoupnsKE.Data.Migrations
{
    public partial class ProductModelModification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StoreID",
                table: "Product");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Product",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProductDescription",
                table: "Product",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StoreName",
                table: "Product",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ProductDescription",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "StoreName",
                table: "Product");

            migrationBuilder.AddColumn<Guid>(
                name: "StoreID",
                table: "Product",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
