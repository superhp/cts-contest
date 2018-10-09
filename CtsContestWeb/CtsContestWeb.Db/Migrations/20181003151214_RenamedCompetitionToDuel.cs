using Microsoft.EntityFrameworkCore.Migrations;

namespace CtsContestWeb.Db.Migrations
{
    public partial class RenamedCompetitionToDuel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
             name: "Competitions",
             newName: "Duels");

            migrationBuilder.RenameTable(
                name: "CompetitionSolutions",
                newName: "DuelSolutions"
                );

            migrationBuilder.DropForeignKey(
                name: "FK_CompetitionSolutions_Competitions_CompetitionId",
                table: "DuelSolutions");

            migrationBuilder.RenameColumn(
                name: "CompetitionId",
                table: "DuelSolutions",
                newName: "DuelId");

            migrationBuilder.RenameColumn(
                name: "CompetitionSolutionId",
                table: "DuelSolutions",
                newName: "DuelSolutionId");

            migrationBuilder.RenameIndex(
                name: "IX_CompetitionSolutions_UserEmail_CompetitionId",
                table: "DuelSolutions",
                newName: "IX_DuelSolutions_UserEmail_DuelId");

            migrationBuilder.RenameIndex(
                name: "IX_CompetitionSolutions_CompetitionId",
                table: "DuelSolutions",
                newName: "IX_DuelSolutions_DuelId");

            migrationBuilder.RenameColumn(
                name: "CompetitionId",
                table: "Duels",
                newName: "DuelId");

            migrationBuilder.AddForeignKey(
                name: "FK_DuelSolutions_Duels_DuelId",
                table: "DuelSolutions",
                column: "DuelId",
                principalTable: "Duels",
                principalColumn: "DuelId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
             name: "DuelSolutions",
             newName: "CompetitionSolutions");

            migrationBuilder.RenameTable(
                   name: "Duels",
                   newName: "Competitions");

            migrationBuilder.DropForeignKey(
                name: "FK_DuelSolutions_Duels_DuelId",
                table: "CompetitionSolutions");

            migrationBuilder.RenameColumn(
                name: "DuelId",
                table: "CompetitionSolutions",
                newName: "CompetitionId");

            migrationBuilder.RenameColumn(
                name: "DuelSolutionId",
                table: "CompetitionSolutions",
                newName: "CompetitionSolutionId");

            migrationBuilder.RenameIndex(
                name: "IX_DuelSolutions_UserEmail_DuelId",
                table: "CompetitionSolutions",
                newName: "IX_CompetitionSolutions_UserEmail_CompetitionId");

            migrationBuilder.RenameIndex(
                name: "IX_DuelSolutions_DuelId",
                table: "CompetitionSolutions",
                newName: "IX_CompetitionSolutions_CompetitionId");

            migrationBuilder.RenameColumn(
                name: "DuelId",
                table: "Competitions",
                newName: "CompetitionId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompetitionSolutions_Competitions_CompetitionId",
                table: "CompetitionSolutions",
                column: "CompetitionId",
                principalTable: "Competitions",
                principalColumn: "CompetitionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
