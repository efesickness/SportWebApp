using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SporWebDeneme1.Migrations
{
    public partial class CourseBranchIdAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Courses",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_BranchId",
                table: "Courses",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Branches_BranchId",
                table: "Courses",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Branches_BranchId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_BranchId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Courses");
        }
    }
}
