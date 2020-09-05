using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoupnsKE.Data.Migrations
{
    public partial class Core_Model_Addition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Deals",
                columns: table => new
                {
                    DealsID = table.Column<Guid>(nullable: false),
                    DealName = table.Column<string>(nullable: true),
                    DealDescription = table.Column<string>(nullable: true),
                    DealRating = table.Column<float>(nullable: false),
                    Store = table.Column<string>(nullable: true),
                    OldPrice = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    NewPrice = table.Column<decimal>(type: "decimal(18, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deals", x => x.DealsID);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    ProductID = table.Column<Guid>(nullable: false),
                    ProductName = table.Column<string>(nullable: true),
                    ProductCategory = table.Column<string>(nullable: true),
                    StoreID = table.Column<Guid>(nullable: false),
                    SKU = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.ProductID);
                });

            migrationBuilder.CreateTable(
                name: "Store",
                columns: table => new
                {
                    StoreID = table.Column<Guid>(nullable: false),
                    StoreName = table.Column<string>(nullable: true),
                    StoreReflink = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Store", x => x.StoreID);
                });

            migrationBuilder.CreateTable(
                name: "TrackedPrice",
                columns: table => new
                {
                    TrackedPriceID = table.Column<Guid>(nullable: false),
                    UserID = table.Column<Guid>(nullable: false),
                    ProductID = table.Column<Guid>(nullable: false),
                    DesiredPrice = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    LowestPrice = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    StoreWithLowestPrice = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackedPrice", x => x.TrackedPriceID);
                });

            migrationBuilder.CreateTable(
                name: "UserCoupons",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCoupons", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Coupon",
                columns: table => new
                {
                    CouponID = table.Column<Guid>(nullable: false),
                    StoreID = table.Column<Guid>(nullable: false),
                    CouponCategory = table.Column<string>(nullable: true),
                    CouponCode = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ExpiryDate = table.Column<DateTime>(nullable: false),
                    Restrictions = table.Column<string>(nullable: true),
                    UserCouponsID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coupon", x => x.CouponID);
                    table.ForeignKey(
                        name: "FK_Coupon_UserCoupons_UserCouponsID",
                        column: x => x.UserCouponsID,
                        principalTable: "UserCoupons",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Coupon_UserCouponsID",
                table: "Coupon",
                column: "UserCouponsID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Coupon");

            migrationBuilder.DropTable(
                name: "Deals");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Store");

            migrationBuilder.DropTable(
                name: "TrackedPrice");

            migrationBuilder.DropTable(
                name: "UserCoupons");
        }
    }
}
