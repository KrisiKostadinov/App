using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ForumApp.Data.Migrations
{
    public partial class addcoments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_UserPosts_PostId",
                table: "Comment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comment",
                table: "Comment");

            migrationBuilder.RenameTable(
                name: "Comment",
                newName: "UserComments");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_PostId",
                table: "UserComments",
                newName: "IX_UserComments_PostId");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "UserComments",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserComments",
                table: "UserComments",
                column: "Id");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserComments_UserPosts_PostId",
                table: "UserComments");

            migrationBuilder.DropForeignKey(
                name: "FK_UserComments_AspNetUsers_UserId",
                table: "UserComments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserComments",
                table: "UserComments");

            migrationBuilder.DropIndex(
                name: "IX_UserComments_UserId",
                table: "UserComments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserComments");

            migrationBuilder.RenameTable(
                name: "UserComments",
                newName: "Comment");

            migrationBuilder.RenameIndex(
                name: "IX_UserComments_PostId",
                table: "Comment",
                newName: "IX_Comment_PostId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comment",
                table: "Comment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_UserPosts_PostId",
                table: "Comment",
                column: "PostId",
                principalTable: "UserPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
