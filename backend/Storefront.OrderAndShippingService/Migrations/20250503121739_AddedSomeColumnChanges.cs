using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Storefront.OrderAndShippingService.Migrations
{
    /// <inheritdoc />
    public partial class AddedSomeColumnChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VarientId",
                table: "OrderItems",
                newName: "VariantId");

            migrationBuilder.AddColumn<string>(
                name: "BillingAddress_Email",
                table: "Orders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillingAddress_PhoneNumber",
                table: "Orders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress_Email",
                table: "Orders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress_PhoneNumber",
                table: "Orders",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillingAddress_Email",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "BillingAddress_PhoneNumber",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippingAddress_Email",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippingAddress_PhoneNumber",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "VariantId",
                table: "OrderItems",
                newName: "VarientId");
        }
    }
}
