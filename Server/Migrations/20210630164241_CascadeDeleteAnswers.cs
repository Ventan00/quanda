using Microsoft.EntityFrameworkCore.Migrations;

namespace Quanda.Server.Migrations
{
    public partial class CascadeDeleteAnswers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "Answer_Answer",
                table: "Answer");

            migrationBuilder.AddForeignKey(
                name: "Answer_Answer",
                table: "Answer",
                column: "IdRootAnswer",
                principalTable: "Answer",
                principalColumn: "IdAnswer",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "Answer_Answer",
                table: "Answer");

            migrationBuilder.AddForeignKey(
                name: "Answer_Answer",
                table: "Answer",
                column: "IdRootAnswer",
                principalTable: "Answer",
                principalColumn: "IdAnswer",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
