using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Quanda.Server.Data.Migrations
{
    public partial class Updatetables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "Question_Notification",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "User_Notification",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "User_Question",
                table: "Question");

            migrationBuilder.DropForeignKey(
                name: "Answer_RatingAnswer",
                table: "Rating_Answer");

            migrationBuilder.DropForeignKey(
                name: "User_RatingAnswer",
                table: "Rating_Answer");

            migrationBuilder.DropForeignKey(
                name: "Service_User",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "Role_UserRole",
                table: "User_Role");

            migrationBuilder.DropForeignKey(
                name: "User_UserRole",
                table: "User_Role");

            migrationBuilder.DropTable(
                name: "Question_Category");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notification",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_Notification_IdUser",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "Points",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "IdQuestion",
                table: "Notification",
                newName: "Type");

            migrationBuilder.AddColumn<bool>(
                name: "IsDarkModeOff",
                table: "User",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Counter",
                table: "Notification",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdEntity",
                table: "Notification",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSeen",
                table: "Notification",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notification",
                table: "Notification",
                column: "IdUser");

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

            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    IdTag = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdMainTag = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Tag_pk", x => x.IdTag);
                    table.ForeignKey(
                        name: "Tag_Tag",
                        column: x => x.IdMainTag,
                        principalTable: "Tag",
                        principalColumn: "IdTag",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Thread",
                columns: table => new
                {
                    IdThread = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdReceiver = table.Column<int>(type: "int", nullable: false),
                    IdSender = table.Column<int>(type: "int", nullable: false),
                    Header = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Thread_pk", x => x.IdThread);
                    table.ForeignKey(
                        name: "Thread_Receiver",
                        column: x => x.IdReceiver,
                        principalTable: "User",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "Thread_Sender",
                        column: x => x.IdSender,
                        principalTable: "User",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Question_Tag",
                columns: table => new
                {
                    IdQuestion = table.Column<int>(type: "int", nullable: false),
                    IdTag = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Question_Tag", x => new { x.IdQuestion, x.IdTag });
                    table.ForeignKey(
                        name: "QuestionTag_Question",
                        column: x => x.IdQuestion,
                        principalTable: "Question",
                        principalColumn: "IdQuestion",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "QuestionTag_Tag",
                        column: x => x.IdTag,
                        principalTable: "Tag",
                        principalColumn: "IdTag",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tag_User",
                columns: table => new
                {
                    IdUser = table.Column<int>(type: "int", nullable: false),
                    IdTag = table.Column<int>(type: "int", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("TagUser_pk", x => new { x.IdUser, x.IdTag });
                    table.ForeignKey(
                        name: "TagUser_Tag",
                        column: x => x.IdTag,
                        principalTable: "Tag",
                        principalColumn: "IdTag",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "TagUser_User",
                        column: x => x.IdUser,
                        principalTable: "User",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    IdMessage = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdSender = table.Column<int>(type: "int", nullable: false),
                    IsSeen = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IdThread = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Text = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Message_pk", x => x.IdMessage);
                    table.ForeignKey(
                        name: "Message_Thread",
                        column: x => x.IdThread,
                        principalTable: "Thread",
                        principalColumn: "IdThread",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "Message_User",
                        column: x => x.IdSender,
                        principalTable: "User",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Report",
                columns: table => new
                {
                    IdReport = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdEntity = table.Column<int>(type: "int", nullable: false),
                    IdIssuer = table.Column<int>(type: "int", nullable: false),
                    IdMessage = table.Column<int>(type: "int", nullable: true),
                    IdAnswer = table.Column<int>(type: "int", nullable: true),
                    IdQuestion = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Report_pk", x => x.IdReport);
                    table.ForeignKey(
                        name: "Report_Answer",
                        column: x => x.IdAnswer,
                        principalTable: "Answer",
                        principalColumn: "IdAnswer",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "Report_Message",
                        column: x => x.IdMessage,
                        principalTable: "Message",
                        principalColumn: "IdMessage",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "Report_Question",
                        column: x => x.IdQuestion,
                        principalTable: "Question",
                        principalColumn: "IdQuestion",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "Report_User",
                        column: x => x.IdIssuer,
                        principalTable: "User",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Message_IdSender",
                table: "Message",
                column: "IdSender");

            migrationBuilder.CreateIndex(
                name: "IX_Message_IdThread",
                table: "Message",
                column: "IdThread");

            migrationBuilder.CreateIndex(
                name: "IX_Question_Tag_IdTag",
                table: "Question_Tag",
                column: "IdTag");

            migrationBuilder.CreateIndex(
                name: "Unique_code",
                table: "Recovery_User",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Report_IdAnswer",
                table: "Report",
                column: "IdAnswer");

            migrationBuilder.CreateIndex(
                name: "IX_Report_IdIssuer",
                table: "Report",
                column: "IdIssuer");

            migrationBuilder.CreateIndex(
                name: "IX_Report_IdMessage",
                table: "Report",
                column: "IdMessage");

            migrationBuilder.CreateIndex(
                name: "IX_Report_IdQuestion",
                table: "Report",
                column: "IdQuestion");

            migrationBuilder.CreateIndex(
                name: "IX_Tag_IdMainTag",
                table: "Tag",
                column: "IdMainTag");

            migrationBuilder.CreateIndex(
                name: "IX_Tag_User_IdTag",
                table: "Tag_User",
                column: "IdTag");

            migrationBuilder.CreateIndex(
                name: "IX_Thread_IdReceiver",
                table: "Thread",
                column: "IdReceiver");

            migrationBuilder.CreateIndex(
                name: "IX_Thread_IdSender",
                table: "Thread",
                column: "IdSender");

            migrationBuilder.AddForeignKey(
                name: "Notification_User",
                table: "Notification",
                column: "IdUser",
                principalTable: "User",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "Question_User",
                table: "Question",
                column: "IdUser",
                principalTable: "User",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "RatingAnswer_Answer",
                table: "Rating_Answer",
                column: "IdAnswer",
                principalTable: "Answer",
                principalColumn: "IdAnswer",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "RatingAnswer_User",
                table: "Rating_Answer",
                column: "IdUser",
                principalTable: "User",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "User_Service",
                table: "User",
                column: "IdService",
                principalTable: "Service",
                principalColumn: "IdService",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "Role_UserRole",
                table: "User_Role",
                column: "IdRole",
                principalTable: "Role",
                principalColumn: "IdRole",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "User_UserRole",
                table: "User_Role",
                column: "IdUser",
                principalTable: "User",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "Notification_User",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "Question_User",
                table: "Question");

            migrationBuilder.DropForeignKey(
                name: "RatingAnswer_Answer",
                table: "Rating_Answer");

            migrationBuilder.DropForeignKey(
                name: "RatingAnswer_User",
                table: "Rating_Answer");

            migrationBuilder.DropForeignKey(
                name: "User_Service",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "Role_UserRole",
                table: "User_Role");

            migrationBuilder.DropForeignKey(
                name: "User_UserRole",
                table: "User_Role");

            migrationBuilder.DropTable(
                name: "Question_Tag");

            migrationBuilder.DropTable(
                name: "Recovery_User");

            migrationBuilder.DropTable(
                name: "Report");

            migrationBuilder.DropTable(
                name: "Tag_User");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropTable(
                name: "Thread");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notification",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "IsDarkModeOff",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Counter",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "IdEntity",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "IsSeen",
                table: "Notification");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Notification",
                newName: "IdQuestion");

            migrationBuilder.AddColumn<int>(
                name: "Points",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notification",
                table: "Notification",
                columns: new[] { "IdQuestion", "IdUser" });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    IdCategory = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdMainCategory = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
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
                name: "IX_Notification_IdUser",
                table: "Notification",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_Category_IdMainCategory",
                table: "Category",
                column: "IdMainCategory");

            migrationBuilder.CreateIndex(
                name: "IX_Question_Category_IdCategory",
                table: "Question_Category",
                column: "IdCategory");

            migrationBuilder.AddForeignKey(
                name: "Question_Notification",
                table: "Notification",
                column: "IdQuestion",
                principalTable: "Question",
                principalColumn: "IdQuestion",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "User_Notification",
                table: "Notification",
                column: "IdUser",
                principalTable: "User",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "User_Question",
                table: "Question",
                column: "IdUser",
                principalTable: "User",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "Answer_RatingAnswer",
                table: "Rating_Answer",
                column: "IdAnswer",
                principalTable: "Answer",
                principalColumn: "IdAnswer",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "User_RatingAnswer",
                table: "Rating_Answer",
                column: "IdUser",
                principalTable: "User",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "Service_User",
                table: "User",
                column: "IdService",
                principalTable: "Service",
                principalColumn: "IdService",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "Role_UserRole",
                table: "User_Role",
                column: "IdRole",
                principalTable: "Role",
                principalColumn: "IdRole",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "User_UserRole",
                table: "User_Role",
                column: "IdUser",
                principalTable: "User",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
