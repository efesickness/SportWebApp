using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SporWebDeneme1.Migrations
{
    public partial class TrainingAdded2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrainingSessions",
                columns: table => new
                {
                    TrainingSessionId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CourseSessionId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    Note = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingSessions", x => x.TrainingSessionId);
                    table.ForeignKey(
                        name: "FK_TrainingSessions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrainingSessions_CourseSessions_CourseSessionId",
                        column: x => x.CourseSessionId,
                        principalTable: "CourseSessions",
                        principalColumn: "CourseSessionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainingAssistants",
                columns: table => new
                {
                    TrainingAssistantId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TrainingSessionId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingAssistants", x => x.TrainingAssistantId);
                    table.ForeignKey(
                        name: "FK_TrainingAssistants_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrainingAssistants_TrainingSessions_TrainingSessionId",
                        column: x => x.TrainingSessionId,
                        principalTable: "TrainingSessions",
                        principalColumn: "TrainingSessionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainingStaffs",
                columns: table => new
                {
                    TrainingStuffId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TrainingSessionId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingStaffs", x => x.TrainingStuffId);
                    table.ForeignKey(
                        name: "FK_TrainingStaffs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrainingStaffs_TrainingSessions_TrainingSessionId",
                        column: x => x.TrainingSessionId,
                        principalTable: "TrainingSessions",
                        principalColumn: "TrainingSessionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrainingAssistants_TrainingSessionId",
                table: "TrainingAssistants",
                column: "TrainingSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingAssistants_UserId",
                table: "TrainingAssistants",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingSessions_CourseSessionId",
                table: "TrainingSessions",
                column: "CourseSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingSessions_UserId",
                table: "TrainingSessions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingStaffs_TrainingSessionId",
                table: "TrainingStaffs",
                column: "TrainingSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingStaffs_UserId",
                table: "TrainingStaffs",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainingAssistants");

            migrationBuilder.DropTable(
                name: "TrainingStaffs");

            migrationBuilder.DropTable(
                name: "TrainingSessions");
        }
    }
}
