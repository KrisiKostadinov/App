using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ForumApp.Data.Migrations
{
    public partial class AddCategoriesToPost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "UserPosts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "UserCategories",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserPosts_CategoryId",
                table: "UserPosts",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPosts_UserCategories_CategoryId",
                table: "UserPosts",
                column: "CategoryId",
                principalTable: "UserCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPosts_UserCategories_CategoryId",
                table: "UserPosts");

            migrationBuilder.DropIndex(
                name: "IX_UserPosts_CategoryId",
                table: "UserPosts");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "UserPosts");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "UserCategories",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
