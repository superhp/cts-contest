using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CtsContestWeb.Db.Migrations
{
    public partial class TasksInputsOutputs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    TaskId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    InputType = table.Column<string>(nullable: true),
                    Value = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.TaskId);
                });

            migrationBuilder.CreateTable(
                name: "TaskInputs",
                columns: table => new
                {
                    TaskInputId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TaskId = table.Column<int>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    IsSample = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskInputs", x => x.TaskInputId);
                    table.ForeignKey(
                        name: "FK_TaskInputs_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "TaskId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskOutputs",
                columns: table => new
                {
                    TaskOutputId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TaskInputId = table.Column<int>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskOutputs", x => x.TaskOutputId);
                    table.ForeignKey(
                        name: "FK_TaskOutputs_TaskInputs_TaskInputId",
                        column: x => x.TaskInputId,
                        principalTable: "TaskInputs",
                        principalColumn: "TaskInputId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskInputs_TaskId",
                table: "TaskInputs",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskOutputs_TaskInputId",
                table: "TaskOutputs",
                column: "TaskInputId",
                unique: true);

            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskOutputs");

            migrationBuilder.DropTable(
                name: "TaskInputs");

            migrationBuilder.DropTable(
                name: "Tasks");
        }
    }
}
