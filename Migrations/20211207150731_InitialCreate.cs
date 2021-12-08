using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace geoDistance.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "addresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    Street = table.Column<string>(type: "TEXT", nullable: true),
                    Number = table.Column<string>(type: "TEXT", nullable: true),
                    City = table.Column<string>(type: "TEXT", nullable: true),
                    Country = table.Column<string>(type: "TEXT", nullable: true),
                    PostalCode = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_addresses", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "addresses");
        }
    }
}
