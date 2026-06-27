CREATE TABLE [dbo].[TBDespesa] (
    [Id]             UNIQUEIDENTIFIER NOT NULL,
    [Descricao]      NVARCHAR (100)   NOT NULL,
    [Data]           DATETIME2 (0)    NOT NULL,
    [Valor]          DECIMAL (18, 2)  NOT NULL,
    [FormaPagamento] INT              NOT NULL
);
GO

ALTER TABLE [dbo].[TBDespesa]
    ADD CONSTRAINT [PK_TBDespesa] PRIMARY KEY CLUSTERED ([Id] ASC);
GO

