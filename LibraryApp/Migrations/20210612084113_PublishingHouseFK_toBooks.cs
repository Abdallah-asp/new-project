using Microsoft.EntityFrameworkCore.Migrations;

namespace LibraryApp.Migrations
{
    public partial class PublishingHouseFK_toBooks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Books_Publishing_house_id",
                table: "Books",
                column: "Publishing_house_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_publishingHouses_Publishing_house_id",
                table: "Books",
                column: "Publishing_house_id",
                principalTable: "publishingHouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_publishingHouses_Publishing_house_id",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_Publishing_house_id",
                table: "Books");
        }
    }
}
