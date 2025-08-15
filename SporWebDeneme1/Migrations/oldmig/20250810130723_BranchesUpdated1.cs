using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SporWebDeneme1.Migrations
{
    public partial class BranchesUpdated1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BranchAssignments_AspNetUsers_ApplicationUserId",
                table: "BranchAssignments");

            migrationBuilder.DropIndex(
                name: "IX_BranchAssignments_ApplicationUserId",
                table: "BranchAssignments");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "BranchAssignments");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "BranchAssignments",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.CreateIndex(
                name: "IX_BranchAssignments_UserId",
                table: "BranchAssignments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BranchAssignments_AspNetUsers_UserId",
                table: "BranchAssignments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BranchAssignments_AspNetUsers_UserId",
                table: "BranchAssignments");

            migrationBuilder.DropIndex(
                name: "IX_BranchAssignments_UserId",
                table: "BranchAssignments");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "BranchAssignments",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "BranchAssignments",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BranchAssignments_ApplicationUserId",
                table: "BranchAssignments",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BranchAssignments_AspNetUsers_ApplicationUserId",
                table: "BranchAssignments",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
