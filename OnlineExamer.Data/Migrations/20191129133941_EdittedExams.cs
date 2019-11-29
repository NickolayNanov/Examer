using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineExamer.Data.Migrations
{
    public partial class EdittedExams : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinishedAt",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "StartedAt",
                table: "Exams");

            migrationBuilder.AddColumn<DateTime>(
                name: "FinishedAt",
                table: "UserExams",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartedAt",
                table: "UserExams",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinishedAt",
                table: "UserExams");

            migrationBuilder.DropColumn(
                name: "StartedAt",
                table: "UserExams");

            migrationBuilder.AddColumn<DateTime>(
                name: "FinishedAt",
                table: "Exams",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartedAt",
                table: "Exams",
                type: "datetime2",
                nullable: true);
        }
    }
}
