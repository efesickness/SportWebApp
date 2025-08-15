using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SporWebDeneme1.Migrations
{
    public partial class CourseSessionDaysAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DaysOfWeek",
                table: "CourseSessions");

            migrationBuilder.CreateTable(
                name: "CourseSessionDay",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CourseSessionId = table.Column<int>(type: "INTEGER", nullable: false),
                    DayOfWeek = table.Column<int>(type: "INTEGER", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseSessionDay", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseSessionDay_CourseSessions_CourseSessionId",
                        column: x => x.CourseSessionId,
                        principalTable: "CourseSessions",
                        principalColumn: "CourseSessionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseSessionDay_CourseSessionId",
                table: "CourseSessionDay",
                column: "CourseSessionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseSessionDay");

            migrationBuilder.AddColumn<int>(
                name: "DaysOfWeek",
                table: "CourseSessions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
