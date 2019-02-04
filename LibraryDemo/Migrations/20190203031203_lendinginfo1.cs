using Microsoft.EntityFrameworkCore.Migrations;

namespace LibraryDemo.Migrations
{
    public partial class lendinginfo1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "KeeperUserName",
                table: "Books",
                newName: "UserName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Books",
                newName: "KeeperUserName");
        }
    }
}
