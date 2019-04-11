using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ForumApp.Data.Migrations
{
    public partial class part : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserComments_UserPosts_PostId",
                table: "UserComments");

            migrationBuilder.DropForeignKey(
                name: "FK_UserComments_AspNetUsers_UserId",
                table: "UserComments");

            migrationBuilder.DropIndex(
                name: "IX_UserComments_UserId",
                table: "UserComments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserComments");

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "UserComments",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_UserComments_UserPosts_PostId",
                table: "UserComments",
                column: "PostId",
                principalTable: "UserPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserComments_UserPosts_PostId",
                table: "UserComments");

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "UserComments",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "UserComments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserComments_UserId",
                table: "UserComments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserComments_UserPosts_PostId",
                table: "UserComments",
                column: "PostId",
                principalTable: "UserPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserComments_AspNetUsers_UserId",
                table: "UserComments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
