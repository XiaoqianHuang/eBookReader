using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ReadIt.Data.Migrations
{
    public partial class fullinformation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdateTime",
                table: "Documents",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UploadUserName",
                table: "Documents",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommentUserName",
                table: "Comments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastUpdateTime",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "UploadUserName",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "CommentUserName",
                table: "Comments");
        }
    }
}
