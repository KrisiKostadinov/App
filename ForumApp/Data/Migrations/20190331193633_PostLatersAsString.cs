using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ForumApp.Data.Migrations
{
    public partial class PostLatersAsString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostLaters_AspNetUsers_AuthorId",
                table: "PostLaters");

            migrationBuilder.DropForeignKey(
                name: "FK_PostLaters_Posts_PostId",
                table: "PostLaters");

            migrationBuilder.DropIndex(
                name: "IX_PostLaters_AuthorId",
                table: "PostLaters");

            migrationBuilder.DropIndex(
                name: "IX_PostLaters_PostId",
                table: "PostLaters");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "PostLaters",
                newName: "Author");

            migrationBuilder.AlterColumn<string>(
                name: "Author",
                table: "PostLaters",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Author",
                table: "PostLaters",
                newName: "AuthorId");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "PostLaters",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostLaters_AuthorId",
                table: "PostLaters",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_PostLaters_PostId",
                table: "PostLaters",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostLaters_AspNetUsers_AuthorId",
                table: "PostLaters",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PostLaters_Posts_PostId",
                table: "PostLaters",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
