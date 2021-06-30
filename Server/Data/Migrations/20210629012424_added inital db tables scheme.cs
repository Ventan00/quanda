using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Quanda.Server.Migrations
{
    public partial class addedinitaldbtablesscheme : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    IdCategory = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdMainCategory = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Category_pk", x => x.IdCategory);
                    table.ForeignKey(
                        name: "Category_Category",
                        column: x => x.IdMainCategory,
                        principalTable: "Category",
                        principalColumn: "IdCategory",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    IdRole = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Role_pk", x => x.IdRole);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Service",
                columns: table => new
                {
                    IdService = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Connection = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Service_pk", x => x.IdService);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    IdUser = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nickname = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FirstName = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastName = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HashedPassword = table.Column<string>(type: "varchar(84)", maxLength: 84, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Points = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    PhoneNumber = table.Column<string>(type: "varchar(9)", maxLength: 9, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Bio = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Avatar = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RefreshToken = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RefreshTokenExpirationDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IdService = table.Column<int>(type: "int", nullable: true),
                    ServiceToken = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("User_pk", x => x.IdUser);
                    table.ForeignKey(
                        name: "Service_User",
                        column: x => x.IdService,
                        principalTable: "Service",
                        principalColumn: "IdService",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Question",
                columns: table => new
                {
                    IdQuestion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Header = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "text", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PublishDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Views = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsFinished = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    IsModified = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    ToCheck = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    IdUser = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Question_pk", x => x.IdQuestion);
                    table.ForeignKey(
                        name: "User_Question",
                        column: x => x.IdUser,
                        principalTable: "User",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "User_Role",
                columns: table => new
                {
                    IdRole = table.Column<int>(type: "int", nullable: false),
                    IdUser = table.Column<int>(type: "int", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("UserRole_pk", x => new { x.IdRole, x.IdUser });
                    table.ForeignKey(
                        name: "Role_UserRole",
                        column: x => x.IdRole,
                        principalTable: "Role",
                        principalColumn: "IdRole",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "User_UserRole",
                        column: x => x.IdUser,
                        principalTable: "User",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Answer",
                columns: table => new
                {
                    IdAnswer = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Text = table.Column<string>(type: "text", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Rating = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsModified = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    IdQuestion = table.Column<int>(type: "int", nullable: false),
                    IdUser = table.Column<int>(type: "int", nullable: false),
                    IdRootAnswer = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Answer_pk", x => x.IdAnswer);
                    table.ForeignKey(
                        name: "Answer_Answer",
                        column: x => x.IdRootAnswer,
                        principalTable: "Answer",
                        principalColumn: "IdAnswer",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "Answer_Question",
                        column: x => x.IdQuestion,
                        principalTable: "Question",
                        principalColumn: "IdQuestion",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "Answer_User",
                        column: x => x.IdUser,
                        principalTable: "User",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    IdUser = table.Column<int>(type: "int", nullable: false),
                    IdQuestion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => new { x.IdQuestion, x.IdUser });
                    table.ForeignKey(
                        name: "Question_Notification",
                        column: x => x.IdQuestion,
                        principalTable: "Question",
                        principalColumn: "IdQuestion",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "User_Notification",
                        column: x => x.IdUser,
                        principalTable: "User",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Question_Category",
                columns: table => new
                {
                    IdQuestion = table.Column<int>(type: "int", nullable: false),
                    IdCategory = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Question_Category", x => new { x.IdQuestion, x.IdCategory });
                    table.ForeignKey(
                        name: "Category_QuestionCategory",
                        column: x => x.IdCategory,
                        principalTable: "Category",
                        principalColumn: "IdCategory",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "Question_QuestionCategory",
                        column: x => x.IdQuestion,
                        principalTable: "Question",
                        principalColumn: "IdQuestion",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Answer_IdQuestion",
                table: "Answer",
                column: "IdQuestion");

            migrationBuilder.CreateIndex(
                name: "IX_Answer_IdRootAnswer",
                table: "Answer",
                column: "IdRootAnswer");

            migrationBuilder.CreateIndex(
                name: "IX_Answer_IdUser",
                table: "Answer",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_Category_IdMainCategory",
                table: "Category",
                column: "IdMainCategory");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_IdUser",
                table: "Notification",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_Question_IdUser",
                table: "Question",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_Question_Category_IdCategory",
                table: "Question_Category",
                column: "IdCategory");

            migrationBuilder.CreateIndex(
                name: "IX_User_IdService",
                table: "User",
                column: "IdService");

            migrationBuilder.CreateIndex(
                name: "IX_User_Role_IdUser",
                table: "User_Role",
                column: "IdUser");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Answer");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "Question_Category");

            migrationBuilder.DropTable(
                name: "User_Role");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Question");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Service");
        }
    }
}
