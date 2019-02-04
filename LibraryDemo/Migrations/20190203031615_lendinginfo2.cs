using Microsoft.EntityFrameworkCore.Migrations;

namespace LibraryDemo.Migrations
{
    public partial class lendinginfo2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Books",
                newName: "StuId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StuId",
                table: "Books",
                newName: "UserName");
        }
    }
}
