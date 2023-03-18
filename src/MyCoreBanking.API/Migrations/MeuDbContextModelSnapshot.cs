﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyCoreBanking.API.Data;

#nullable disable

namespace MyCoreBanking.API.Migrations
{
    [DbContext(typeof(MeuDbContext))]
    partial class MeuDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("MyCoreBanking.API.Data.Entities.CartaoDeCredito", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

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

                    b.Property<Guid?>("UsuarioId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UsuarioId");

                    b.ToTable("CartaoDeCredito", (string)null);
                });

            modelBuilder.Entity("MyCoreBanking.API.Data.Entities.ContaCorrente", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

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

                    b.Property<Guid?>("UsuarioId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UsuarioId");

                    b.ToTable("ContaCorrente", (string)null);
                });

            modelBuilder.Entity("MyCoreBanking.API.Data.Entities.MeioDePagamento", b =>
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

                    b.ToTable("MeioDePagamento", (string)null);
                });

            modelBuilder.Entity("MyCoreBanking.API.Data.Entities.Transacao", b =>
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

                    b.Property<DateTime>("UltimaAtualizacaoEm")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UsuarioId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Valor")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("MeioDePagamentoId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Transacao", (string)null);
                });

            modelBuilder.Entity("MyCoreBanking.API.Data.Entities.Usuario", b =>
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

                    b.ToTable("Usuario", (string)null);
                });

            modelBuilder.Entity("MyCoreBanking.API.Data.Entities.CartaoDeCredito", b =>
                {
                    b.HasOne("MyCoreBanking.API.Data.Entities.MeioDePagamento", "MeioDePagamento")
                        .WithOne("CartaoDeCredito")
                        .HasForeignKey("MyCoreBanking.API.Data.Entities.CartaoDeCredito", "Id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("MyCoreBanking.API.Data.Entities.Usuario", null)
                        .WithMany("CartoesDeCredito")
                        .HasForeignKey("UsuarioId");

                    b.Navigation("MeioDePagamento");
                });

            modelBuilder.Entity("MyCoreBanking.API.Data.Entities.ContaCorrente", b =>
                {
                    b.HasOne("MyCoreBanking.API.Data.Entities.MeioDePagamento", "MeioDePagamento")
                        .WithOne("ContaCorrente")
                        .HasForeignKey("MyCoreBanking.API.Data.Entities.ContaCorrente", "Id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("MyCoreBanking.API.Data.Entities.Usuario", null)
                        .WithMany("ContasCorrente")
                        .HasForeignKey("UsuarioId");

                    b.Navigation("MeioDePagamento");
                });

            modelBuilder.Entity("MyCoreBanking.API.Data.Entities.MeioDePagamento", b =>
                {
                    b.HasOne("MyCoreBanking.API.Data.Entities.Usuario", "Usuario")
                        .WithMany("MeiosDePagamento")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("MyCoreBanking.API.Data.Entities.Transacao", b =>
                {
                    b.HasOne("MyCoreBanking.API.Data.Entities.MeioDePagamento", "MeioDePagamento")
                        .WithMany("Transacoes")
                        .HasForeignKey("MeioDePagamentoId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("MyCoreBanking.API.Data.Entities.Usuario", "Usuario")
                        .WithMany("Transacoes")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("MeioDePagamento");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("MyCoreBanking.API.Data.Entities.MeioDePagamento", b =>
                {
                    b.Navigation("CartaoDeCredito");

                    b.Navigation("ContaCorrente");

                    b.Navigation("Transacoes");
                });

            modelBuilder.Entity("MyCoreBanking.API.Data.Entities.Usuario", b =>
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
