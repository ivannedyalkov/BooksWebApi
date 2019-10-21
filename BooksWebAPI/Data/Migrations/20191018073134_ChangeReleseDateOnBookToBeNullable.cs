using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BooksWebAPI.Data.Migrations
{
    public partial class ChangeReleseDateOnBookToBeNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ReleaseDate",
                table: "Books",
                type: "SmallDateTime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "SmallDateTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ReleaseDate",
                table: "Books",
                type: "SmallDateTime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "SmallDateTime",
                oldNullable: true);
        }
    }
}
