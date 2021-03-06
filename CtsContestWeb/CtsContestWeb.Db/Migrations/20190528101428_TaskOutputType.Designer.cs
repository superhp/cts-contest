﻿// <auto-generated />
using System;
using CtsContestWeb.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CtsContestWeb.Db.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20190528101428_TaskOutputType")]
    partial class TaskOutputType
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CtsContestWeb.Db.Entities.CodeSkeleton", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Language");

                    b.Property<string>("ReadInputIntegerNumberOfLinesOfIntegers");

                    b.Property<string>("ReadInteger");

                    b.Property<string>("ReadLine");

                    b.Property<string>("ReadLineOfIntegers");

                    b.Property<string>("Skeleton");

                    b.Property<string>("WriteLine");

                    b.HasKey("Id");

                    b.ToTable("CodeSkeletons");
                });

            modelBuilder.Entity("CtsContestWeb.Db.Entities.ContactInfo", b =>
                {
                    b.Property<string>("Email")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Answer");

                    b.Property<string>("CourseName");

                    b.Property<int?>("CourseNumber");

                    b.Property<DateTime>("Created");

                    b.Property<string>("Degree");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Phone");

                    b.Property<string>("Surname")
                        .IsRequired();

                    b.HasKey("Email");

                    b.ToTable("ContactInformation");
                });

            modelBuilder.Entity("CtsContestWeb.Db.Entities.Duel", b =>
                {
                    b.Property<int>("DuelId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Created");

                    b.Property<string>("FirstPlayerEmail");

                    b.Property<int>("Prize");

                    b.Property<string>("SecondPlayerEmail");

                    b.Property<int>("TaskId");

                    b.Property<string>("WinnerEmail");

                    b.HasKey("DuelId");

                    b.HasIndex("SecondPlayerEmail");

                    b.HasIndex("WinnerEmail");

                    b.HasIndex("FirstPlayerEmail", "SecondPlayerEmail", "TaskId")
                        .IsUnique()
                        .HasFilter("[FirstPlayerEmail] IS NOT NULL AND [SecondPlayerEmail] IS NOT NULL");

                    b.ToTable("Duels");
                });

            modelBuilder.Entity("CtsContestWeb.Db.Entities.DuelSolution", b =>
                {
                    b.Property<int>("DuelSolutionId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Created");

                    b.Property<int>("DuelId");

                    b.Property<bool>("IsCorrect");

                    b.Property<int>("Language");

                    b.Property<string>("Source");

                    b.Property<string>("UserEmail");

                    b.HasKey("DuelSolutionId");

                    b.HasIndex("DuelId");

                    b.HasIndex("UserEmail", "DuelId")
                        .IsUnique()
                        .HasFilter("[UserEmail] IS NOT NULL");

                    b.ToTable("DuelSolutions");
                });

            modelBuilder.Entity("CtsContestWeb.Db.Entities.GivenPurchase", b =>
                {
                    b.Property<Guid>("GivenPurchaseId");

                    b.Property<DateTime>("Created");

                    b.HasKey("GivenPurchaseId");

                    b.ToTable("GivenPurchases");
                });

            modelBuilder.Entity("CtsContestWeb.Db.Entities.Prize", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Category");

                    b.Property<DateTime>("Created");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<string>("Picture");

                    b.Property<int>("Price");

                    b.Property<int>("Quantity");

                    b.HasKey("Id");

                    b.ToTable("Prizes");
                });

            modelBuilder.Entity("CtsContestWeb.Db.Entities.Purchase", b =>
                {
                    b.Property<Guid>("PurchaseId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Cost");

                    b.Property<DateTime>("Created");

                    b.Property<int>("PrizeId");

                    b.Property<string>("UserEmail");

                    b.HasKey("PurchaseId");

                    b.HasIndex("UserEmail", "PrizeId")
                        .IsUnique()
                        .HasFilter("[UserEmail] IS NOT NULL");

                    b.ToTable("Purchases");
                });

            modelBuilder.Entity("CtsContestWeb.Db.Entities.Solution", b =>
                {
                    b.Property<int>("SolutionId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Created");

                    b.Property<bool>("IsCorrect");

                    b.Property<int>("Language");

                    b.Property<int>("Score");

                    b.Property<string>("Source");

                    b.Property<int>("TaskId");

                    b.Property<string>("UserEmail");

                    b.HasKey("SolutionId");

                    b.HasIndex("UserEmail", "TaskId")
                        .IsUnique()
                        .HasFilter("[UserEmail] IS NOT NULL");

                    b.ToTable("Solutions");
                });

            modelBuilder.Entity("CtsContestWeb.Db.Entities.Task", b =>
                {
                    b.Property<int>("Id");

                    b.Property<DateTime>("Created");

                    b.Property<string>("Description");

                    b.Property<bool>("Enabled");

                    b.Property<string>("InputType");

                    b.Property<string>("Name");

                    b.Property<string>("OutputType");

                    b.Property<int>("Value");

                    b.HasKey("Id");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("CtsContestWeb.Db.Entities.TaskTestCase", b =>
                {
                    b.Property<int>("TaskTestCaseId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Created");

                    b.Property<string>("Input");

                    b.Property<bool>("IsSample");

                    b.Property<string>("Output");

                    b.Property<int>("TaskId");

                    b.HasKey("TaskTestCaseId");

                    b.HasIndex("TaskId");

                    b.ToTable("TaskTestCases");
                });

            modelBuilder.Entity("CtsContestWeb.Db.Entities.User", b =>
                {
                    b.Property<string>("Email")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FullName");

                    b.Property<string>("Picture");

                    b.HasKey("Email");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CtsContestWeb.Db.Entities.Duel", b =>
                {
                    b.HasOne("CtsContestWeb.Db.Entities.User", "FirstPlayer")
                        .WithMany()
                        .HasForeignKey("FirstPlayerEmail");

                    b.HasOne("CtsContestWeb.Db.Entities.User", "SecondPlayer")
                        .WithMany()
                        .HasForeignKey("SecondPlayerEmail");

                    b.HasOne("CtsContestWeb.Db.Entities.User", "Winner")
                        .WithMany()
                        .HasForeignKey("WinnerEmail");
                });

            modelBuilder.Entity("CtsContestWeb.Db.Entities.DuelSolution", b =>
                {
                    b.HasOne("CtsContestWeb.Db.Entities.Duel", "Duel")
                        .WithMany()
                        .HasForeignKey("DuelId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CtsContestWeb.Db.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserEmail");
                });

            modelBuilder.Entity("CtsContestWeb.Db.Entities.GivenPurchase", b =>
                {
                    b.HasOne("CtsContestWeb.Db.Entities.Purchase", "Purchase")
                        .WithOne("GivenPurchase")
                        .HasForeignKey("CtsContestWeb.Db.Entities.GivenPurchase", "GivenPurchaseId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CtsContestWeb.Db.Entities.Purchase", b =>
                {
                    b.HasOne("CtsContestWeb.Db.Entities.User", "User")
                        .WithMany("Purchases")
                        .HasForeignKey("UserEmail");
                });

            modelBuilder.Entity("CtsContestWeb.Db.Entities.Solution", b =>
                {
                    b.HasOne("CtsContestWeb.Db.Entities.User", "User")
                        .WithMany("Solutions")
                        .HasForeignKey("UserEmail");
                });

            modelBuilder.Entity("CtsContestWeb.Db.Entities.TaskTestCase", b =>
                {
                    b.HasOne("CtsContestWeb.Db.Entities.Task", "Task")
                        .WithMany("TestCases")
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
