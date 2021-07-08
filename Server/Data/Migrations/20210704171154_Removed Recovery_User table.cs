using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Quanda.Server.Data.Migrations
{
    public partial class RemovedRecovery_Usertable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Recovery_User");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Recovery_User",
                columns: table => new
                {
                    IdUser = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExpirationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("RecoveryUser_pk", x => x.IdUser);
                    table.ForeignKey(
                        name: "RecoveryUser_User",
                        column: x => x.IdUser,
                        principalTable: "User",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "Unique_code",
                table: "Recovery_User",
                column: "Code",
                unique: true);
        }
    }
}
