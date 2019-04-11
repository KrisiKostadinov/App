using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ForumApp.Data.Migrations
{
    public partial class add : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserComments_AspNetUsers_UserId",
                table: "UserComments");

            migrationBuilder.DropIndex(
                name: "IX_UserComments_UserId",
                table: "UserComments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserComments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "UserComments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserComments_UserId",
                table: "UserComments",
                column: "UserId");

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
