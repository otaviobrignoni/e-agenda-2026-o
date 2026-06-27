CREATE TABLE [dbo].[TBItemTarefa] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
    [Titulo]        NVARCHAR (100)   NOT NULL,
    [EstaConcluida] BIT              NULL,
    [TarefaId]      UNIQUEIDENTIFIER NOT NULL
);
GO

ALTER TABLE [dbo].[TBItemTarefa]
    ADD CONSTRAINT [FK_TBItemTarefa_TBTarefa] FOREIGN KEY ([TarefaId]) REFERENCES [dbo].[TBTarefa] ([Id]) ON DELETE CASCADE;
GO

ALTER TABLE [dbo].[TBItemTarefa]
    ADD CONSTRAINT [PK_TBItemTarefa] PRIMARY KEY CLUSTERED ([Id] ASC);
GO

