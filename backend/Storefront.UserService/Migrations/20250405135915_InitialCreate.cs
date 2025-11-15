using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Storefront.UserService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WhishList",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId1 = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WhishList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WhishList_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WhishList_Users_UserId1",
                        column: x => x.UserId1,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WhishListItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WhishListId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    WhishListId1 = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WhishListItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WhishListItem_WhishList_WhishListId",
                        column: x => x.WhishListId,
                        principalTable: "WhishList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WhishListItem_WhishList_WhishListId1",
                        column: x => x.WhishListId1,
                        principalTable: "WhishList",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_WhishList_UserId",
                table: "WhishList",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WhishList_UserId1",
                table: "WhishList",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_WhishListItem_WhishListId",
                table: "WhishListItem",
                column: "WhishListId");

            migrationBuilder.CreateIndex(
                name: "IX_WhishListItem_WhishListId1",
                table: "WhishListItem",
                column: "WhishListId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WhishListItem");

            migrationBuilder.DropTable(
                name: "WhishList");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
