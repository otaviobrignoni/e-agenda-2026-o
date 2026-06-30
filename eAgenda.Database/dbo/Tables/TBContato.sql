CREATE TABLE [dbo].[TBContato] (
    [Id]       UNIQUEIDENTIFIER NOT NULL,
    [Nome]     NVARCHAR (100)   NOT NULL,
    [Email]    NVARCHAR (255)   NOT NULL,
    [Telefone] NVARCHAR (15)    NOT NULL,
    [Cargo]    NVARCHAR (50)    NULL,
    [Empresa]  NVARCHAR (50)    NULL,
    CONSTRAINT [PK_TBContato] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_TBContato_Email] UNIQUE NONCLUSTERED ([Email] ASC),
    CONSTRAINT [UQ_TBContato_Telefone] UNIQUE NONCLUSTERED ([Telefone] ASC)
);
GO

