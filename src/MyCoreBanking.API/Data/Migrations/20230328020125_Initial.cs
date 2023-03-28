using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyCoreBanking.API.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TipoDeTransacao",
                table: "TransacaoEntity",
                newName: "TipoTransacao");

            migrationBuilder.RenameColumn(
                name: "TipoDeOperacao",
                table: "TransacaoEntity",
                newName: "TipoOperacao");

            migrationBuilder.RenameColumn(
                name: "MeioDePagamento",
                table: "TransacaoEntity",
                newName: "MeioPagamento");

            migrationBuilder.RenameColumn(
                name: "DataDeEfetivacao",
                table: "TransacaoEntity",
                newName: "DataEfetivacao");

            migrationBuilder.RenameColumn(
                name: "DataDaTransacao",
                table: "TransacaoEntity",
                newName: "DataTransacao");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TipoTransacao",
                table: "TransacaoEntity",
                newName: "TipoDeTransacao");

            migrationBuilder.RenameColumn(
                name: "TipoOperacao",
                table: "TransacaoEntity",
                newName: "TipoDeOperacao");

            migrationBuilder.RenameColumn(
                name: "MeioPagamento",
                table: "TransacaoEntity",
                newName: "MeioDePagamento");

            migrationBuilder.RenameColumn(
                name: "DataTransacao",
                table: "TransacaoEntity",
                newName: "DataDaTransacao");

            migrationBuilder.RenameColumn(
                name: "DataEfetivacao",
                table: "TransacaoEntity",
                newName: "DataDeEfetivacao");
        }
    }
}
