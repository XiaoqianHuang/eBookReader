using Microsoft.EntityFrameworkCore.Migrations;

namespace ReadIt.Data.Migrations
{
    public partial class seperatemodels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_CommentUserId1",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_AspNetUsers_UploadUserId1",
                table: "Documents");

            migrationBuilder.RenameColumn(
                name: "UploadUserId1",
                table: "Documents",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Documents_UploadUserId1",
                table: "Documents",
                newName: "IX_Documents_UserId");

            migrationBuilder.RenameColumn(
                name: "CommentUserId1",
                table: "Comments",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_CommentUserId1",
                table: "Comments",
                newName: "IX_Comments_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_UserId",
                table: "Comments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_AspNetUsers_UserId",
                table: "Documents",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_UserId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_AspNetUsers_UserId",
                table: "Documents");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Documents",
                newName: "UploadUserId1");

            migrationBuilder.RenameIndex(
                name: "IX_Documents_UserId",
                table: "Documents",
                newName: "IX_Documents_UploadUserId1");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Comments",
                newName: "CommentUserId1");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                newName: "IX_Comments_CommentUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_CommentUserId1",
                table: "Comments",
                column: "CommentUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_AspNetUsers_UploadUserId1",
                table: "Documents",
                column: "UploadUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
