using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineExamer.Data.Migrations
{
    public partial class AddedUrlToQuestions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImgUrl",
                table: "Questions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgUrl",
                table: "Questions");
        }
    }
}
