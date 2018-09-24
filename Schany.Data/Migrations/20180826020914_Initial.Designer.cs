﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Schany.Data.DataContext;

namespace Schany.Data.Migrations
{
    [DbContext(typeof(SchanyDbContext))]
    [Migration("20180826020914_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.2-rtm-30932")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Schany.Data.Entities.Customer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateTime");

                    b.Property<Guid>("CreateUserId");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool?>("IsLocked");

                    b.Property<DateTime?>("LastLoginTime");

                    b.Property<int?>("LoginErrorTimes");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(18);

                    b.Property<string>("Pic")
                        .HasMaxLength(200);

                    b.Property<string>("TrueName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("Schany.Data.Entities.MyDictionary", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Code");

                    b.Property<DateTime>("CreateTime");

                    b.Property<Guid>("CreateUserId");

                    b.Property<int>("DicType");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("LastUpdatedTime");

                    b.Property<Guid?>("LastUpdatedUserId");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.HasIndex("CreateUserId");

                    b.ToTable("MyDictionaries");
                });

            modelBuilder.Entity("Schany.Data.Entities.MyDictionary", b =>
                {
                    b.HasOne("Schany.Data.Entities.Customer", "CreateUser")
                        .WithMany()
                        .HasForeignKey("CreateUserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
