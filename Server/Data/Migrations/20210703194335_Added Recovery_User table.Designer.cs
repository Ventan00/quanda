﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Quanda.Server.Data;

namespace Quanda.Server.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20210703194335_Added Recovery_User table")]
    partial class AddedRecovery_Usertable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.7");

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

                    b.Property<int>("Rating")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

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

            modelBuilder.Entity("Quanda.Shared.Models.Category", b =>
                {
                    b.Property<int>("IdCategory")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("IdMainCategory")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("varchar(30)");

                    b.HasKey("IdCategory")
                        .HasName("Category_pk");

                    b.HasIndex("IdMainCategory");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("Quanda.Shared.Models.Notification", b =>
                {
                    b.Property<int>("IdQuestion")
                        .HasColumnType("int");

                    b.Property<int>("IdUser")
                        .HasColumnType("int");

                    b.HasKey("IdQuestion", "IdUser");

                    b.HasIndex("IdUser");

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

            modelBuilder.Entity("Quanda.Shared.Models.QuestionCategory", b =>
                {
                    b.Property<int>("IdQuestion")
                        .HasColumnType("int");

                    b.Property<int>("IdCategory")
                        .HasColumnType("int");

                    b.HasKey("IdQuestion", "IdCategory");

                    b.HasIndex("IdCategory");

                    b.ToTable("Question_Category");
                });

            modelBuilder.Entity("Quanda.Shared.Models.RecoveryUser", b =>
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
                        .HasName("RecoveryUser_pk");

                    b.HasIndex("Code")
                        .IsUnique()
                        .HasDatabaseName("Unique_code");

                    b.ToTable("Recovery_User");
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

                    b.Property<int>("Points")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<string>("RefreshToken")
                        .HasMaxLength(36)
                        .HasColumnType("varchar(36)");

                    b.Property<DateTime?>("RefreshTokenExpirationDate")
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

            modelBuilder.Entity("Quanda.Shared.Models.Category", b =>
                {
                    b.HasOne("Quanda.Shared.Models.Category", "IdMainCategoryNavigation")
                        .WithMany("InverseIdMainCategoriesNavigation")
                        .HasForeignKey("IdMainCategory")
                        .HasConstraintName("Category_Category");

                    b.Navigation("IdMainCategoryNavigation");
                });

            modelBuilder.Entity("Quanda.Shared.Models.Notification", b =>
                {
                    b.HasOne("Quanda.Shared.Models.Question", "IdQuestionNavigation")
                        .WithMany("Notifications")
                        .HasForeignKey("IdQuestion")
                        .HasConstraintName("Question_Notification")
                        .IsRequired();

                    b.HasOne("Quanda.Shared.Models.User", "IdUserNavigation")
                        .WithMany("Notifications")
                        .HasForeignKey("IdUser")
                        .HasConstraintName("User_Notification")
                        .IsRequired();

                    b.Navigation("IdQuestionNavigation");

                    b.Navigation("IdUserNavigation");
                });

            modelBuilder.Entity("Quanda.Shared.Models.Question", b =>
                {
                    b.HasOne("Quanda.Shared.Models.User", "IdUserNavigation")
                        .WithMany("Questions")
                        .HasForeignKey("IdUser")
                        .HasConstraintName("User_Question")
                        .IsRequired();

                    b.Navigation("IdUserNavigation");
                });

            modelBuilder.Entity("Quanda.Shared.Models.QuestionCategory", b =>
                {
                    b.HasOne("Quanda.Shared.Models.Category", "IdCategoryNavigation")
                        .WithMany("QuestionCategories")
                        .HasForeignKey("IdCategory")
                        .HasConstraintName("Category_QuestionCategory")
                        .IsRequired();

                    b.HasOne("Quanda.Shared.Models.Question", "IdQuestionNavigation")
                        .WithMany("QuestionCategories")
                        .HasForeignKey("IdQuestion")
                        .HasConstraintName("Question_QuestionCategory")
                        .IsRequired();

                    b.Navigation("IdCategoryNavigation");

                    b.Navigation("IdQuestionNavigation");
                });

            modelBuilder.Entity("Quanda.Shared.Models.RecoveryUser", b =>
                {
                    b.HasOne("Quanda.Shared.Models.User", "IdUserNavigation")
                        .WithOne("IdRecoveryUserNavigation")
                        .HasForeignKey("Quanda.Shared.Models.RecoveryUser", "IdUser")
                        .HasConstraintName("RecoveryUser_User")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

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

            modelBuilder.Entity("Quanda.Shared.Models.User", b =>
                {
                    b.HasOne("Quanda.Shared.Models.Service", "IdServiceNavigation")
                        .WithMany("Users")
                        .HasForeignKey("IdService")
                        .HasConstraintName("Service_User");

                    b.Navigation("IdServiceNavigation");
                });

            modelBuilder.Entity("Quanda.Shared.Models.UserRole", b =>
                {
                    b.HasOne("Quanda.Shared.Models.Role", "IdRoleNavigation")
                        .WithMany("UserRoles")
                        .HasForeignKey("IdRole")
                        .HasConstraintName("Role_UserRole")
                        .IsRequired();

                    b.HasOne("Quanda.Shared.Models.User", "IdUserNavigation")
                        .WithMany("UserRoles")
                        .HasForeignKey("IdUser")
                        .HasConstraintName("User_UserRole")
                        .IsRequired();

                    b.Navigation("IdRoleNavigation");

                    b.Navigation("IdUserNavigation");
                });

            modelBuilder.Entity("Quanda.Shared.Models.Answer", b =>
                {
                    b.Navigation("InverseIdRootAnswersNavigation");
                });

            modelBuilder.Entity("Quanda.Shared.Models.Category", b =>
                {
                    b.Navigation("InverseIdMainCategoriesNavigation");

                    b.Navigation("QuestionCategories");
                });

            modelBuilder.Entity("Quanda.Shared.Models.Question", b =>
                {
                    b.Navigation("Answers");

                    b.Navigation("Notifications");

                    b.Navigation("QuestionCategories");
                });

            modelBuilder.Entity("Quanda.Shared.Models.Role", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("Quanda.Shared.Models.Service", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("Quanda.Shared.Models.User", b =>
                {
                    b.Navigation("Answers");

                    b.Navigation("IdRecoveryUserNavigation");

                    b.Navigation("IdTempUserNavigation");

                    b.Navigation("Notifications");

                    b.Navigation("Questions");

                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
