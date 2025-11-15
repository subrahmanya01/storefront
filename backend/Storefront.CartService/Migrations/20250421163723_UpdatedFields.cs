using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Storefront.CartService.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER TABLE ""Carts""
                ALTER COLUMN ""UserId"" TYPE uuid
                USING ""UserId""::uuid;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Carts",
                type: "text",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");
        }
    }
}
