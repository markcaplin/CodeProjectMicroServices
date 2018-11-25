IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [Accounts] (
    [AccountId] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [DateCreated] datetime2 NOT NULL,
    [DateUpdated] datetime2 NOT NULL,
    [PurchasedApplications] int NOT NULL,
    CONSTRAINT [PK_Accounts] PRIMARY KEY ([AccountId])
);

GO

CREATE TABLE [UserTypes] (
    [UserTypeId] int NOT NULL IDENTITY,
    [Description] nvarchar(max) NULL,
    CONSTRAINT [PK_UserTypes] PRIMARY KEY ([UserTypeId])
);

GO

CREATE TABLE [Users] (
    [UserId] int NOT NULL IDENTITY,
    [EmailAddress] nvarchar(450) NULL,
    [Password] nvarchar(max) NULL,
    [PasswordSalt] nvarchar(max) NULL,
    [FirstName] nvarchar(max) NULL,
    [LastName] nvarchar(max) NULL,
    [AccountId] int NOT NULL,
    [UserTypeId] int NOT NULL,
    [DateCreated] datetime2 NOT NULL,
    [DateUpdated] datetime2 NOT NULL,
    [DateLastLogin] datetime2 NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([UserId]),
    CONSTRAINT [FK_Users_Accounts_AccountId] FOREIGN KEY ([AccountId]) REFERENCES [Accounts] ([AccountId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Users_UserTypes_UserTypeId] FOREIGN KEY ([UserTypeId]) REFERENCES [UserTypes] ([UserTypeId]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_Users_AccountId] ON [Users] ([AccountId]);

GO

CREATE UNIQUE INDEX [IX_Users_EmailAddress] ON [Users] ([EmailAddress]) WHERE [EmailAddress] IS NOT NULL;

GO

CREATE INDEX [IX_Users_UserTypeId] ON [Users] ([UserTypeId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20180826141948_InitialDatabase', N'2.1.2-rtm-30932');

GO

 SET IDENTITY_INSERT UserTypes ON;  INSERT INTO UserTypes (UserTypeId, Description)  VALUES( 1, 'Administrator');  INSERT INTO UserTypes (UserTypeId, Description)  VALUES( 2, 'Non-Administrator');  SET IDENTITY_INSERT UserTypes OFF; 

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20180826142309_SeedUserTypes', N'2.1.2-rtm-30932');

GO

