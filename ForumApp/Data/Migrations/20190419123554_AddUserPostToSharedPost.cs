using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ForumApp.Data.Migrations
{
    public partial class AddUserPostToSharedPost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PostId",
                table: "SharedPosts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserPostId",
                table: "SharedPosts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SharedPosts_PostId",
                table: "SharedPosts",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_SharedPosts_UserPosts_PostId",
                table: "SharedPosts",
                column: "PostId",
                principalTable: "UserPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SharedPosts_UserPosts_PostId",
                table: "SharedPosts");

            migrationBuilder.DropIndex(
                name: "IX_SharedPosts_PostId",
                table: "SharedPosts");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "SharedPosts");

            migrationBuilder.DropColumn(
                name: "UserPostId",
                table: "SharedPosts");
        }
    }
}
