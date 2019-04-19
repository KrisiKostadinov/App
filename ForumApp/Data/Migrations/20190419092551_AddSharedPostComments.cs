using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ForumApp.Data.Migrations
{
    public partial class AddSharedPostComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserComments_SharedPosts_SharedPostId",
                table: "UserComments");

            migrationBuilder.DropIndex(
                name: "IX_UserComments_SharedPostId",
                table: "UserComments");

            migrationBuilder.DropColumn(
                name: "SharedPostId",
                table: "UserComments");

            migrationBuilder.CreateTable(
                name: "SharedPostComments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: false),
                    SharedPostId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SharedPostComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SharedPostComments_SharedPosts_SharedPostId",
                        column: x => x.SharedPostId,
                        principalTable: "SharedPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SharedPostComments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SharedPostComments_SharedPostId",
                table: "SharedPostComments",
                column: "SharedPostId");

            migrationBuilder.CreateIndex(
                name: "IX_SharedPostComments_UserId",
                table: "SharedPostComments",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SharedPostComments");

            migrationBuilder.AddColumn<int>(
                name: "SharedPostId",
                table: "UserComments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserComments_SharedPostId",
                table: "UserComments",
                column: "SharedPostId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserComments_SharedPosts_SharedPostId",
                table: "UserComments",
                column: "SharedPostId",
                principalTable: "SharedPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
