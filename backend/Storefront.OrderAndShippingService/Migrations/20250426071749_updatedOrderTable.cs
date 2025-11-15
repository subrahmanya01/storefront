using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Storefront.OrderAndShippingService.Migrations
{
    /// <inheritdoc />
    public partial class updatedOrderTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "OrderItems");

            migrationBuilder.AlterColumn<string>(
                name: "ProductId",
                table: "OrderItems",
                type: "text",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<string>(
                name: "VarientId",
                table: "OrderItems",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VarientId",
                table: "OrderItems");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "OrderItems",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "OrderItems",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
