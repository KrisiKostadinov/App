using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ForumApp.Data.Migrations
{
    public partial class SharedPost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SharedPostId",
                table: "UserComments",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SharedPosts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CategoryId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    SahredDate = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SharedPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SharedPosts_UserCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "UserCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SharedPosts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserComments_SharedPostId",
                table: "UserComments",
                column: "SharedPostId");

            migrationBuilder.CreateIndex(
                name: "IX_SharedPosts_CategoryId",
                table: "SharedPosts",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SharedPosts_UserId",
                table: "SharedPosts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserComments_SharedPosts_SharedPostId",
                table: "UserComments",
                column: "SharedPostId",
                principalTable: "SharedPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserComments_SharedPosts_SharedPostId",
                table: "UserComments");

            migrationBuilder.DropTable(
                name: "SharedPosts");

            migrationBuilder.DropIndex(
                name: "IX_UserComments_SharedPostId",
                table: "UserComments");

            migrationBuilder.DropColumn(
                name: "SharedPostId",
                table: "UserComments");
        }
    }
}
