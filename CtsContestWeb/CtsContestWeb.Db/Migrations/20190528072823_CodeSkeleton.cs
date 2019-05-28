using Microsoft.EntityFrameworkCore.Migrations;

namespace CtsContestWeb.Db.Migrations
{
    public partial class CodeSkeleton : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CodeSkeletons",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Language = table.Column<string>(nullable: true),
                    WriteLine = table.Column<string>(nullable: true),
                    ReadLine = table.Column<string>(nullable: true),
                    ReadInteger = table.Column<string>(nullable: true),
                    ReadLineOfIntegers = table.Column<string>(nullable: true),
                    ReadInputIntegerNumberOfLinesOfIntegers = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeSkeletons", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CodeSkeletons");
        }
    }
}
