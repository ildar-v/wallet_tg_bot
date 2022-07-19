using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hello.Ildar.Bot.DataAccess.Migrations
{
    public partial class TelegramUser_Add_Records_and_LastRecordId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LastRecordId",
                table: "Users",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastRecordId",
                table: "Users");
        }
    }
}
