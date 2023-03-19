﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyCoreBanking.API.Data;

#nullable disable

namespace MyCoreBanking.API.Migrations
{
    [DbContext(typeof(MeuDbContext))]
    [Migration("20230319034457_Add_TransacaoTipo")]
    partial class Add_TransacaoTipo
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("MyCoreBanking.API.Data.Entities.CartaoDeCreditoEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWSEQUENTIALID()");

                    b.Property<string>("Banco")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Bandeira")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NumerosFinais")
                        .IsRequired()
                        .HasMaxLength(4)
                        .HasColumnType("nvarchar(4)");

                    b.Property<Guid?>("UsuarioEntityId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UsuarioEntityId");

                    b.ToTable("CartaoDeCreditoEntity", (string)null);
                });

            modelBuilder.Entity("MyCoreBanking.API.Data.Entities.ContaCorrenteEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWSEQUENTIALID()");

                    b.Property<string>("Agencia")
                        .IsRequired()
                        .HasMaxLength(4)
                        .HasColumnType("nvarchar(4)");

                    b.Property<string>("Banco")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Conta")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<Guid?>("UsuarioEntityId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UsuarioEntityId");

                    b.ToTable("ContaCorrenteEntity", (string)null);
                });

            modelBuilder.Entity("MyCoreBanking.API.Data.Entities.MeioDePagamentoEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWSEQUENTIALID()");

                    b.Property<string>("Apelido")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("CriadoEm")
                        .HasColumnType("datetime2");

                    b.Property<string>("Observacao")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("Tipo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UltimaAtualizacaoEm")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UsuarioId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UsuarioId");

                    b.ToTable("MeioDePagamentoEntity", (string)null);
                });

            modelBuilder.Entity("MyCoreBanking.API.Data.Entities.TransacaoEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWSEQUENTIALID()");

                    b.Property<DateTime>("CriadoEm")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataPagamento")
                        .HasColumnType("datetime2");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<Guid>("MeioDePagamentoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Observacao")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("Tipo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UltimaAtualizacaoEm")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UsuarioId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Valor")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("MeioDePagamentoId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("TransacaoEntity", (string)null);
                });

            modelBuilder.Entity("MyCoreBanking.API.Data.Entities.UsuarioEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWSEQUENTIALID()");

                    b.Property<DateTime>("CriadoEm")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("SenhaHash")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<DateTime>("UltimaAtualizacaoEm")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("UsuarioEntity", (string)null);
                });

            modelBuilder.Entity("MyCoreBanking.API.Data.Entities.CartaoDeCreditoEntity", b =>
                {
                    b.HasOne("MyCoreBanking.API.Data.Entities.MeioDePagamentoEntity", "MeioDePagamento")
                        .WithOne("CartaoDeCredito")
                        .HasForeignKey("MyCoreBanking.API.Data.Entities.CartaoDeCreditoEntity", "Id")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("MyCoreBanking.API.Data.Entities.UsuarioEntity", null)
                        .WithMany("CartoesDeCredito")
                        .HasForeignKey("UsuarioEntityId");

                    b.Navigation("MeioDePagamento");
                });

            modelBuilder.Entity("MyCoreBanking.API.Data.Entities.ContaCorrenteEntity", b =>
                {
                    b.HasOne("MyCoreBanking.API.Data.Entities.MeioDePagamentoEntity", "MeioDePagamento")
                        .WithOne("ContaCorrente")
                        .HasForeignKey("MyCoreBanking.API.Data.Entities.ContaCorrenteEntity", "Id")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("MyCoreBanking.API.Data.Entities.UsuarioEntity", null)
                        .WithMany("ContasCorrente")
                        .HasForeignKey("UsuarioEntityId");

                    b.Navigation("MeioDePagamento");
                });

            modelBuilder.Entity("MyCoreBanking.API.Data.Entities.MeioDePagamentoEntity", b =>
                {
                    b.HasOne("MyCoreBanking.API.Data.Entities.UsuarioEntity", "Usuario")
                        .WithMany("MeiosDePagamento")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("MyCoreBanking.API.Data.Entities.TransacaoEntity", b =>
                {
                    b.HasOne("MyCoreBanking.API.Data.Entities.MeioDePagamentoEntity", "MeioDePagamento")
                        .WithMany("Transacoes")
                        .HasForeignKey("MeioDePagamentoId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("MyCoreBanking.API.Data.Entities.UsuarioEntity", "Usuario")
                        .WithMany("Transacoes")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("MeioDePagamento");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("MyCoreBanking.API.Data.Entities.MeioDePagamentoEntity", b =>
                {
                    b.Navigation("CartaoDeCredito");

                    b.Navigation("ContaCorrente");

                    b.Navigation("Transacoes");
                });

            modelBuilder.Entity("MyCoreBanking.API.Data.Entities.UsuarioEntity", b =>
                {
                    b.Navigation("CartoesDeCredito");

                    b.Navigation("ContasCorrente");

                    b.Navigation("MeiosDePagamento");

                    b.Navigation("Transacoes");
                });
#pragma warning restore 612, 618
        }
    }
}
