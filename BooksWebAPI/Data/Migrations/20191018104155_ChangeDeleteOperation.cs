using Microsoft.EntityFrameworkCore.Migrations;

namespace BooksWebAPI.Data.Migrations
{
    public partial class ChangeDeleteOperation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_XRefUserBooks_AspNetUsers_UserId",
                table: "XRefUserBooks");

            migrationBuilder.AddForeignKey(
                name: "FK_XRefUserBooks_AspNetUsers_UserId",
                table: "XRefUserBooks",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_XRefUserBooks_AspNetUsers_UserId",
                table: "XRefUserBooks");

            migrationBuilder.AddForeignKey(
                name: "FK_XRefUserBooks_AspNetUsers_UserId",
                table: "XRefUserBooks",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
