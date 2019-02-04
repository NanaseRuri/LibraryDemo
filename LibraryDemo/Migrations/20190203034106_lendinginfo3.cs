using Microsoft.EntityFrameworkCore.Migrations;

namespace LibraryDemo.Migrations
{
    public partial class lendinginfo3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StuId",
                table: "Books");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StuId",
                table: "Books",
                nullable: true);
        }
    }
}
