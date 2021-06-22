using Microsoft.EntityFrameworkCore.Migrations;

namespace LibraryApp.Migrations
{
    public partial class AddBookIdToFeedBack : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Book_id",
                table: "FeedBacks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FeedBacks_Book_id",
                table: "FeedBacks",
                column: "Book_id");

            migrationBuilder.AddForeignKey(
                name: "FK_FeedBacks_Books_Book_id",
                table: "FeedBacks",
                column: "Book_id",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeedBacks_Books_Book_id",
                table: "FeedBacks");

            migrationBuilder.DropIndex(
                name: "IX_FeedBacks_Book_id",
                table: "FeedBacks");

            migrationBuilder.DropColumn(
                name: "Book_id",
                table: "FeedBacks");
        }
    }
}
