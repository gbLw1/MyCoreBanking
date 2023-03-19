using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyCoreBanking.API.Migrations
{
    public partial class Add_TransacaoTipo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Tipo",
                table: "TransacaoEntity",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "TransacaoEntity");
        }
    }
}
