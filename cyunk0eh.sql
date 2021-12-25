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

CREATE TABLE [CurrencyModel] (
    [Id] nvarchar(450) NOT NULL,
    [Date] datetime2 NOT NULL,
    [CharValute] nvarchar(max) NOT NULL,
    [NominalValute] int NOT NULL,
    [NameValute] nvarchar(max) NOT NULL,
    [Value] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_CurrencyModel] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20211219151527_m1', N'6.0.1');
GO

COMMIT;
GO

