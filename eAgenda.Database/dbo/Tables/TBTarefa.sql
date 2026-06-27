CREATE TABLE [dbo].[TBTarefa] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
    [Titulo]        NVARCHAR (100)   NOT NULL,
    [Prioridade]    INT              NOT NULL,
    [DataCriacao]   DATETIME2 (0)    NOT NULL,
    [DataConclusao] DATETIME2 (0)    NULL,
    CONSTRAINT [PK_TBTarefa] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

