using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ForumApp.Data.Migrations
{
    public partial class AddPostIdAndUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SharedUsers_UserPosts_PostId",
                table: "SharedUsers");

            migrationBuilder.RenameColumn(
                name: "User",
                table: "SharedUsers",
                newName: "UserId");

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "SharedUsers",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SharedUsers_UserPosts_PostId",
                table: "SharedUsers",
                column: "PostId",
                principalTable: "UserPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SharedUsers_UserPosts_PostId",
                table: "SharedUsers");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "SharedUsers",
                newName: "User");

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "SharedUsers",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_SharedUsers_UserPosts_PostId",
                table: "SharedUsers",
                column: "PostId",
                principalTable: "UserPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
