using Microsoft.EntityFrameworkCore.Migrations;

namespace Quanda.Server.Data.Migrations
{
    public partial class AddedRating_Answertable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Answer");

            migrationBuilder.CreateTable(
                name: "Rating_Answer",
                columns: table => new
                {
                    IdAnswer = table.Column<int>(type: "int", nullable: false),
                    IdUser = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("RatingAnswer_pk", x => new { x.IdAnswer, x.IdUser });
                    table.ForeignKey(
                        name: "Answer_RatingAnswer",
                        column: x => x.IdAnswer,
                        principalTable: "Answer",
                        principalColumn: "IdAnswer",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "User_RatingAnswer",
                        column: x => x.IdUser,
                        principalTable: "User",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Rating_Answer_IdUser",
                table: "Rating_Answer",
                column: "IdUser");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rating_Answer");

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Answer",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
