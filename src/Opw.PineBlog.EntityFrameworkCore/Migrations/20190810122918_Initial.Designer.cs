﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Opw.PineBlog.EntityFrameworkCore;

namespace Opw.PineBlog.EntityFrameworkCore.Migrations
{
    [DbContext(typeof(BlogEntityDbContext))]
    [Migration("20190810122918_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Opw.PineBlog.Entities.Author", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Avatar")
                        .HasMaxLength(160);

                    b.Property<string>("Bio");

                    b.Property<DateTime>("Created");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(160);

                    b.Property<string>("Email")
                        .HasMaxLength(254);

                    b.Property<DateTime>("Modified");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(160);

                    b.HasKey("Id");

                    b.ToTable("PineBlog_Authors");
                });

            modelBuilder.Entity("Opw.PineBlog.Entities.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("AuthorId");

                    b.Property<string>("Categories")
                        .HasMaxLength(2000);

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<string>("CoverCaption")
                        .HasMaxLength(160);

                    b.Property<string>("CoverLink")
                        .HasMaxLength(254);

                    b.Property<string>("CoverUrl")
                        .HasMaxLength(254);

                    b.Property<DateTime>("Created");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(450);

                    b.Property<DateTime>("Modified");

                    b.Property<DateTime?>("Published");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasMaxLength(160);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(160);

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("PineBlog_Posts");
                });

            modelBuilder.Entity("Opw.PineBlog.Entities.Settings", b =>
                {
                    b.Property<DateTime>("Created");

                    b.Property<string>("CoverCaption");

                    b.Property<string>("CoverLink");

                    b.Property<string>("CoverUrl");

                    b.Property<string>("Description");

                    b.Property<DateTime>("Modified");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(160);

                    b.HasKey("Created");

                    b.ToTable("PineBlog_Settings");
                });

            modelBuilder.Entity("Opw.PineBlog.Entities.Post", b =>
                {
                    b.HasOne("Opw.PineBlog.Entities.Author", "Author")
                        .WithMany("Posts")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
