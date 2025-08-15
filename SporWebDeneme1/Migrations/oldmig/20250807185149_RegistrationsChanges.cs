using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SporWebDeneme1.Migrations
{
    public partial class RegistrationsChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CourseSessionId",
                table: "Registrations",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "Registrations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_CourseId",
                table: "Registrations",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Registrations_Courses_CourseId",
                table: "Registrations",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Registrations_Courses_CourseId",
                table: "Registrations");

            migrationBuilder.DropIndex(
                name: "IX_Registrations_CourseId",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Registrations");

            migrationBuilder.AlterColumn<int>(
                name: "CourseSessionId",
                table: "Registrations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);
        }
    }
}
