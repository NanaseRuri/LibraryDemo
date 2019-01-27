using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LibraryDemo.Migrations
{
    public partial class addIma : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "ImageMimeType",
                table: "Books");

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "BooksDetail",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageMimeType",
                table: "BooksDetail",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "BooksDetail");

            migrationBuilder.DropColumn(
                name: "ImageMimeType",
                table: "BooksDetail");

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "Books",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageMimeType",
                table: "Books",
                nullable: true);
        }
    }
}
