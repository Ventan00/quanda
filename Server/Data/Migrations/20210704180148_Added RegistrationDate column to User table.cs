using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Quanda.Server.Data.Migrations
{
    public partial class AddedRegistrationDatecolumntoUsertable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RegistrationDate",
                table: "User",
                type: "datetime(6)",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegistrationDate",
                table: "User");
        }
    }
}
