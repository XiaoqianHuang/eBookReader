using Microsoft.EntityFrameworkCore.Migrations;

namespace ReadIt.Data.Migrations
{
    public partial class ModifyModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_UserId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentAuthor_Authors_AuthorId",
                table: "DocumentAuthor");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentAuthor_Documents_DocumentId",
                table: "DocumentAuthor");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentCategory_Categories_CategoryId",
                table: "DocumentCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentCategory_Documents_DocumentId",
                table: "DocumentCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_AspNetUsers_UserId",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_UserId",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Comments_UserId",
                table: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DocumentCategory",
                table: "DocumentCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DocumentAuthor",
                table: "DocumentAuthor");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Comments");

            migrationBuilder.RenameTable(
                name: "DocumentCategory",
                newName: "DocumentCategories");

            migrationBuilder.RenameTable(
                name: "DocumentAuthor",
                newName: "DocumentAuthors");

            migrationBuilder.RenameIndex(
                name: "IX_DocumentCategory_CategoryId",
                table: "DocumentCategories",
                newName: "IX_DocumentCategories_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_DocumentAuthor_AuthorId",
                table: "DocumentAuthors",
                newName: "IX_DocumentAuthors_AuthorId");

            migrationBuilder.AlterColumn<string>(
                name: "UploadUserId",
                table: "Documents",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Comments",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "CommentUserId",
                table: "Comments",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DocumentCategories",
                table: "DocumentCategories",
                columns: new[] { "DocumentId", "CategoryId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_DocumentAuthors",
                table: "DocumentAuthors",
                columns: new[] { "DocumentId", "AuthorId" });

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentAuthors_Authors_AuthorId",
                table: "DocumentAuthors",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "AuthorId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentAuthors_Documents_DocumentId",
                table: "DocumentAuthors",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "DocumentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentCategories_Categories_CategoryId",
                table: "DocumentCategories",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentCategories_Documents_DocumentId",
                table: "DocumentCategories",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "DocumentId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentAuthors_Authors_AuthorId",
                table: "DocumentAuthors");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentAuthors_Documents_DocumentId",
                table: "DocumentAuthors");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentCategories_Categories_CategoryId",
                table: "DocumentCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentCategories_Documents_DocumentId",
                table: "DocumentCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DocumentCategories",
                table: "DocumentCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DocumentAuthors",
                table: "DocumentAuthors");

            migrationBuilder.RenameTable(
                name: "DocumentCategories",
                newName: "DocumentCategory");

            migrationBuilder.RenameTable(
                name: "DocumentAuthors",
                newName: "DocumentAuthor");

            migrationBuilder.RenameIndex(
                name: "IX_DocumentCategories_CategoryId",
                table: "DocumentCategory",
                newName: "IX_DocumentCategory_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_DocumentAuthors_AuthorId",
                table: "DocumentAuthor",
                newName: "IX_DocumentAuthor_AuthorId");

            migrationBuilder.AlterColumn<int>(
                name: "UploadUserId",
                table: "Documents",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Documents",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Content",
                table: "Comments",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CommentUserId",
                table: "Comments",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Comments",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DocumentCategory",
                table: "DocumentCategory",
                columns: new[] { "DocumentId", "CategoryId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_DocumentAuthor",
                table: "DocumentAuthor",
                columns: new[] { "DocumentId", "AuthorId" });

            migrationBuilder.CreateIndex(
                name: "IX_Documents_UserId",
                table: "Documents",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_UserId",
                table: "Comments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentAuthor_Authors_AuthorId",
                table: "DocumentAuthor",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "AuthorId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentAuthor_Documents_DocumentId",
                table: "DocumentAuthor",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "DocumentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentCategory_Categories_CategoryId",
                table: "DocumentCategory",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentCategory_Documents_DocumentId",
                table: "DocumentCategory",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "DocumentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_AspNetUsers_UserId",
                table: "Documents",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
