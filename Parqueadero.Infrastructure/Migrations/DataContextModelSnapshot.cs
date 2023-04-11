﻿// <auto-generated />
using System;
using Parqueadero.Infrastructure.DataSource;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Parqueadero.Infrastructure.Migrations;

[DbContext(typeof(DataContext))]
partial class DataContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder
            .HasAnnotation("ProductVersion", "7.0.0")
            .HasAnnotation("Relational:MaxIdentifierLength", 128);

        SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

        modelBuilder.Entity("Block.Domain.Entities.Voter", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uniqueidentifier");

                b.Property<DateTime>("CreatedOn")
                    .HasColumnType("datetime2");

                b.Property<DateTime>("DateOfBirth")
                    .HasColumnType("datetime2");

                b.Property<DateTime>("LastModifiedOn")
                    .HasColumnType("datetime2");

                b.Property<string>("Nid")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Origin")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.HasKey("Id");

                b.ToTable("Voter");
            });
#pragma warning restore 612, 618
    }
}


