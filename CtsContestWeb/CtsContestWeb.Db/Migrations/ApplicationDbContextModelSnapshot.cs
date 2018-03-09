﻿// <auto-generated />
using CtsContestWeb.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace CtsContestWeb.Db.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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

            modelBuilder.Entity("CtsContestWeb.Db.Entities.GivenPurchase", b =>
                {
                    b.Property<Guid>("GivenPurchaseId");

                    b.Property<DateTime>("Created");

                    b.HasKey("GivenPurchaseId");

                    b.ToTable("GivenPurchases");
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
                        .ValueGeneratedOnAdd();

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
#pragma warning restore 612, 618
        }
    }
}
