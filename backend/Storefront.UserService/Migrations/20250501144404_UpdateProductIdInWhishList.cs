using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Storefront.UserService.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductIdInWhishList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WhishList_Users_UserId",
                table: "WhishList");

            migrationBuilder.DropForeignKey(
                name: "FK_WhishList_Users_UserId1",
                table: "WhishList");

            migrationBuilder.DropForeignKey(
                name: "FK_WhishListItem_WhishList_WhishListId",
                table: "WhishListItem");

            migrationBuilder.DropForeignKey(
                name: "FK_WhishListItem_WhishList_WhishListId1",
                table: "WhishListItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WhishListItem",
                table: "WhishListItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WhishList",
                table: "WhishList");

            migrationBuilder.RenameTable(
                name: "WhishListItem",
                newName: "WhishListItems");

            migrationBuilder.RenameTable(
                name: "WhishList",
                newName: "WhishLists");

            migrationBuilder.RenameIndex(
                name: "IX_WhishListItem_WhishListId1",
                table: "WhishListItems",
                newName: "IX_WhishListItems_WhishListId1");

            migrationBuilder.RenameIndex(
                name: "IX_WhishListItem_WhishListId",
                table: "WhishListItems",
                newName: "IX_WhishListItems_WhishListId");

            migrationBuilder.RenameIndex(
                name: "IX_WhishList_UserId1",
                table: "WhishLists",
                newName: "IX_WhishLists_UserId1");

            migrationBuilder.RenameIndex(
                name: "IX_WhishList_UserId",
                table: "WhishLists",
                newName: "IX_WhishLists_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "ProductId",
                table: "WhishListItems",
                type: "text",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WhishListItems",
                table: "WhishListItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WhishLists",
                table: "WhishLists",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WhishListItems_WhishLists_WhishListId",
                table: "WhishListItems",
                column: "WhishListId",
                principalTable: "WhishLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WhishListItems_WhishLists_WhishListId1",
                table: "WhishListItems",
                column: "WhishListId1",
                principalTable: "WhishLists",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WhishLists_Users_UserId",
                table: "WhishLists",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WhishLists_Users_UserId1",
                table: "WhishLists",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WhishListItems_WhishLists_WhishListId",
                table: "WhishListItems");

            migrationBuilder.DropForeignKey(
                name: "FK_WhishListItems_WhishLists_WhishListId1",
                table: "WhishListItems");

            migrationBuilder.DropForeignKey(
                name: "FK_WhishLists_Users_UserId",
                table: "WhishLists");

            migrationBuilder.DropForeignKey(
                name: "FK_WhishLists_Users_UserId1",
                table: "WhishLists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WhishLists",
                table: "WhishLists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WhishListItems",
                table: "WhishListItems");

            migrationBuilder.RenameTable(
                name: "WhishLists",
                newName: "WhishList");

            migrationBuilder.RenameTable(
                name: "WhishListItems",
                newName: "WhishListItem");

            migrationBuilder.RenameIndex(
                name: "IX_WhishLists_UserId1",
                table: "WhishList",
                newName: "IX_WhishList_UserId1");

            migrationBuilder.RenameIndex(
                name: "IX_WhishLists_UserId",
                table: "WhishList",
                newName: "IX_WhishList_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_WhishListItems_WhishListId1",
                table: "WhishListItem",
                newName: "IX_WhishListItem_WhishListId1");

            migrationBuilder.RenameIndex(
                name: "IX_WhishListItems_WhishListId",
                table: "WhishListItem",
                newName: "IX_WhishListItem_WhishListId");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "WhishListItem",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_WhishList",
                table: "WhishList",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WhishListItem",
                table: "WhishListItem",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WhishList_Users_UserId",
                table: "WhishList",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WhishList_Users_UserId1",
                table: "WhishList",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WhishListItem_WhishList_WhishListId",
                table: "WhishListItem",
                column: "WhishListId",
                principalTable: "WhishList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WhishListItem_WhishList_WhishListId1",
                table: "WhishListItem",
                column: "WhishListId1",
                principalTable: "WhishList",
                principalColumn: "Id");
        }
    }
}
