﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UltimateMahjongConnect.Infrastructure.Models;

#nullable disable

namespace UltimateMahjongConnect.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbSQLContext))]
    partial class ApplicationDbSQLContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.3");

            modelBuilder.Entity("UltimateMahjongConnect.Infrastructure.Persistence.GamerEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Age")
                        .HasColumnType("INTEGER");

                    b.Property<string>("BankDetails")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Pseudonyme")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("Score")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Gamers");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Age = 25,
                            BankDetails = "FR76 1234 5678 9012 3456 7890 123",
                            Email = "testgamer@example.com",
                            Password = "SecurePassword123!",
                            Pseudonyme = "TestGamer123",
                            Score = 1000
                        },
                        new
                        {
                            Id = 2,
                            Age = 18,
                            BankDetails = "FR76 7890 5678 9012 3456 1234 123",
                            Email = "darkzelios@example.com",
                            Password = "DarkZelios123!",
                            Pseudonyme = "DarkZelios",
                            Score = 10000
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
