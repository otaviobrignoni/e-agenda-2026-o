CREATE TABLE [dbo].[TBCompromisso] (
    [Id]          UNIQUEIDENTIFIER NOT NULL,
    [Assunto]     NVARCHAR (100)   NOT NULL,
    [Data]        DATE             NOT NULL,
    [HoraInicio]  TIME (0)         NOT NULL,
    [HoraTermino] TIME (0)         NOT NULL,
    [Tipo]        INT              NOT NULL,
    [LocalOuLink] NVARCHAR (255)   NOT NULL,
    [ContatoId]   UNIQUEIDENTIFIER NULL
);
GO

ALTER TABLE [dbo].[TBCompromisso]
    ADD CONSTRAINT [CK_TBCompromisso_HorarioValido] CHECK ([HoraTermino]>[HoraInicio]);
GO

ALTER TABLE [dbo].[TBCompromisso]
    ADD CONSTRAINT [PK_TBCompromisso] PRIMARY KEY CLUSTERED ([Id] ASC);
GO

ALTER TABLE [dbo].[TBCompromisso]
    ADD CONSTRAINT [FK_TBCompromisso_TBContato] FOREIGN KEY ([ContatoId]) REFERENCES [dbo].[TBContato] ([Id]);
GO

