using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ForumApp.Data.Migrations
{
    public partial class addcoment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserComments_UserPosts_PostId",
                table: "UserComments");

            migrationBuilder.DropIndex(
                name: "IX_UserComments_PostId",
                table: "UserComments");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "UserComments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PostId",
                table: "UserComments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserComments_PostId",
                table: "UserComments",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserComments_UserPosts_PostId",
                table: "UserComments",
                column: "PostId",
                principalTable: "UserPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
