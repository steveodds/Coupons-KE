using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoupnsKE.Data.Migrations
{
    public partial class UserCouponsSaveCouponID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coupon_UserCoupons_UserCouponsID",
                table: "Coupon");

            migrationBuilder.DropIndex(
                name: "IX_Coupon_UserCouponsID",
                table: "Coupon");

            migrationBuilder.DropColumn(
                name: "UserCouponsID",
                table: "Coupon");

            migrationBuilder.AddColumn<Guid>(
                name: "CouponID",
                table: "UserCoupons",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CouponID",
                table: "UserCoupons");

            migrationBuilder.AddColumn<int>(
                name: "UserCouponsID",
                table: "Coupon",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Coupon_UserCouponsID",
                table: "Coupon",
                column: "UserCouponsID");

            migrationBuilder.AddForeignKey(
                name: "FK_Coupon_UserCoupons_UserCouponsID",
                table: "Coupon",
                column: "UserCouponsID",
                principalTable: "UserCoupons",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
