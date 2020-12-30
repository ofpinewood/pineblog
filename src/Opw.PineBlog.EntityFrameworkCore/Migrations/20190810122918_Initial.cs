using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Opw.PineBlog.EntityFrameworkCore.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PineBlog_Authors",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    UserName = table.Column<string>(maxLength: 160, nullable: false),
                    Email = table.Column<string>(maxLength: 254, nullable: true),
                    DisplayName = table.Column<string>(maxLength: 160, nullable: false),
                    Bio = table.Column<string>(nullable: true),
                    Avatar = table.Column<string>(maxLength: 160, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PineBlog_Authors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PineBlog_Settings",
                columns: table => new
                {
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(maxLength: 160, nullable: false),
                    Description = table.Column<string>(nullable: true),
                    CoverUrl = table.Column<string>(nullable: true),
                    CoverCaption = table.Column<string>(nullable: true),
                    CoverLink = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PineBlog_Settings", x => x.Created);
                });

            migrationBuilder.CreateTable(
                name: "PineBlog_Posts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    AuthorId = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(maxLength: 160, nullable: false),
                    Description = table.Column<string>(maxLength: 450, nullable: false),
                    Content = table.Column<string>(nullable: false),
                    Categories = table.Column<string>(maxLength: 2000, nullable: true),
                    Published = table.Column<DateTime>(nullable: true),
                    Slug = table.Column<string>(maxLength: 160, nullable: false),
                    CoverUrl = table.Column<string>(maxLength: 254, nullable: true),
                    CoverCaption = table.Column<string>(maxLength: 160, nullable: true),
                    CoverLink = table.Column<string>(maxLength: 254, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PineBlog_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PineBlog_Posts_PineBlog_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "PineBlog_Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PineBlog_Posts_AuthorId",
                table: "PineBlog_Posts",
                column: "AuthorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PineBlog_Posts");

            migrationBuilder.DropTable(
                name: "PineBlog_Settings");

            migrationBuilder.DropTable(
                name: "PineBlog_Authors");
        }
    }
}
