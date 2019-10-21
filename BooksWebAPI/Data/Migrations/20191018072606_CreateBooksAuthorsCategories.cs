using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BooksWebAPI.Data.Migrations
{
    public partial class CreateBooksAuthorsCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Author",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    ImageUrl = table.Column<string>(maxLength: 1024, nullable: true),
                    Website = table.Column<string>(maxLength: 1024, nullable: true),
                    Description = table.Column<string>(maxLength: 512, nullable: true),
                    Date = table.Column<DateTime>(type: "SmallDateTime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Author", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Description = table.Column<string>(maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Description = table.Column<string>(maxLength: 512, nullable: true),
                    ImageUrl = table.Column<string>(maxLength: 1024, nullable: true),
                    AuthorId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Books_Author_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Author",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "XRefBookCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BookId = table.Column<Guid>(nullable: false),
                    CategoryId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XRefBookCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_XRefBookCategories_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_XRefBookCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "XRefUserBooks",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    BookId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XRefUserBooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_XRefUserBooks_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_XRefUserBooks_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_AuthorId",
                table: "Books",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_XRefBookCategories_BookId",
                table: "XRefBookCategories",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_XRefBookCategories_CategoryId",
                table: "XRefBookCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_XRefUserBooks_BookId",
                table: "XRefUserBooks",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_XRefUserBooks_UserId",
                table: "XRefUserBooks",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "XRefBookCategories");

            migrationBuilder.DropTable(
                name: "XRefUserBooks");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Author");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");
        }
    }
}
