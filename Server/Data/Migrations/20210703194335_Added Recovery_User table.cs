using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Quanda.Server.Data.Migrations
{
    public partial class AddedRecovery_Usertable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "User",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

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
                name: "Unique_email",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Unique_nickname",
                table: "User",
                column: "Nickname",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Unique_code",
                table: "Temp_User",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Unique_code",
                table: "Recovery_User",
                column: "Code",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Recovery_User");

            migrationBuilder.DropIndex(
                name: "Unique_email",
                table: "User");

            migrationBuilder.DropIndex(
                name: "Unique_nickname",
                table: "User");

            migrationBuilder.DropIndex(
                name: "Unique_code",
                table: "Temp_User");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "User",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
