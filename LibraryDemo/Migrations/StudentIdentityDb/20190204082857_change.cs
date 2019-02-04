using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LibraryDemo.Migrations.StudentIdentityDb
{
    public partial class change : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Book");

            migrationBuilder.DropTable(
                name: "Bookshelf");

            migrationBuilder.DropColumn(
                name: "AppointingBookBarCode",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Fine",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppointingBookBarCode",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Fine",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "Bookshelf",
                columns: table => new
                {
                    BookshelfId = table.Column<int>(nullable: false),
                    Location = table.Column<string>(nullable: false),
                    MaxFetchNumber = table.Column<string>(nullable: false),
                    MinFetchNumber = table.Column<string>(nullable: false),
                    Sort = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookshelf", x => x.BookshelfId);
                });

            migrationBuilder.CreateTable(
                name: "Book",
                columns: table => new
                {
                    BarCode = table.Column<string>(nullable: false),
                    AppointedLatestTime = table.Column<DateTime>(nullable: true),
                    BookshelfId = table.Column<int>(nullable: false),
                    BorrowTime = table.Column<DateTime>(nullable: true),
                    FetchBookNumber = table.Column<string>(nullable: true),
                    ISBN = table.Column<string>(nullable: true),
                    KeeperId = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    MatureTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Sort = table.Column<string>(nullable: true),
                    State = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Book", x => x.BarCode);
                    table.ForeignKey(
                        name: "FK_Book_Bookshelf_BookshelfId",
                        column: x => x.BookshelfId,
                        principalTable: "Bookshelf",
                        principalColumn: "BookshelfId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Book_AspNetUsers_KeeperId",
                        column: x => x.KeeperId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Book_BookshelfId",
                table: "Book",
                column: "BookshelfId");

            migrationBuilder.CreateIndex(
                name: "IX_Book_KeeperId",
                table: "Book",
                column: "KeeperId");
        }
    }
}
