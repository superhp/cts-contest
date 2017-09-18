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
    [Migration("20170912082301_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CtsContestWeb.Db.Entities.GivenPurchase", b =>
                {
                    b.Property<Guid>("GivenPurchaseId");

                    b.Property<DateTime>("Created");

                    b.HasKey("GivenPurchaseId");

                    b.ToTable("GivenPurchase");
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

            modelBuilder.Entity("CtsContestWeb.Db.Entities.GivenPurchase", b =>
                {
                    b.HasOne("CtsContestWeb.Db.Entities.Purchase", "Purchase")
                        .WithOne("GivenPurchase")
                        .HasForeignKey("CtsContestWeb.Db.Entities.GivenPurchase", "GivenPurchaseId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}