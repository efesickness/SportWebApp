using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SporWebDeneme1.Migrations
{
    public partial class TrainingSessionSeriesAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseSessionDay_CourseSessions_CourseSessionId",
                table: "CourseSessionDay");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseSessionDay",
                table: "CourseSessionDay");

            migrationBuilder.RenameTable(
                name: "CourseSessionDay",
                newName: "CourseSessionDays");

            migrationBuilder.RenameIndex(
                name: "IX_CourseSessionDay_CourseSessionId",
                table: "CourseSessionDays",
                newName: "IX_CourseSessionDays_CourseSessionId");

            migrationBuilder.AddColumn<int>(
                name: "SeriesId",
                table: "TrainingSessions",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseSessionDays",
                table: "CourseSessionDays",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "TrainingSessionSeries",
                columns: table => new
                {
                    SeriesId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CourseSessionId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    Note = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingSessionSeries", x => x.SeriesId);
                    table.ForeignKey(
                        name: "FK_TrainingSessionSeries_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrainingSessionSeries_CourseSessions_CourseSessionId",
                        column: x => x.CourseSessionId,
                        principalTable: "CourseSessions",
                        principalColumn: "CourseSessionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainingSessionSeriesDays",
                columns: table => new
                {
                    SeriesDayId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TrainingSessionSeriesId = table.Column<int>(type: "INTEGER", nullable: false),
                    DayOfWeek = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingSessionSeriesDays", x => x.SeriesDayId);
                    table.ForeignKey(
                        name: "FK_TrainingSessionSeriesDays_TrainingSessionSeries_TrainingSessionSeriesId",
                        column: x => x.TrainingSessionSeriesId,
                        principalTable: "TrainingSessionSeries",
                        principalColumn: "SeriesId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrainingSessions_SeriesId",
                table: "TrainingSessions",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingSessionSeries_CourseSessionId",
                table: "TrainingSessionSeries",
                column: "CourseSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingSessionSeries_UserId",
                table: "TrainingSessionSeries",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingSessionSeriesDays_TrainingSessionSeriesId",
                table: "TrainingSessionSeriesDays",
                column: "TrainingSessionSeriesId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseSessionDays_CourseSessions_CourseSessionId",
                table: "CourseSessionDays",
                column: "CourseSessionId",
                principalTable: "CourseSessions",
                principalColumn: "CourseSessionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingSessions_TrainingSessionSeries_SeriesId",
                table: "TrainingSessions",
                column: "SeriesId",
                principalTable: "TrainingSessionSeries",
                principalColumn: "SeriesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseSessionDays_CourseSessions_CourseSessionId",
                table: "CourseSessionDays");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainingSessions_TrainingSessionSeries_SeriesId",
                table: "TrainingSessions");

            migrationBuilder.DropTable(
                name: "TrainingSessionSeriesDays");

            migrationBuilder.DropTable(
                name: "TrainingSessionSeries");

            migrationBuilder.DropIndex(
                name: "IX_TrainingSessions_SeriesId",
                table: "TrainingSessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseSessionDays",
                table: "CourseSessionDays");

            migrationBuilder.DropColumn(
                name: "SeriesId",
                table: "TrainingSessions");

            migrationBuilder.RenameTable(
                name: "CourseSessionDays",
                newName: "CourseSessionDay");

            migrationBuilder.RenameIndex(
                name: "IX_CourseSessionDays_CourseSessionId",
                table: "CourseSessionDay",
                newName: "IX_CourseSessionDay_CourseSessionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseSessionDay",
                table: "CourseSessionDay",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseSessionDay_CourseSessions_CourseSessionId",
                table: "CourseSessionDay",
                column: "CourseSessionId",
                principalTable: "CourseSessions",
                principalColumn: "CourseSessionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
