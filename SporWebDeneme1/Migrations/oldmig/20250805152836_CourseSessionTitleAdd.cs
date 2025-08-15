using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SporWebDeneme1.Migrations
{
    public partial class CourseSessionTitleAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "CourseSessions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "CourseSessions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "CourseSessions");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "CourseSessions");
        }
    }
}
