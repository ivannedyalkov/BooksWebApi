using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BooksWebAPI.Data.Migrations
{
    public partial class RemoveColumnFromAuthorAndAddToBookModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Author");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReleaseDate",
                table: "Books",
                type: "SmallDateTime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReleaseDate",
                table: "Books");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Author",
                type: "SmallDateTime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
