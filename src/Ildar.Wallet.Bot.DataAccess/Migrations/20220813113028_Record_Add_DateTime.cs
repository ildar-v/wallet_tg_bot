using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ildar.Wallet.Bot.DataAccess.Migrations
{
    public partial class Record_Add_DateTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateTime",
                table: "Records",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2022, 8, 13, 0, 0, 0, 0, DateTimeKind.Utc));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateTime",
                table: "Records");
        }
    }
}
