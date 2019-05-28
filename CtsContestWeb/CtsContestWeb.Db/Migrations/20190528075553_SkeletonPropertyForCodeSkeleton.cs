using Microsoft.EntityFrameworkCore.Migrations;

namespace CtsContestWeb.Db.Migrations
{
    public partial class SkeletonPropertyForCodeSkeleton : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Skeleton",
                table: "CodeSkeletons",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Skeleton",
                table: "CodeSkeletons");
        }
    }
}
