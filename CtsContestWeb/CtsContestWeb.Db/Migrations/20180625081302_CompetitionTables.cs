using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CtsContestWeb.Db.Migrations
{
    public partial class CompetitionTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Competitions",
                columns: table => new
                {
                    CompetitionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstPlayerEmail = table.Column<string>(nullable: true),
                    SecondPlayerEmail = table.Column<string>(nullable: true),
                    WinnerEmail = table.Column<string>(nullable: true),
                    TaskId = table.Column<int>(nullable: false),
                    Prize = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competitions", x => x.CompetitionId);
                    table.ForeignKey(
                        name: "FK_Competitions_Users_FirstPlayerEmail",
                        column: x => x.FirstPlayerEmail,
                        principalTable: "Users",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Competitions_Users_SecondPlayerEmail",
                        column: x => x.SecondPlayerEmail,
                        principalTable: "Users",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Competitions_Users_WinnerEmail",
                        column: x => x.WinnerEmail,
                        principalTable: "Users",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CompetitionSolutions",
                columns: table => new
                {
                    CompetitionSolutionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CompetitionId = table.Column<int>(nullable: false),
                    UserEmail = table.Column<string>(nullable: true),
                    Source = table.Column<string>(nullable: true),
                    IsCorrect = table.Column<bool>(nullable: false),
                    Language = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetitionSolutions", x => x.CompetitionSolutionId);
                    table.ForeignKey(
                        name: "FK_CompetitionSolutions_Competitions_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competitions",
                        principalColumn: "CompetitionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompetitionSolutions_Users_UserEmail",
                        column: x => x.UserEmail,
                        principalTable: "Users",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Competitions_SecondPlayerEmail",
                table: "Competitions",
                column: "SecondPlayerEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Competitions_WinnerEmail",
                table: "Competitions",
                column: "WinnerEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Competitions_FirstPlayerEmail_SecondPlayerEmail_TaskId",
                table: "Competitions",
                columns: new[] { "FirstPlayerEmail", "SecondPlayerEmail", "TaskId" },
                unique: true,
                filter: "[FirstPlayerEmail] IS NOT NULL AND [SecondPlayerEmail] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionSolutions_CompetitionId",
                table: "CompetitionSolutions",
                column: "CompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionSolutions_UserEmail_CompetitionId",
                table: "CompetitionSolutions",
                columns: new[] { "UserEmail", "CompetitionId" },
                unique: true,
                filter: "[UserEmail] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompetitionSolutions");

            migrationBuilder.DropTable(
                name: "Competitions");
        }
    }
}
