using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyCoreBanking.API.Migrations
{
    public partial class EntityNameRefactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartaoDeCredito");

            migrationBuilder.DropTable(
                name: "ContaCorrente");

            migrationBuilder.DropTable(
                name: "Transacao");

            migrationBuilder.DropTable(
                name: "MeioDePagamento");

            migrationBuilder.DropTable(
                name: "Usuario");

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
                name: "MeioDePagamentoEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Apelido = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Observacao = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UltimaAtualizacaoEm = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeioDePagamentoEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MeioDePagamentoEntity_UsuarioEntity_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "UsuarioEntity",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CartaoDeCreditoEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    NumerosFinais = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Banco = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bandeira = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsuarioEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartaoDeCreditoEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartaoDeCreditoEntity_MeioDePagamentoEntity_Id",
                        column: x => x.Id,
                        principalTable: "MeioDePagamentoEntity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CartaoDeCreditoEntity_UsuarioEntity_UsuarioEntityId",
                        column: x => x.UsuarioEntityId,
                        principalTable: "UsuarioEntity",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ContaCorrenteEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Banco = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Agencia = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Conta = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UsuarioEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContaCorrenteEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContaCorrenteEntity_MeioDePagamentoEntity_Id",
                        column: x => x.Id,
                        principalTable: "MeioDePagamentoEntity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ContaCorrenteEntity_UsuarioEntity_UsuarioEntityId",
                        column: x => x.UsuarioEntityId,
                        principalTable: "UsuarioEntity",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TransacaoEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Descricao = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Observacao = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DataPagamento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MeioDePagamentoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UltimaAtualizacaoEm = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransacaoEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransacaoEntity_MeioDePagamentoEntity_MeioDePagamentoId",
                        column: x => x.MeioDePagamentoId,
                        principalTable: "MeioDePagamentoEntity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransacaoEntity_UsuarioEntity_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "UsuarioEntity",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartaoDeCreditoEntity_UsuarioEntityId",
                table: "CartaoDeCreditoEntity",
                column: "UsuarioEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ContaCorrenteEntity_UsuarioEntityId",
                table: "ContaCorrenteEntity",
                column: "UsuarioEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_MeioDePagamentoEntity_UsuarioId",
                table: "MeioDePagamentoEntity",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_TransacaoEntity_MeioDePagamentoId",
                table: "TransacaoEntity",
                column: "MeioDePagamentoId");

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
                name: "CartaoDeCreditoEntity");

            migrationBuilder.DropTable(
                name: "ContaCorrenteEntity");

            migrationBuilder.DropTable(
                name: "TransacaoEntity");

            migrationBuilder.DropTable(
                name: "MeioDePagamentoEntity");

            migrationBuilder.DropTable(
                name: "UsuarioEntity");

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    SenhaHash = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    UltimaAtualizacaoEm = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MeioDePagamento",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Apelido = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Observacao = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UltimaAtualizacaoEm = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeioDePagamento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MeioDePagamento_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CartaoDeCredito",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Banco = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bandeira = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumerosFinais = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartaoDeCredito", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartaoDeCredito_MeioDePagamento_Id",
                        column: x => x.Id,
                        principalTable: "MeioDePagamento",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CartaoDeCredito_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ContaCorrente",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Agencia = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Banco = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Conta = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContaCorrente", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContaCorrente_MeioDePagamento_Id",
                        column: x => x.Id,
                        principalTable: "MeioDePagamento",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ContaCorrente_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Transacao",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    MeioDePagamentoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataPagamento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Observacao = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    UltimaAtualizacaoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transacao_MeioDePagamento_MeioDePagamentoId",
                        column: x => x.MeioDePagamentoId,
                        principalTable: "MeioDePagamento",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transacao_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartaoDeCredito_UsuarioId",
                table: "CartaoDeCredito",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_ContaCorrente_UsuarioId",
                table: "ContaCorrente",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_MeioDePagamento_UsuarioId",
                table: "MeioDePagamento",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Transacao_MeioDePagamentoId",
                table: "Transacao",
                column: "MeioDePagamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Transacao_UsuarioId",
                table: "Transacao",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_Email",
                table: "Usuario",
                column: "Email",
                unique: true);
        }
    }
}
