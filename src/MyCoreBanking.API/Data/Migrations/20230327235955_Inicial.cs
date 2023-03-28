using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyCoreBanking.API.Migrations
{
    public partial class Inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsuarioEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Nome = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    SenhaHash = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UltimaAtualizacaoEm = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContaEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Saldo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Banco = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UltimaAtualizacaoEm = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContaEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContaEntity_UsuarioEntity_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "UsuarioEntity",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TransacaoEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Descricao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Observacao = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DataDeEfetivacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataDaTransacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TipoDeOperacao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoDeTransacao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MeioDePagamento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Categoria = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReferenciaParcelaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DataVencimento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NumeroParcelas = table.Column<int>(type: "int", nullable: true),
                    ValorParcela = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UltimaAtualizacaoEm = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransacaoEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransacaoEntity_ContaEntity_ContaId",
                        column: x => x.ContaId,
                        principalTable: "ContaEntity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransacaoEntity_UsuarioEntity_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "UsuarioEntity",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContaEntity_UsuarioId",
                table: "ContaEntity",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_TransacaoEntity_ContaId",
                table: "TransacaoEntity",
                column: "ContaId");

            migrationBuilder.CreateIndex(
                name: "IX_TransacaoEntity_UsuarioId",
                table: "TransacaoEntity",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioEntity_Email",
                table: "UsuarioEntity",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransacaoEntity");

            migrationBuilder.DropTable(
                name: "ContaEntity");

            migrationBuilder.DropTable(
                name: "UsuarioEntity");
        }
    }
}
