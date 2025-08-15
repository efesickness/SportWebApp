using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SporWebDeneme1.Migrations
{
    public partial class CoursePriceUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SessionPrice",
                table: "CourseSessions");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Courses",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Courses");

            migrationBuilder.AddColumn<decimal>(
                name: "SessionPrice",
                table: "CourseSessions",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
