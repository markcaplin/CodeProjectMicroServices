IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [Products] (
    [AccountId] int NOT NULL,
    [ProductId] int NOT NULL IDENTITY,
    [ProductNumber] nvarchar(450) NULL,
    [Description] nvarchar(max) NULL,
    [BinLocation] nvarchar(max) NULL,
    [UnitPrice] float NOT NULL,
    [AverageCost] float NOT NULL,
    [OnHandQuantity] int NOT NULL,
    [OnOrderQuantity] int NOT NULL,
    [CommittedQuantity] int NOT NULL,
    [DateCreated] datetime2 NOT NULL,
    [DateUpdated] datetime2 NOT NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY ([ProductId])
);

GO

CREATE TABLE [TransactionQueue] (
    [TransactionQueueId] int NOT NULL IDENTITY,
    [TransactionCode] nvarchar(max) NULL,
    [Payload] nvarchar(max) NULL,
    [SentToExchange] bit NOT NULL,
    [DateCreated] datetime2 NOT NULL,
    [DateSentToExchange] datetime2 NOT NULL,
    [DateToResendToExchange] datetime2 NOT NULL,
    CONSTRAINT [PK_TransactionQueue] PRIMARY KEY ([TransactionQueueId])
);

GO

CREATE INDEX [IX_Products_ProductNumber] ON [Products] ([ProductNumber]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20180826235151_InitialDatabase', N'2.1.2-rtm-30932');

GO

