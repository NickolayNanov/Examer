using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineExamer.Data.Migrations
{
    public partial class AddedChangesToAnswer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuestionId1",
                table: "Answers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionId1",
                table: "Answers",
                column: "QuestionId1",
                unique: true,
                filter: "[QuestionId1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Questions_QuestionId1",
                table: "Answers",
                column: "QuestionId1",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Questions_QuestionId1",
                table: "Answers");

            migrationBuilder.DropIndex(
                name: "IX_Answers_QuestionId1",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "QuestionId1",
                table: "Answers");
        }
    }
}
