using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SporWebDeneme1.Migrations
{
    public partial class Update1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "StartTime",
                table: "CourseSessionDay",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "EndTime",
                table: "CourseSessionDay",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "TEXT");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "StartTime",
                table: "CourseSessionDay",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0),
                oldClrType: typeof(TimeSpan),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "EndTime",
                table: "CourseSessionDay",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0),
                oldClrType: typeof(TimeSpan),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}
