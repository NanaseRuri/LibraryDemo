using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LibraryDemo.Migrations.LendingInfoDb
{
    public partial class LendingInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BooksDetail",
                columns: table => new
                {
                    ISBN = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Author = table.Column<string>(nullable: false),
                    Press = table.Column<string>(nullable: false),
                    PublishDateTime = table.Column<DateTime>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    SoundCassettes = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BooksDetail", x => x.ISBN);
                });

            migrationBuilder.CreateTable(
                name: "Bookshelves",
                columns: table => new
                {
                    BookshelfId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Sort = table.Column<string>(nullable: false),
                    MinFetchNumber = table.Column<string>(nullable: false),
                    MaxFetchNumber = table.Column<string>(nullable: false),
                    Location = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookshelves", x => x.BookshelfId);
                });

            migrationBuilder.CreateTable(
                name: "RecommendedBooks",
                columns: table => new
                {
                    ISBN = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Author = table.Column<string>(nullable: false),
                    Press = table.Column<string>(nullable: false),
                    PublishDateTime = table.Column<DateTime>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    SoundCassettes = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecommendedBooks", x => x.ISBN);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    NormalizedUserName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    NormalizedEmail = table.Column<string>(nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Degree = table.Column<int>(nullable: false),
                    MaxBooksNumber = table.Column<int>(nullable: false),
                    AppointingBookBarCode = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(maxLength: 14, nullable: true),
                    Fine = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    BarCode = table.Column<string>(nullable: false),
                    ISBN = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    FetchBookNumber = table.Column<string>(nullable: true),
                    BorrowTime = table.Column<DateTime>(nullable: true),
                    MatureTime = table.Column<DateTime>(nullable: true),
                    AppointedLatestTime = table.Column<DateTime>(nullable: true),
                    State = table.Column<int>(nullable: false),
                    KeeperId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.BarCode);
                    table.ForeignKey(
                        name: "FK_Books_Students_KeeperId",
                        column: x => x.KeeperId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BookMiddles",
                columns: table => new
                {
                    BookMiddleId = table.Column<int>(nullable: false),
                    BookId = table.Column<string>(nullable: false),
                    BookshelfId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookMiddles", x => new { x.BookId, x.BookshelfId });
                    table.ForeignKey(
                        name: "FK_BookMiddles_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BarCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookMiddles_Bookshelves_BookshelfId",
                        column: x => x.BookshelfId,
                        principalTable: "Bookshelves",
                        principalColumn: "BookshelfId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookMiddles_BookshelfId",
                table: "BookMiddles",
                column: "BookshelfId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_KeeperId",
                table: "Books",
                column: "KeeperId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookMiddles");

            migrationBuilder.DropTable(
                name: "BooksDetail");

            migrationBuilder.DropTable(
                name: "RecommendedBooks");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Bookshelves");

            migrationBuilder.DropTable(
                name: "Students");
        }
    }
}
