using Microsoft.EntityFrameworkCore.Migrations;

namespace Quanda.Server.Data.Migrations
{
    public partial class Added_ViewsColumn_To_User_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Views",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Views",
                table: "User");
        }
    }
}
