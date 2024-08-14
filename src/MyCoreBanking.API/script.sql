IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [UsuarioEntity] (
    [Id] uniqueidentifier NOT NULL DEFAULT (NEWSEQUENTIALID()),
    [Nome] nvarchar(250) NOT NULL,
    [Email] nvarchar(250) NOT NULL,
    [SenhaHash] nvarchar(250) NOT NULL,
    [CriadoEm] datetime2 NOT NULL,
    [UltimaAtualizacaoEm] datetime2 NOT NULL,
    CONSTRAINT [PK_UsuarioEntity] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [ContaEntity] (
    [Id] uniqueidentifier NOT NULL DEFAULT (NEWSEQUENTIALID()),
    [Saldo] decimal(18,2) NOT NULL,
    [Banco] nvarchar(max) NOT NULL,
    [Descricao] nvarchar(100) NOT NULL,
    [Tipo] nvarchar(max) NOT NULL,
    [UsuarioId] uniqueidentifier NOT NULL,
    [CriadoEm] datetime2 NOT NULL,
    [UltimaAtualizacaoEm] datetime2 NOT NULL,
    CONSTRAINT [PK_ContaEntity] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ContaEntity_UsuarioEntity_UsuarioId] FOREIGN KEY ([UsuarioId]) REFERENCES [UsuarioEntity] ([Id])
);
GO

CREATE TABLE [TransacaoEntity] (
    [Id] uniqueidentifier NOT NULL DEFAULT (NEWSEQUENTIALID()),
    [Descricao] nvarchar(100) NOT NULL,
    [Observacao] nvarchar(250) NULL,
    [Valor] decimal(18,2) NOT NULL,
    [DataEfetivacao] datetime2 NULL,
    [DataTransacao] datetime2 NOT NULL,
    [TipoOperacao] nvarchar(max) NOT NULL,
    [TipoTransacao] nvarchar(max) NOT NULL,
    [MeioPagamento] nvarchar(max) NOT NULL,
    [Categoria] nvarchar(max) NOT NULL,
    [ReferenciaParcelaId] uniqueidentifier NULL,
    [ParcelaAtual] int NULL,
    [NumeroParcelas] int NULL,
    [UsuarioId] uniqueidentifier NOT NULL,
    [ContaId] uniqueidentifier NOT NULL,
    [CriadoEm] datetime2 NOT NULL,
    [UltimaAtualizacaoEm] datetime2 NOT NULL,
    CONSTRAINT [PK_TransacaoEntity] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TransacaoEntity_ContaEntity_ContaId] FOREIGN KEY ([ContaId]) REFERENCES [ContaEntity] ([Id]),
    CONSTRAINT [FK_TransacaoEntity_UsuarioEntity_UsuarioId] FOREIGN KEY ([UsuarioId]) REFERENCES [UsuarioEntity] ([Id])
);
GO

CREATE INDEX [IX_ContaEntity_UsuarioId] ON [ContaEntity] ([UsuarioId]);
GO

CREATE INDEX [IX_TransacaoEntity_ContaId] ON [TransacaoEntity] ([ContaId]);
GO

CREATE INDEX [IX_TransacaoEntity_UsuarioId] ON [TransacaoEntity] ([UsuarioId]);
GO

CREATE UNIQUE INDEX [IX_UsuarioEntity_Email] ON [UsuarioEntity] ([Email]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230406030336_Initial', N'6.0.15');
GO

COMMIT;
GO


