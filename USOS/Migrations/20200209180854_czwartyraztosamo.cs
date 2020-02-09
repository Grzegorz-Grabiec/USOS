using Microsoft.EntityFrameworkCore.Migrations;

namespace USOS.Migrations
{
    public partial class czwartyraztosamo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Header",
                table: "News",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Header",
                table: "News");
        }
    }
}
