using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Migrations
{
    public partial class UpdateLaptopModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Laptops_Stores_StoreId",
                table: "Laptops");

            migrationBuilder.AlterColumn<Guid>(
                name: "StoreId",
                table: "Laptops",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Laptops",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Laptops_Stores_StoreId",
                table: "Laptops",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Laptops_Stores_StoreId",
                table: "Laptops");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Laptops");

            migrationBuilder.AlterColumn<Guid>(
                name: "StoreId",
                table: "Laptops",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Laptops_Stores_StoreId",
                table: "Laptops",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id");
        }
    }
}
