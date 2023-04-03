using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyCoreBanking.API.Migrations
{
    public partial class UpdateDataTransacao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataVencimento",
                table: "TransacaoEntity");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataVencimento",
                table: "TransacaoEntity",
                type: "datetime2",
                nullable: true);
        }
    }
}
