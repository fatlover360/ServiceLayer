
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 03/28/2017 19:32:45
-- Generated from EDMX file: C:\Git\ServiceLayer\ServiceLayerNew\ModelMyHealth.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [MyHealthDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_FrequenciaCardiacaValoresAvisoFrequenciaCardiaca]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AvisoFrequenciaCardiacaSet] DROP CONSTRAINT [FK_FrequenciaCardiacaValoresAvisoFrequenciaCardiaca];
GO
IF OBJECT_ID(N'[dbo].[FK_AvisoSaturacaoSaturacaoValores]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AvisoSaturacaoSet] DROP CONSTRAINT [FK_AvisoSaturacaoSaturacaoValores];
GO
IF OBJECT_ID(N'[dbo].[FK_AvisoPressaoSanguineaPressaoSanguineaValores]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AvisoPressaoSanguineaSet] DROP CONSTRAINT [FK_AvisoPressaoSanguineaPressaoSanguineaValores];
GO
IF OBJECT_ID(N'[dbo].[FK_AvisoFrequenciaCardiacaTipoAviso]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AvisoFrequenciaCardiacaSet] DROP CONSTRAINT [FK_AvisoFrequenciaCardiacaTipoAviso];
GO
IF OBJECT_ID(N'[dbo].[FK_AvisoSaturacaoTipoAviso]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AvisoSaturacaoSet] DROP CONSTRAINT [FK_AvisoSaturacaoTipoAviso];
GO
IF OBJECT_ID(N'[dbo].[FK_AvisoPressaoSanguineaTipoAviso]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AvisoPressaoSanguineaSet] DROP CONSTRAINT [FK_AvisoPressaoSanguineaTipoAviso];
GO
IF OBJECT_ID(N'[dbo].[FK_SaturacaoValoresUtente]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SaturacaoValoresSet] DROP CONSTRAINT [FK_SaturacaoValoresUtente];
GO
IF OBJECT_ID(N'[dbo].[FK_FrequenciaCardiacaValoresUtente]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FrequenciaCardiacaValoresSet] DROP CONSTRAINT [FK_FrequenciaCardiacaValoresUtente];
GO
IF OBJECT_ID(N'[dbo].[FK_PressaoSanguineaValoresUtente]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PressaoSanguineaValoresSet] DROP CONSTRAINT [FK_PressaoSanguineaValoresUtente];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[ConfiguracoesLimitesSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ConfiguracoesLimitesSet];
GO
IF OBJECT_ID(N'[dbo].[FrequenciaCardiacaValoresSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FrequenciaCardiacaValoresSet];
GO
IF OBJECT_ID(N'[dbo].[PressaoSanguineaValoresSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PressaoSanguineaValoresSet];
GO
IF OBJECT_ID(N'[dbo].[SaturacaoValoresSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SaturacaoValoresSet];
GO
IF OBJECT_ID(N'[dbo].[UtenteSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UtenteSet];
GO
IF OBJECT_ID(N'[dbo].[AvisoFrequenciaCardiacaSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AvisoFrequenciaCardiacaSet];
GO
IF OBJECT_ID(N'[dbo].[AvisoSaturacaoSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AvisoSaturacaoSet];
GO
IF OBJECT_ID(N'[dbo].[AvisoPressaoSanguineaSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AvisoPressaoSanguineaSet];
GO
IF OBJECT_ID(N'[dbo].[TipoAvisoSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TipoAvisoSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'ConfiguracoesLimitesSet'
CREATE TABLE [dbo].[ConfiguracoesLimitesSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ValorMinimo] int  NOT NULL,
    [ValorMaximo] int  NOT NULL,
    [ValorCriticoMinimo] int  NOT NULL,
    [ValorCriticoMaximo] int  NOT NULL,
    [Nome] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'FrequenciaCardiacaValoresSet'
CREATE TABLE [dbo].[FrequenciaCardiacaValoresSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Data] datetime  NOT NULL,
    [Hora] time  NOT NULL,
    [Frequencia] int  NOT NULL,
    [Utentes_Id] int  NOT NULL
);
GO

-- Creating table 'PressaoSanguineaValoresSet'
CREATE TABLE [dbo].[PressaoSanguineaValoresSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Data] datetime  NOT NULL,
    [Hora] time  NOT NULL,
    [Distolica] int  NOT NULL,
    [Sistolica] int  NOT NULL,
    [Utentes_Id] int  NOT NULL
);
GO

-- Creating table 'SaturacaoValoresSet'
CREATE TABLE [dbo].[SaturacaoValoresSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Data] datetime  NOT NULL,
    [Hora] time  NOT NULL,
    [Saturacao] int  NOT NULL,
    [Utentes_Id] int  NOT NULL
);
GO

-- Creating table 'UtenteSet'
CREATE TABLE [dbo].[UtenteSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nome] nvarchar(max)  NOT NULL,
    [Apelido] nvarchar(max)  NOT NULL,
    [NIF] int  NOT NULL,
    [Telefone] int  NULL,
    [Email] nvarchar(max)  NULL,
    [NumeroEmergencia] int  NOT NULL,
    [NomeEmergencia] nvarchar(max)  NULL,
    [Morada] nvarchar(max)  NOT NULL,
    [Sexo] nvarchar(1)  NOT NULL,
    [Alergias] nvarchar(max)  NULL,
    [Peso] float  NULL,
    [Altura] int  NULL,
    [SNS] int  NOT NULL,
    [DataNascimento] datetime  NOT NULL,
    [Ativo] bit  NOT NULL,
    [CodigoPaisTelefone] nvarchar(max)  NULL,
    [CodigoPaisNumeroEmergencia] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'AvisoFrequenciaCardiacaSet'
CREATE TABLE [dbo].[AvisoFrequenciaCardiacaSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FrequenciaCardiacaValorSet_Id] int  NOT NULL,
    [TipoAvisoSet_Id] int  NOT NULL
);
GO

-- Creating table 'AvisoSaturacaoSet'
CREATE TABLE [dbo].[AvisoSaturacaoSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SaturacaoValorSet_Id] int  NOT NULL,
    [TipoAvisoSet_Id] int  NOT NULL
);
GO

-- Creating table 'AvisoPressaoSanguineaSet'
CREATE TABLE [dbo].[AvisoPressaoSanguineaSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PressaoSanguineaValorSet_Id] int  NOT NULL,
    [TipoAvisoSet_Id] int  NOT NULL
);
GO

-- Creating table 'TipoAvisoSet'
CREATE TABLE [dbo].[TipoAvisoSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nome] nvarchar(max)  NOT NULL,
    [TempoMinimo] int  NOT NULL,
    [TempoMaximo] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'ConfiguracoesLimitesSet'
ALTER TABLE [dbo].[ConfiguracoesLimitesSet]
ADD CONSTRAINT [PK_ConfiguracoesLimitesSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'FrequenciaCardiacaValoresSet'
ALTER TABLE [dbo].[FrequenciaCardiacaValoresSet]
ADD CONSTRAINT [PK_FrequenciaCardiacaValoresSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PressaoSanguineaValoresSet'
ALTER TABLE [dbo].[PressaoSanguineaValoresSet]
ADD CONSTRAINT [PK_PressaoSanguineaValoresSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SaturacaoValoresSet'
ALTER TABLE [dbo].[SaturacaoValoresSet]
ADD CONSTRAINT [PK_SaturacaoValoresSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UtenteSet'
ALTER TABLE [dbo].[UtenteSet]
ADD CONSTRAINT [PK_UtenteSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AvisoFrequenciaCardiacaSet'
ALTER TABLE [dbo].[AvisoFrequenciaCardiacaSet]
ADD CONSTRAINT [PK_AvisoFrequenciaCardiacaSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AvisoSaturacaoSet'
ALTER TABLE [dbo].[AvisoSaturacaoSet]
ADD CONSTRAINT [PK_AvisoSaturacaoSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AvisoPressaoSanguineaSet'
ALTER TABLE [dbo].[AvisoPressaoSanguineaSet]
ADD CONSTRAINT [PK_AvisoPressaoSanguineaSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TipoAvisoSet'
ALTER TABLE [dbo].[TipoAvisoSet]
ADD CONSTRAINT [PK_TipoAvisoSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [FrequenciaCardiacaValorSet_Id] in table 'AvisoFrequenciaCardiacaSet'
ALTER TABLE [dbo].[AvisoFrequenciaCardiacaSet]
ADD CONSTRAINT [FK_FrequenciaCardiacaValoresAvisoFrequenciaCardiaca]
    FOREIGN KEY ([FrequenciaCardiacaValorSet_Id])
    REFERENCES [dbo].[FrequenciaCardiacaValoresSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FrequenciaCardiacaValoresAvisoFrequenciaCardiaca'
CREATE INDEX [IX_FK_FrequenciaCardiacaValoresAvisoFrequenciaCardiaca]
ON [dbo].[AvisoFrequenciaCardiacaSet]
    ([FrequenciaCardiacaValorSet_Id]);
GO

-- Creating foreign key on [SaturacaoValorSet_Id] in table 'AvisoSaturacaoSet'
ALTER TABLE [dbo].[AvisoSaturacaoSet]
ADD CONSTRAINT [FK_AvisoSaturacaoSaturacaoValores]
    FOREIGN KEY ([SaturacaoValorSet_Id])
    REFERENCES [dbo].[SaturacaoValoresSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AvisoSaturacaoSaturacaoValores'
CREATE INDEX [IX_FK_AvisoSaturacaoSaturacaoValores]
ON [dbo].[AvisoSaturacaoSet]
    ([SaturacaoValorSet_Id]);
GO

-- Creating foreign key on [PressaoSanguineaValorSet_Id] in table 'AvisoPressaoSanguineaSet'
ALTER TABLE [dbo].[AvisoPressaoSanguineaSet]
ADD CONSTRAINT [FK_AvisoPressaoSanguineaPressaoSanguineaValores]
    FOREIGN KEY ([PressaoSanguineaValorSet_Id])
    REFERENCES [dbo].[PressaoSanguineaValoresSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AvisoPressaoSanguineaPressaoSanguineaValores'
CREATE INDEX [IX_FK_AvisoPressaoSanguineaPressaoSanguineaValores]
ON [dbo].[AvisoPressaoSanguineaSet]
    ([PressaoSanguineaValorSet_Id]);
GO

-- Creating foreign key on [TipoAvisoSet_Id] in table 'AvisoFrequenciaCardiacaSet'
ALTER TABLE [dbo].[AvisoFrequenciaCardiacaSet]
ADD CONSTRAINT [FK_AvisoFrequenciaCardiacaTipoAviso]
    FOREIGN KEY ([TipoAvisoSet_Id])
    REFERENCES [dbo].[TipoAvisoSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AvisoFrequenciaCardiacaTipoAviso'
CREATE INDEX [IX_FK_AvisoFrequenciaCardiacaTipoAviso]
ON [dbo].[AvisoFrequenciaCardiacaSet]
    ([TipoAvisoSet_Id]);
GO

-- Creating foreign key on [TipoAvisoSet_Id] in table 'AvisoSaturacaoSet'
ALTER TABLE [dbo].[AvisoSaturacaoSet]
ADD CONSTRAINT [FK_AvisoSaturacaoTipoAviso]
    FOREIGN KEY ([TipoAvisoSet_Id])
    REFERENCES [dbo].[TipoAvisoSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AvisoSaturacaoTipoAviso'
CREATE INDEX [IX_FK_AvisoSaturacaoTipoAviso]
ON [dbo].[AvisoSaturacaoSet]
    ([TipoAvisoSet_Id]);
GO

-- Creating foreign key on [TipoAvisoSet_Id] in table 'AvisoPressaoSanguineaSet'
ALTER TABLE [dbo].[AvisoPressaoSanguineaSet]
ADD CONSTRAINT [FK_AvisoPressaoSanguineaTipoAviso]
    FOREIGN KEY ([TipoAvisoSet_Id])
    REFERENCES [dbo].[TipoAvisoSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AvisoPressaoSanguineaTipoAviso'
CREATE INDEX [IX_FK_AvisoPressaoSanguineaTipoAviso]
ON [dbo].[AvisoPressaoSanguineaSet]
    ([TipoAvisoSet_Id]);
GO

-- Creating foreign key on [Utentes_Id] in table 'SaturacaoValoresSet'
ALTER TABLE [dbo].[SaturacaoValoresSet]
ADD CONSTRAINT [FK_SaturacaoValoresUtente]
    FOREIGN KEY ([Utentes_Id])
    REFERENCES [dbo].[UtenteSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SaturacaoValoresUtente'
CREATE INDEX [IX_FK_SaturacaoValoresUtente]
ON [dbo].[SaturacaoValoresSet]
    ([Utentes_Id]);
GO

-- Creating foreign key on [Utentes_Id] in table 'FrequenciaCardiacaValoresSet'
ALTER TABLE [dbo].[FrequenciaCardiacaValoresSet]
ADD CONSTRAINT [FK_FrequenciaCardiacaValoresUtente]
    FOREIGN KEY ([Utentes_Id])
    REFERENCES [dbo].[UtenteSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FrequenciaCardiacaValoresUtente'
CREATE INDEX [IX_FK_FrequenciaCardiacaValoresUtente]
ON [dbo].[FrequenciaCardiacaValoresSet]
    ([Utentes_Id]);
GO

-- Creating foreign key on [Utentes_Id] in table 'PressaoSanguineaValoresSet'
ALTER TABLE [dbo].[PressaoSanguineaValoresSet]
ADD CONSTRAINT [FK_PressaoSanguineaValoresUtente]
    FOREIGN KEY ([Utentes_Id])
    REFERENCES [dbo].[UtenteSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PressaoSanguineaValoresUtente'
CREATE INDEX [IX_FK_PressaoSanguineaValoresUtente]
ON [dbo].[PressaoSanguineaValoresSet]
    ([Utentes_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------

SET IDENTITY_INSERT [dbo].[ConfiguracoesLimitesSet]ON
    INSERT INTO [dbo].[ConfiguracoesLimitesSet]([Id], [ValorMinimo], [ValorMaximo], [ValorCriticoMinimo], [ValorCriticoMaximo], [Nome]) VALUES (1, 60, 120, 30, 180, N'HR')
    INSERT INTO [dbo].[ConfiguracoesLimitesSet]([Id], [ValorMinimo], [ValorMaximo], [ValorCriticoMinimo], [ValorCriticoMaximo], [Nome]) VALUES (2, 90, 100, 80, 100, N'SPO2')
    INSERT INTO [dbo].[ConfiguracoesLimitesSet]([Id], [ValorMinimo], [ValorMaximo], [ValorCriticoMinimo], [ValorCriticoMaximo], [Nome]) VALUES (3, 90, 180, 60, 190, N'BP')
SET IDENTITY_INSERT [dbo].[ConfiguracoesLimitesSet]OFF

SET IDENTITY_INSERT [dbo].[TipoAvisoSet] ON
	INSERT INTO [dbo].[TipoAvisoSet] ([Id], [Nome], [TempoMinimo], [TempoMaximo]) VALUES (1, N'EAC', 10, 0)
	INSERT INTO [dbo].[TipoAvisoSet] ([Id], [Nome], [TempoMinimo], [TempoMaximo]) VALUES (2, N'EAI', 10, 30)
	INSERT INTO [dbo].[TipoAvisoSet] ([Id], [Nome], [TempoMinimo], [TempoMaximo]) VALUES (3, N'ECC', 60, 0)
	INSERT INTO [dbo].[TipoAvisoSet] ([Id], [Nome], [TempoMinimo], [TempoMaximo]) VALUES (4, N'ECI', 60, 120)
	INSERT INTO [dbo].[TipoAvisoSet] ([Id], [Nome], [TempoMinimo], [TempoMaximo]) VALUES (5, N'ECA', 0, 0)
SET IDENTITY_INSERT [dbo].[TipoAvisoSet] OFF
