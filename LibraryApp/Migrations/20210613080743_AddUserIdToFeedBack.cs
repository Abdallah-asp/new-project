using Microsoft.EntityFrameworkCore.Migrations;

namespace LibraryApp.Migrations
{
    public partial class AddUserIdToFeedBack : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "User_id",
                table: "FeedBacks",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FeedBacks_User_id",
                table: "FeedBacks",
                column: "User_id");

            migrationBuilder.AddForeignKey(
                name: "FK_FeedBacks_AspNetUsers_User_id",
                table: "FeedBacks",
                column: "User_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeedBacks_AspNetUsers_User_id",
                table: "FeedBacks");

            migrationBuilder.DropIndex(
                name: "IX_FeedBacks_User_id",
                table: "FeedBacks");

            migrationBuilder.DropColumn(
                name: "User_id",
                table: "FeedBacks");
        }
    }
}
