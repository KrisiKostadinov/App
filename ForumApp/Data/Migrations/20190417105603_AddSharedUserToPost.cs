using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ForumApp.Data.Migrations
{
    public partial class AddSharedUserToPost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PostId",
                table: "SharedUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SharedUsers_PostId",
                table: "SharedUsers",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_SharedUsers_UserPosts_PostId",
                table: "SharedUsers",
                column: "PostId",
                principalTable: "UserPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SharedUsers_UserPosts_PostId",
                table: "SharedUsers");

            migrationBuilder.DropIndex(
                name: "IX_SharedUsers_PostId",
                table: "SharedUsers");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "SharedUsers");
        }
    }
}
