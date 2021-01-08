using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Opw.PineBlog.EntityFrameworkCore.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class BlogSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PineBlog_Settings");

            migrationBuilder.CreateTable(
                name: "PineBlog_BlogSettings",
                columns: table => new
                {
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(maxLength: 160, nullable: false),
                    Description = table.Column<string>(maxLength: 450, nullable: true),
                    CoverUrl = table.Column<string>(maxLength: 254, nullable: true),
                    CoverCaption = table.Column<string>(maxLength: 160, nullable: true),
                    CoverLink = table.Column<string>(maxLength: 254, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PineBlog_BlogSettings", x => x.Created);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PineBlog_BlogSettings");

            migrationBuilder.CreateTable(
                name: "PineBlog_Settings",
                columns: table => new
                {
                    Created = table.Column<DateTime>(nullable: false),
                    CoverCaption = table.Column<string>(nullable: true),
                    CoverLink = table.Column<string>(nullable: true),
                    CoverUrl = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Modified = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(maxLength: 160, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PineBlog_Settings", x => x.Created);
                });
        }
    }
}
