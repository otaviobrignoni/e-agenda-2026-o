CREATE TABLE [dbo].[TBCategoria] (
    [Id]     UNIQUEIDENTIFIER NOT NULL,
    [Titulo] NVARCHAR (100)   NOT NULL
);
GO

ALTER TABLE [dbo].[TBCategoria]
    ADD CONSTRAINT [PK_TBCategoria] PRIMARY KEY CLUSTERED ([Id] ASC);
GO

ALTER TABLE [dbo].[TBCategoria]
    ADD CONSTRAINT [UQ_TBCategoria_Titulo] UNIQUE NONCLUSTERED ([Titulo] ASC);
GO

