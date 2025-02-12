﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PakingAPI;

namespace PakingAPI.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PakingAPI.Model.Parking", b =>
                {
                    b.Property<int>("ParkingID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(25)")
                        .HasMaxLength(25);

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FreeParkingEnd")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FreeParkingStart")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StreetAdress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("ParkingID");

                    b.HasIndex("UserID");

                    b.ToTable("Parkings");

                    b.HasData(
                        new
                        {
                            ParkingID = 1,
                            City = "Gathenburg",
                            Country = "Sweden",
                            FreeParkingEnd = "Ends at 15 march",
                            FreeParkingStart = "free 24 h",
                            StreetAdress = "Danska vägen",
                            UserID = 1
                        },
                        new
                        {
                            ParkingID = 2,
                            City = "Gathenburg",
                            Country = "Sweden",
                            FreeParkingEnd = "Ends at 08:00",
                            FreeParkingStart = "free from 18:00",
                            StreetAdress = "Exportgatan",
                            UserID = 3
                        },
                        new
                        {
                            ParkingID = 3,
                            City = "Uppsala",
                            Country = "Sweden",
                            FreeParkingEnd = "Ends at 08:00",
                            FreeParkingStart = "free from 18:00",
                            StreetAdress = "Gränby vägen",
                            UserID = 1
                        });
                });

            modelBuilder.Entity("PakingAPI.Model.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserID");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserID = 1,
                            Age = 24,
                            Email = "samir20jan@gmail.com",
                            FirstName = "Samir",
                            LastName = "Jan"
                        },
                        new
                        {
                            UserID = 2,
                            Age = 30,
                            Email = "johan@gmail.com",
                            FirstName = "Johan",
                            LastName = "Berg"
                        },
                        new
                        {
                            UserID = 3,
                            Age = 20,
                            Email = "david.jahn@gmail.com",
                            FirstName = "David",
                            LastName = "Jahnson"
                        });
                });

            modelBuilder.Entity("PakingAPI.Models.Feedback", b =>
                {
                    b.Property<int>("FeedbackID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ParkingID")
                        .HasColumnType("int");

                    b.Property<int?>("Rate")
                        .HasColumnType("int");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("FeedbackID");

                    b.HasIndex("ParkingID");

                    b.HasIndex("UserID");

                    b.ToTable("Feedbacks");

                    b.HasData(
                        new
                        {
                            FeedbackID = 1,
                            Comment = "Greate parking",
                            ParkingID = 1,
                            Rate = 8,
                            UserID = 2
                        },
                        new
                        {
                            FeedbackID = 2,
                            Comment = "There is no parking at this adress",
                            ParkingID = 2,
                            Rate = 1,
                            UserID = 3
                        });
                });

            modelBuilder.Entity("PakingAPI.Models.UserAccount", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("UserAccounts");
                });

            modelBuilder.Entity("PakingAPI.Model.Parking", b =>
                {
                    b.HasOne("PakingAPI.Model.User", "User")
                        .WithMany("Parkings")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PakingAPI.Models.Feedback", b =>
                {
                    b.HasOne("PakingAPI.Model.Parking", "Parking")
                        .WithMany("feedbacks")
                        .HasForeignKey("ParkingID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PakingAPI.Model.User", "User")
                        .WithMany("Feedbacks")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
