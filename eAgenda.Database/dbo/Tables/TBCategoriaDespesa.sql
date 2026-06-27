CREATE TABLE [dbo].[TBCategoriaDespesa] (
    [CategoriaId] UNIQUEIDENTIFIER NOT NULL,
    [DespesaId]   UNIQUEIDENTIFIER NOT NULL
);
GO

ALTER TABLE [dbo].[TBCategoriaDespesa]
    ADD CONSTRAINT [FK_TBCategoriaDespesa_TBDespesa] FOREIGN KEY ([DespesaId]) REFERENCES [dbo].[TBDespesa] ([Id]);
GO

ALTER TABLE [dbo].[TBCategoriaDespesa]
    ADD CONSTRAINT [FK_TBCategoriaDespesa_TBCategoria] FOREIGN KEY ([CategoriaId]) REFERENCES [dbo].[TBCategoria] ([Id]);
GO

ALTER TABLE [dbo].[TBCategoriaDespesa]
    ADD CONSTRAINT [PK_TBCategoriaDespesa] PRIMARY KEY CLUSTERED ([CategoriaId] ASC, [DespesaId] ASC);
GO

