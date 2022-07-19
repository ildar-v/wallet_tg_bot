using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ildar.Wallet.Bot.DataAccess.Migrations
{
    public partial class UserId_To_TelegramUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Records");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Records",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
