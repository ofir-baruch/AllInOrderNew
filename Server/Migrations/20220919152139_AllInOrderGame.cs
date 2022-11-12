using Microsoft.EntityFrameworkCore.Migrations;

namespace AllinOrder_Shahaf_Ofir_Snir.Server.Migrations
{
    public partial class AllInOrderGame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ItemTime",
                table: "Games",
                newName: "QuestionTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QuestionTime",
                table: "Games",
                newName: "ItemTime");
        }
    }
}
