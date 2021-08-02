﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Quanda.Server.Data;

namespace Quanda.Server.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.8");

            modelBuilder.Entity("Quanda.Shared.Models.Answer", b =>
                {
                    b.Property<int>("IdAnswer")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("IdQuestion")
                        .HasColumnType("int");

                    b.Property<int?>("IdRootAnswer")
                        .HasColumnType("int");

                    b.Property<int>("IdUser")
                        .HasColumnType("int");

                    b.Property<bool>("IsModified")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false);

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("IdAnswer")
                        .HasName("Answer_pk");

                    b.HasIndex("IdQuestion");

                    b.HasIndex("IdRootAnswer");

                    b.HasIndex("IdUser");

                    b.ToTable("Answer");
                });

            modelBuilder.Entity("Quanda.Shared.Models.Message", b =>
                {
                    b.Property<int>("IdMessage")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("IdSender")
                        .HasColumnType("int");

                    b.Property<int>("IdThread")
                        .HasColumnType("int");

                    b.Property<bool>("IsSeen")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("IdMessage")
                        .HasName("Message_pk");

                    b.HasIndex("IdSender");

                    b.HasIndex("IdThread");

                    b.ToTable("Message");
                });

            modelBuilder.Entity("Quanda.Shared.Models.Notification", b =>
                {
                    b.Property<int>("IdUser")
                        .HasColumnType("int");

                    b.Property<int>("Counter")
                        .HasColumnType("int");

                    b.Property<int?>("IdEntity")
                        .HasColumnType("int");

                    b.Property<bool>("IsSeen")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false);

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("IdUser");

                    b.ToTable("Notification");
                });

            modelBuilder.Entity("Quanda.Shared.Models.Question", b =>
                {
                    b.Property<int>("IdQuestion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Header")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<int>("IdUser")
                        .HasColumnType("int");

                    b.Property<bool>("IsFinished")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false);

                    b.Property<bool>("IsModified")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false);

                    b.Property<DateTime>("PublishDate")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("ToCheck")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false);

                    b.Property<int>("Views")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.HasKey("IdQuestion")
                        .HasName("Question_pk");

                    b.HasIndex("IdUser");

                    b.ToTable("Question");
                });

            modelBuilder.Entity("Quanda.Shared.Models.QuestionTag", b =>
                {
                    b.Property<int>("IdQuestion")
                        .HasColumnType("int");

                    b.Property<int>("IdTag")
                        .HasColumnType("int");

                    b.HasKey("IdQuestion", "IdTag");

                    b.HasIndex("IdTag");

                    b.ToTable("Question_Tag");
                });

            modelBuilder.Entity("Quanda.Shared.Models.RatingAnswer", b =>
                {
                    b.Property<int>("IdAnswer")
                        .HasColumnType("int");

                    b.Property<int>("IdUser")
                        .HasColumnType("int");

                    b.Property<bool>("Value")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("IdAnswer", "IdUser")
                        .HasName("RatingAnswer_pk");

                    b.HasIndex("IdUser");

                    b.ToTable("Rating_Answer");
                });

            modelBuilder.Entity("Quanda.Shared.Models.Report", b =>
                {
                    b.Property<int>("IdReport")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("IdAnswer")
                        .HasColumnType("int");

                    b.Property<int>("IdIssuer")
                        .HasColumnType("int");

                    b.Property<int?>("IdMessage")
                        .HasColumnType("int");

                    b.Property<int?>("IdQuestion")
                        .HasColumnType("int");

                    b.HasKey("IdReport")
                        .HasName("Report_pk");

                    b.HasIndex("IdAnswer");

                    b.HasIndex("IdIssuer");

                    b.HasIndex("IdMessage");

                    b.HasIndex("IdQuestion");

                    b.ToTable("Report");
                });

            modelBuilder.Entity("Quanda.Shared.Models.Role", b =>
                {
                    b.Property<int>("IdRole")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("varchar(30)");

                    b.HasKey("IdRole")
                        .HasName("Role_pk");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("Quanda.Shared.Models.Service", b =>
                {
                    b.Property<int>("IdService")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Connection")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("IdService")
                        .HasName("Service_pk");

                    b.ToTable("Service");
                });

            modelBuilder.Entity("Quanda.Shared.Models.Tag", b =>
                {
                    b.Property<int>("IdTag")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("IdMainTag")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("varchar(30)");

                    b.HasKey("IdTag")
                        .HasName("Tag_pk");

                    b.HasIndex("IdMainTag");

                    b.ToTable("Tag");
                });

            modelBuilder.Entity("Quanda.Shared.Models.TagUser", b =>
                {
                    b.Property<int>("IdUser")
                        .HasColumnType("int");

                    b.Property<int>("IdTag")
                        .HasColumnType("int");

                    b.Property<int>("Points")
                        .HasColumnType("int");

                    b.HasKey("IdUser", "IdTag")
                        .HasName("TagUser_pk");

                    b.HasIndex("IdTag");

                    b.ToTable("Tag_User");
                });

            modelBuilder.Entity("Quanda.Shared.Models.TempUser", b =>
                {
                    b.Property<int>("IdUser")
                        .HasColumnType("int");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("varchar(36)");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("IdUser")
                        .HasName("TempUser_pk");

                    b.HasIndex("Code")
                        .IsUnique()
                        .HasDatabaseName("Unique_code");

                    b.ToTable("Temp_User");
                });

            modelBuilder.Entity("Quanda.Shared.Models.Thread", b =>
                {
                    b.Property<int>("IdThread")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Header")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<int>("IdReceiver")
                        .HasColumnType("int");

                    b.Property<int>("IdSender")
                        .HasColumnType("int");

                    b.HasKey("IdThread")
                        .HasName("Thread_pk");

                    b.HasIndex("IdReceiver");

                    b.HasIndex("IdSender");

                    b.ToTable("Thread");
                });

            modelBuilder.Entity("Quanda.Shared.Models.User", b =>
                {
                    b.Property<int>("IdUser")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Avatar")
                        .HasColumnType("longtext");

                    b.Property<string>("Bio")
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("FirstName")
                        .HasMaxLength(30)
                        .HasColumnType("varchar(30)");

                    b.Property<string>("HashedPassword")
                        .IsRequired()
                        .HasMaxLength(84)
                        .HasColumnType("varchar(84)");

                    b.Property<int?>("IdService")
                        .HasColumnType("int");

                    b.Property<bool>("IsDarkModeOff")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("LastName")
                        .HasMaxLength(30)
                        .HasColumnType("varchar(30)");

                    b.Property<string>("Nickname")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("varchar(30)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(9)
                        .HasColumnType("varchar(9)");

                    b.Property<string>("RefreshToken")
                        .HasMaxLength(36)
                        .HasColumnType("varchar(36)");

                    b.Property<DateTime?>("RefreshTokenExpirationDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("ServiceToken")
                        .HasColumnType("longtext");

                    b.HasKey("IdUser")
                        .HasName("User_pk");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasDatabaseName("Unique_email");

                    b.HasIndex("IdService");

                    b.HasIndex("Nickname")
                        .IsUnique()
                        .HasDatabaseName("Unique_nickname");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Quanda.Shared.Models.UserRole", b =>
                {
                    b.Property<int>("IdRole")
                        .HasColumnType("int");

                    b.Property<int>("IdUser")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ExpirationDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("IdRole", "IdUser")
                        .HasName("UserRole_pk");

                    b.HasIndex("IdUser");

                    b.ToTable("User_Role");
                });

            modelBuilder.Entity("Quanda.Shared.Models.Answer", b =>
                {
                    b.HasOne("Quanda.Shared.Models.Question", "IdQuestionNavigation")
                        .WithMany("Answers")
                        .HasForeignKey("IdQuestion")
                        .HasConstraintName("Answer_Question")
                        .IsRequired();

                    b.HasOne("Quanda.Shared.Models.Answer", "IdRootAnswerNavigation")
                        .WithMany("InverseIdRootAnswersNavigation")
                        .HasForeignKey("IdRootAnswer")
                        .HasConstraintName("Answer_Answer")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Quanda.Shared.Models.User", "IdUserNavigation")
                        .WithMany("Answers")
                        .HasForeignKey("IdUser")
                        .HasConstraintName("Answer_User")
                        .IsRequired();

                    b.Navigation("IdQuestionNavigation");

                    b.Navigation("IdRootAnswerNavigation");

                    b.Navigation("IdUserNavigation");
                });

            modelBuilder.Entity("Quanda.Shared.Models.Message", b =>
                {
                    b.HasOne("Quanda.Shared.Models.User", "IdSenderNavigation")
                        .WithMany("Messages")
                        .HasForeignKey("IdSender")
                        .HasConstraintName("Message_User")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Quanda.Shared.Models.Thread", "IdThreadNavigation")
                        .WithMany("Messages")
                        .HasForeignKey("IdThread")
                        .HasConstraintName("Message_Thread")
                        .IsRequired();

                    b.Navigation("IdSenderNavigation");

                    b.Navigation("IdThreadNavigation");
                });

            modelBuilder.Entity("Quanda.Shared.Models.Notification", b =>
                {
                    b.HasOne("Quanda.Shared.Models.User", "IdUserNavigation")
                        .WithMany("Notifications")
                        .HasForeignKey("IdUser")
                        .HasConstraintName("Notification_User")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("IdUserNavigation");
                });

            modelBuilder.Entity("Quanda.Shared.Models.Question", b =>
                {
                    b.HasOne("Quanda.Shared.Models.User", "IdUserNavigation")
                        .WithMany("Questions")
                        .HasForeignKey("IdUser")
                        .HasConstraintName("Question_User")
                        .IsRequired();

                    b.Navigation("IdUserNavigation");
                });

            modelBuilder.Entity("Quanda.Shared.Models.QuestionTag", b =>
                {
                    b.HasOne("Quanda.Shared.Models.Question", "IdQuestionNavigation")
                        .WithMany("QuestionTags")
                        .HasForeignKey("IdQuestion")
                        .HasConstraintName("QuestionTag_Question")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Quanda.Shared.Models.Tag", "IdTagNavigation")
                        .WithMany("QuestionTags")
                        .HasForeignKey("IdTag")
                        .HasConstraintName("QuestionTag_Tag")
                        .IsRequired();

                    b.Navigation("IdQuestionNavigation");

                    b.Navigation("IdTagNavigation");
                });

            modelBuilder.Entity("Quanda.Shared.Models.RatingAnswer", b =>
                {
                    b.HasOne("Quanda.Shared.Models.Answer", "IdAnswerNavigation")
                        .WithMany("RatingAnswers")
                        .HasForeignKey("IdAnswer")
                        .HasConstraintName("RatingAnswer_Answer")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Quanda.Shared.Models.User", "IdUserNavigation")
                        .WithMany("RatingAnswers")
                        .HasForeignKey("IdUser")
                        .HasConstraintName("RatingAnswer_User")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("IdAnswerNavigation");

                    b.Navigation("IdUserNavigation");
                });

            modelBuilder.Entity("Quanda.Shared.Models.Report", b =>
                {
                    b.HasOne("Quanda.Shared.Models.Answer", "IdAnswerNavigation")
                        .WithMany("Reports")
                        .HasForeignKey("IdAnswer")
                        .HasConstraintName("Report_Answer")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Quanda.Shared.Models.User", "IdIssuerNavigation")
                        .WithMany("Reports")
                        .HasForeignKey("IdIssuer")
                        .HasConstraintName("Report_User")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Quanda.Shared.Models.Message", "IdMessageNavigation")
                        .WithMany("Reports")
                        .HasForeignKey("IdMessage")
                        .HasConstraintName("Report_Message")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Quanda.Shared.Models.Question", "IdQuestionNavigation")
                        .WithMany("Reports")
                        .HasForeignKey("IdQuestion")
                        .HasConstraintName("Report_Question")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("IdAnswerNavigation");

                    b.Navigation("IdIssuerNavigation");

                    b.Navigation("IdMessageNavigation");

                    b.Navigation("IdQuestionNavigation");
                });

            modelBuilder.Entity("Quanda.Shared.Models.Tag", b =>
                {
                    b.HasOne("Quanda.Shared.Models.Tag", "IdMainTagNavigation")
                        .WithMany("InverseIdMainTagsNavigation")
                        .HasForeignKey("IdMainTag")
                        .HasConstraintName("Tag_Tag");

                    b.Navigation("IdMainTagNavigation");
                });

            modelBuilder.Entity("Quanda.Shared.Models.TagUser", b =>
                {
                    b.HasOne("Quanda.Shared.Models.Tag", "IdTagNavigation")
                        .WithMany("TagUsers")
                        .HasForeignKey("IdTag")
                        .HasConstraintName("TagUser_Tag")
                        .IsRequired();

                    b.HasOne("Quanda.Shared.Models.User", "IdUserNavigation")
                        .WithMany("TagUsers")
                        .HasForeignKey("IdUser")
                        .HasConstraintName("TagUser_User")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("IdTagNavigation");

                    b.Navigation("IdUserNavigation");
                });

            modelBuilder.Entity("Quanda.Shared.Models.TempUser", b =>
                {
                    b.HasOne("Quanda.Shared.Models.User", "IdUserNavigation")
                        .WithOne("IdTempUserNavigation")
                        .HasForeignKey("Quanda.Shared.Models.TempUser", "IdUser")
                        .HasConstraintName("TempUser_User")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("IdUserNavigation");
                });

            modelBuilder.Entity("Quanda.Shared.Models.Thread", b =>
                {
                    b.HasOne("Quanda.Shared.Models.User", "IdReceiverNavigation")
                        .WithMany("ReceiverThreads")
                        .HasForeignKey("IdReceiver")
                        .HasConstraintName("Thread_Receiver")
                        .IsRequired();

                    b.HasOne("Quanda.Shared.Models.User", "IdSenderNavigation")
                        .WithMany("SenderThreads")
                        .HasForeignKey("IdSender")
                        .HasConstraintName("Thread_Sender")
                        .IsRequired();

                    b.Navigation("IdReceiverNavigation");

                    b.Navigation("IdSenderNavigation");
                });

            modelBuilder.Entity("Quanda.Shared.Models.User", b =>
                {
                    b.HasOne("Quanda.Shared.Models.Service", "IdServiceNavigation")
                        .WithMany("Users")
                        .HasForeignKey("IdService")
                        .HasConstraintName("User_Service");

                    b.Navigation("IdServiceNavigation");
                });

            modelBuilder.Entity("Quanda.Shared.Models.UserRole", b =>
                {
                    b.HasOne("Quanda.Shared.Models.Role", "IdRoleNavigation")
                        .WithMany("UserRoles")
                        .HasForeignKey("IdRole")
                        .HasConstraintName("Role_UserRole")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Quanda.Shared.Models.User", "IdUserNavigation")
                        .WithMany("UserRoles")
                        .HasForeignKey("IdUser")
                        .HasConstraintName("User_UserRole")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("IdRoleNavigation");

                    b.Navigation("IdUserNavigation");
                });

            modelBuilder.Entity("Quanda.Shared.Models.Answer", b =>
                {
                    b.Navigation("InverseIdRootAnswersNavigation");

                    b.Navigation("RatingAnswers");

                    b.Navigation("Reports");
                });

            modelBuilder.Entity("Quanda.Shared.Models.Message", b =>
                {
                    b.Navigation("Reports");
                });

            modelBuilder.Entity("Quanda.Shared.Models.Question", b =>
                {
                    b.Navigation("Answers");

                    b.Navigation("QuestionTags");

                    b.Navigation("Reports");
                });

            modelBuilder.Entity("Quanda.Shared.Models.Role", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("Quanda.Shared.Models.Service", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("Quanda.Shared.Models.Tag", b =>
                {
                    b.Navigation("InverseIdMainTagsNavigation");

                    b.Navigation("QuestionTags");

                    b.Navigation("TagUsers");
                });

            modelBuilder.Entity("Quanda.Shared.Models.Thread", b =>
                {
                    b.Navigation("Messages");
                });

            modelBuilder.Entity("Quanda.Shared.Models.User", b =>
                {
                    b.Navigation("Answers");

                    b.Navigation("IdTempUserNavigation");

                    b.Navigation("Messages");

                    b.Navigation("Notifications");

                    b.Navigation("Questions");

                    b.Navigation("RatingAnswers");

                    b.Navigation("ReceiverThreads");

                    b.Navigation("Reports");

                    b.Navigation("SenderThreads");

                    b.Navigation("TagUsers");

                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
