﻿// <auto-generated />
using System;
using FinanceConsoleLibrary.DataAccess.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FinanceConsoleLibrary.Migrations
{
    [DbContext(typeof(StorageContext))]
    [Migration("20230523224619_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");

            modelBuilder.Entity("FinanceConsoleLibrary.DataAccess.Database.Models.Account", b =>
                {
                    b.Property<ushort>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasMaxLength(128)
                        .IsUnicode(false)
                        .HasColumnType("TEXT");

                    b.Property<ushort>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("FinanceConsoleLibrary.DataAccess.Database.Models.Transaction", b =>
                {
                    b.Property<ulong>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AmountInCent")
                        .HasColumnType("INTEGER");

                    b.Property<ushort>("BaseAccountId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("BookingDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .IsUnicode(false)
                        .HasColumnType("TEXT");

                    b.Property<string>("Reference")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .IsUnicode(false)
                        .HasColumnType("TEXT");

                    b.Property<string>("Target")
                        .IsRequired()
                        .HasMaxLength(128)
                        .IsUnicode(false)
                        .HasColumnType("TEXT");

                    b.Property<string>("TargetIban")
                        .IsRequired()
                        .HasMaxLength(128)
                        .IsUnicode(false)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ValueDate")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("BaseAccountId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("FinanceConsoleLibrary.DataAccess.Database.Models.User", b =>
                {
                    b.Property<ushort>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasMaxLength(256)
                        .IsUnicode(false)
                        .HasColumnType("TEXT");

                    b.Property<string>("LoginName")
                        .IsRequired()
                        .HasMaxLength(32)
                        .IsUnicode(false)
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .IsUnicode(false)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("FinanceConsoleLibrary.DataAccess.Database.Models.Account", b =>
                {
                    b.HasOne("FinanceConsoleLibrary.DataAccess.Database.Models.User", "User")
                        .WithMany("Accounts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("FinanceConsoleLibrary.DataAccess.Database.Models.Transaction", b =>
                {
                    b.HasOne("FinanceConsoleLibrary.DataAccess.Database.Models.Account", "BaseAccount")
                        .WithMany("Transactions")
                        .HasForeignKey("BaseAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BaseAccount");
                });

            modelBuilder.Entity("FinanceConsoleLibrary.DataAccess.Database.Models.Account", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("FinanceConsoleLibrary.DataAccess.Database.Models.User", b =>
                {
                    b.Navigation("Accounts");
                });
#pragma warning restore 612, 618
        }
    }
}
