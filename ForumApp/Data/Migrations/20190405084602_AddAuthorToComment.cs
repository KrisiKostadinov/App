using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ForumApp.Data.Migrations
{
    public partial class AddAuthorToComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "UserComments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserComments_AuthorId",
                table: "UserComments",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserComments_AspNetUsers_AuthorId",
                table: "UserComments",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserComments_AspNetUsers_AuthorId",
                table: "UserComments");

            migrationBuilder.DropIndex(
                name: "IX_UserComments_AuthorId",
                table: "UserComments");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "UserComments");
        }
    }
}
