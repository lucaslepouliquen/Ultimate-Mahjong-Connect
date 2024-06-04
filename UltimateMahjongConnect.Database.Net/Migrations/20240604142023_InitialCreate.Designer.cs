﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UltimateMahjongConnect.Database.Net.Models;

#nullable disable

namespace UltimateMahjongConnect.Database.Net.Migrations
{
    [DbContext(typeof(ApplicationDbSQLContext))]
    [Migration("20240604142023_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("UltimateMahjongConnect.Database.Net.Models.Gamer", b =>
                {
                    b.Property<int>("GamerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("GamerId"));

                    b.Property<string>("BankDetails")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Pseudonyme")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("GamerId");

                    b.ToTable("Gamers");
                });
#pragma warning restore 612, 618
        }
    }
}
