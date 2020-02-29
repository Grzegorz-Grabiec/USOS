using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace USOS.Migrations
{
    public partial class lstmv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LessonStudentMark",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LessonID = table.Column<int>(nullable: true),
                    MarkID = table.Column<int>(nullable: true),
                    UsernameId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonStudentMark", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LessonStudentMark_Lesson_LessonID",
                        column: x => x.LessonID,
                        principalTable: "Lesson",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LessonStudentMark_Mark_MarkID",
                        column: x => x.MarkID,
                        principalTable: "Mark",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LessonStudentMark_AspNetUsers_UsernameId",
                        column: x => x.UsernameId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LessonStudentMark_LessonID",
                table: "LessonStudentMark",
                column: "LessonID");

            migrationBuilder.CreateIndex(
                name: "IX_LessonStudentMark_MarkID",
                table: "LessonStudentMark",
                column: "MarkID");

            migrationBuilder.CreateIndex(
                name: "IX_LessonStudentMark_UsernameId",
                table: "LessonStudentMark",
                column: "UsernameId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LessonStudentMark");
        }
    }
}
