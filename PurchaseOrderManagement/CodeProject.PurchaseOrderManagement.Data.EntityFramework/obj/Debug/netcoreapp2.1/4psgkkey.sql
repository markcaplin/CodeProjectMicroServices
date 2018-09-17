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
    [OnOrderQuantity] int NOT NULL,
    [DateCreated] datetime2 NOT NULL,
    [DateUpdated] datetime2 NOT NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY ([ProductId])
);

GO

CREATE TABLE [PurchaseOrderStatuses] (
    [PurchaseOrderStatusId] int NOT NULL IDENTITY,
    [Description] int NOT NULL,
    CONSTRAINT [PK_PurchaseOrderStatuses] PRIMARY KEY ([PurchaseOrderStatusId])
);

GO

CREATE TABLE [Suppliers] (
    [SupplierId] int NOT NULL IDENTITY,
    [AccountId] int NOT NULL,
    [Name] nvarchar(max) NULL,
    [AddressLine1] nvarchar(max) NULL,
    [AddressLine2] nvarchar(max) NULL,
    [City] nvarchar(max) NULL,
    [Region] nvarchar(max) NULL,
    [PostalCode] nvarchar(max) NULL,
    [DateCreated] datetime2 NOT NULL,
    [DateUpdated] datetime2 NOT NULL,
    CONSTRAINT [PK_Suppliers] PRIMARY KEY ([SupplierId])
);

GO

CREATE TABLE [TransactionQueueInbound] (
    [TransactionQueueInboundId] int NOT NULL IDENTITY,
    [SenderTransactionQueueId] int NOT NULL,
    [TransactionCode] nvarchar(max) NULL,
    [Payload] nvarchar(max) NULL,
    [ExchangeName] nvarchar(max) NULL,
    [DateCreated] datetime2 NOT NULL,
    CONSTRAINT [PK_TransactionQueueInbound] PRIMARY KEY ([TransactionQueueInboundId])
);

GO

CREATE TABLE [TransactionQueueInboundHistory] (
    [TransactionQueueInboundHistoryId] int NOT NULL IDENTITY,
    [TransactionQueueInboundId] int NOT NULL,
    [SenderTransactionQueueId] int NOT NULL,
    [TransactionCode] nvarchar(max) NULL,
    [Payload] nvarchar(max) NULL,
    [ExchangeName] nvarchar(max) NULL,
    [ProcessedSuccessfully] bit NOT NULL,
    [DuplicateMessage] bit NOT NULL,
    [ErrorMessage] nvarchar(max) NULL,
    [DateCreatedInbound] datetime2 NOT NULL,
    [DateCreated] datetime2 NOT NULL,
    CONSTRAINT [PK_TransactionQueueInboundHistory] PRIMARY KEY ([TransactionQueueInboundHistoryId])
);

GO

CREATE TABLE [TransactionQueueOutbound] (
    [TransactionQueueOutboundId] int NOT NULL IDENTITY,
    [TransactionCode] nvarchar(max) NULL,
    [Payload] nvarchar(max) NULL,
    [ExchangeName] nvarchar(max) NULL,
    [SentToExchange] bit NOT NULL,
    [DateCreated] datetime2 NOT NULL,
    [DateSentToExchange] datetime2 NOT NULL,
    [DateToResendToExchange] datetime2 NOT NULL,
    CONSTRAINT [PK_TransactionQueueOutbound] PRIMARY KEY ([TransactionQueueOutboundId])
);

GO

CREATE TABLE [TransactionQueueSemaphores] (
    [TransactionQueueSemaphoreId] int NOT NULL IDENTITY,
    [SemaphoreKey] nvarchar(450) NULL,
    [DateCreated] datetime2 NOT NULL,
    [DateUpdated] datetime2 NOT NULL,
    CONSTRAINT [PK_TransactionQueueSemaphores] PRIMARY KEY ([TransactionQueueSemaphoreId])
);

GO

CREATE TABLE [PurchaseOrders] (
    [PurchaseOrderId] int NOT NULL IDENTITY,
    [PurchaseOrderNumber] int NOT NULL,
    [AccountId] int NOT NULL,
    [SupplierId] int NOT NULL,
    [OrderTotal] float NOT NULL,
    [PurchaseOrderStatusId] int NOT NULL,
    [DateCreated] datetime2 NOT NULL,
    [DateUpdated] datetime2 NOT NULL,
    CONSTRAINT [PK_PurchaseOrders] PRIMARY KEY ([PurchaseOrderId]),
    CONSTRAINT [FK_PurchaseOrders_PurchaseOrderStatuses_PurchaseOrderStatusId] FOREIGN KEY ([PurchaseOrderStatusId]) REFERENCES [PurchaseOrderStatuses] ([PurchaseOrderStatusId]) ON DELETE CASCADE,
    CONSTRAINT [FK_PurchaseOrders_Suppliers_SupplierId] FOREIGN KEY ([SupplierId]) REFERENCES [Suppliers] ([SupplierId]) ON DELETE CASCADE
);

GO

CREATE TABLE [PurchaseOrderDetails] (
    [PurchaseOrderDetailId] int NOT NULL IDENTITY,
    [PurchaseOrderId] int NOT NULL,
    [ProductId] int NOT NULL,
    [UnitPrice] float NOT NULL,
    [OrderQuantity] int NOT NULL,
    [DateCreated] datetime2 NOT NULL,
    [DateUpdated] datetime2 NOT NULL,
    CONSTRAINT [PK_PurchaseOrderDetails] PRIMARY KEY ([PurchaseOrderDetailId]),
    CONSTRAINT [FK_PurchaseOrderDetails_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([ProductId]) ON DELETE CASCADE,
    CONSTRAINT [FK_PurchaseOrderDetails_PurchaseOrders_PurchaseOrderId] FOREIGN KEY ([PurchaseOrderId]) REFERENCES [PurchaseOrders] ([PurchaseOrderId]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_Products_ProductNumber] ON [Products] ([ProductNumber]);

GO

CREATE INDEX [IX_PurchaseOrderDetails_ProductId] ON [PurchaseOrderDetails] ([ProductId]);

GO

CREATE INDEX [IX_PurchaseOrderDetails_PurchaseOrderId] ON [PurchaseOrderDetails] ([PurchaseOrderId]);

GO

CREATE INDEX [IX_PurchaseOrders_PurchaseOrderStatusId] ON [PurchaseOrders] ([PurchaseOrderStatusId]);

GO

CREATE INDEX [IX_PurchaseOrders_SupplierId] ON [PurchaseOrders] ([SupplierId]);

GO

CREATE UNIQUE INDEX [IX_TransactionQueueSemaphores_SemaphoreKey] ON [TransactionQueueSemaphores] ([SemaphoreKey]) WHERE [SemaphoreKey] IS NOT NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20180916031613_InitialDatabase', N'2.1.2-rtm-30932');

GO

