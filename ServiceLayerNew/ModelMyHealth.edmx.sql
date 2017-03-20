
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 03/20/2017 20:34:19
-- Generated from EDMX file: C:\Users\j17vi\Source\Repos\ServiceLayer\ServiceLayerNew\ModelMyHealth.edmx
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

IF OBJECT_ID(N'[dbo].[FK_AlertaTipoAlerta]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AlertaSet] DROP CONSTRAINT [FK_AlertaTipoAlerta];
GO
IF OBJECT_ID(N'[dbo].[FK_FrequenciaCardiacaValoresAlerta]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FrequenciaCardiacaValoresSet] DROP CONSTRAINT [FK_FrequenciaCardiacaValoresAlerta];
GO
IF OBJECT_ID(N'[dbo].[FK_PressaoSanguineaValoresAlerta]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PressaoSanguineaValoresSet] DROP CONSTRAINT [FK_PressaoSanguineaValoresAlerta];
GO
IF OBJECT_ID(N'[dbo].[FK_SaturacaoValoresAlerta]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SaturacaoValoresSet] DROP CONSTRAINT [FK_SaturacaoValoresAlerta];
GO
IF OBJECT_ID(N'[dbo].[FK_UtenteFrequenciaCardiacaValores]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FrequenciaCardiacaValoresSet] DROP CONSTRAINT [FK_UtenteFrequenciaCardiacaValores];
GO
IF OBJECT_ID(N'[dbo].[FK_UtentePressaoSanguineaValores]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PressaoSanguineaValoresSet] DROP CONSTRAINT [FK_UtentePressaoSanguineaValores];
GO
IF OBJECT_ID(N'[dbo].[FK_UtenteSaturacaoValores]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SaturacaoValoresSet] DROP CONSTRAINT [FK_UtenteSaturacaoValores];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[AlertaSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AlertaSet];
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
IF OBJECT_ID(N'[dbo].[TipoAlertaSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TipoAlertaSet];
GO
IF OBJECT_ID(N'[dbo].[UtenteSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UtenteSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'AlertaSet'
CREATE TABLE [dbo].[AlertaSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ValorMinimo] int  NOT NULL,
    [ValorMaximo] int  NOT NULL,
    [ValorCriticoMinimo] int  NOT NULL,
    [ValorCriticoMaximo] int  NOT NULL,
    [TipoAlertas_Id] int  NOT NULL
);
GO

-- Creating table 'FrequenciaCardiacaValoresSet'
CREATE TABLE [dbo].[FrequenciaCardiacaValoresSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Data] datetime  NOT NULL,
    [Hora] time  NOT NULL,
    [Frequencia] int  NOT NULL,
    [Utente_Id] int  NOT NULL,
    [Alertas_Id] int  NOT NULL
);
GO

-- Creating table 'PressaoSanguineaValoresSet'
CREATE TABLE [dbo].[PressaoSanguineaValoresSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Data] datetime  NOT NULL,
    [Hora] time  NOT NULL,
    [Distolica] int  NOT NULL,
    [Sistolica] int  NOT NULL,
    [Utente_Id] int  NOT NULL,
    [Alertas_Id] int  NOT NULL
);
GO

-- Creating table 'SaturacaoValoresSet'
CREATE TABLE [dbo].[SaturacaoValoresSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Data] datetime  NOT NULL,
    [Hora] time  NOT NULL,
    [Saturacao] int  NOT NULL,
    [Utente_Id] int  NOT NULL,
    [Alertas_Id] int  NOT NULL
);
GO

-- Creating table 'TipoAlertaSet'
CREATE TABLE [dbo].[TipoAlertaSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nome] nvarchar(max)  NOT NULL
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
	UNIQUE(SNS),
	UNIQUE(NIF)
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'AlertaSet'
ALTER TABLE [dbo].[AlertaSet]
ADD CONSTRAINT [PK_AlertaSet]
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

-- Creating primary key on [Id] in table 'TipoAlertaSet'
ALTER TABLE [dbo].[TipoAlertaSet]
ADD CONSTRAINT [PK_TipoAlertaSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UtenteSet'
ALTER TABLE [dbo].[UtenteSet]
ADD CONSTRAINT [PK_UtenteSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [TipoAlertas_Id] in table 'AlertaSet'
ALTER TABLE [dbo].[AlertaSet]
ADD CONSTRAINT [FK_AlertaTipoAlerta]
    FOREIGN KEY ([TipoAlertas_Id])
    REFERENCES [dbo].[TipoAlertaSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AlertaTipoAlerta'
CREATE INDEX [IX_FK_AlertaTipoAlerta]
ON [dbo].[AlertaSet]
    ([TipoAlertas_Id]);
GO

-- Creating foreign key on [Alertas_Id] in table 'FrequenciaCardiacaValoresSet'
ALTER TABLE [dbo].[FrequenciaCardiacaValoresSet]
ADD CONSTRAINT [FK_FrequenciaCardiacaValoresAlerta]
    FOREIGN KEY ([Alertas_Id])
    REFERENCES [dbo].[AlertaSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FrequenciaCardiacaValoresAlerta'
CREATE INDEX [IX_FK_FrequenciaCardiacaValoresAlerta]
ON [dbo].[FrequenciaCardiacaValoresSet]
    ([Alertas_Id]);
GO

-- Creating foreign key on [Alertas_Id] in table 'PressaoSanguineaValoresSet'
ALTER TABLE [dbo].[PressaoSanguineaValoresSet]
ADD CONSTRAINT [FK_PressaoSanguineaValoresAlerta]
    FOREIGN KEY ([Alertas_Id])
    REFERENCES [dbo].[AlertaSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PressaoSanguineaValoresAlerta'
CREATE INDEX [IX_FK_PressaoSanguineaValoresAlerta]
ON [dbo].[PressaoSanguineaValoresSet]
    ([Alertas_Id]);
GO

-- Creating foreign key on [Alertas_Id] in table 'SaturacaoValoresSet'
ALTER TABLE [dbo].[SaturacaoValoresSet]
ADD CONSTRAINT [FK_SaturacaoValoresAlerta]
    FOREIGN KEY ([Alertas_Id])
    REFERENCES [dbo].[AlertaSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SaturacaoValoresAlerta'
CREATE INDEX [IX_FK_SaturacaoValoresAlerta]
ON [dbo].[SaturacaoValoresSet]
    ([Alertas_Id]);
GO

-- Creating foreign key on [Utente_Id] in table 'FrequenciaCardiacaValoresSet'
ALTER TABLE [dbo].[FrequenciaCardiacaValoresSet]
ADD CONSTRAINT [FK_UtenteFrequenciaCardiacaValores]
    FOREIGN KEY ([Utente_Id])
    REFERENCES [dbo].[UtenteSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UtenteFrequenciaCardiacaValores'
CREATE INDEX [IX_FK_UtenteFrequenciaCardiacaValores]
ON [dbo].[FrequenciaCardiacaValoresSet]
    ([Utente_Id]);
GO

-- Creating foreign key on [Utente_Id] in table 'PressaoSanguineaValoresSet'
ALTER TABLE [dbo].[PressaoSanguineaValoresSet]
ADD CONSTRAINT [FK_UtentePressaoSanguineaValores]
    FOREIGN KEY ([Utente_Id])
    REFERENCES [dbo].[UtenteSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UtentePressaoSanguineaValores'
CREATE INDEX [IX_FK_UtentePressaoSanguineaValores]
ON [dbo].[PressaoSanguineaValoresSet]
    ([Utente_Id]);
GO

-- Creating foreign key on [Utente_Id] in table 'SaturacaoValoresSet'
ALTER TABLE [dbo].[SaturacaoValoresSet]
ADD CONSTRAINT [FK_UtenteSaturacaoValores]
    FOREIGN KEY ([Utente_Id])
    REFERENCES [dbo].[UtenteSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UtenteSaturacaoValores'
CREATE INDEX [IX_FK_UtenteSaturacaoValores]
ON [dbo].[SaturacaoValoresSet]
    ([Utente_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------

SET IDENTITY_INSERT [dbo].[TipoAlertaSet] ON
    INSERT INTO [dbo].[TipoAlertaSet] ([Id], [Nome]) VALUES (1, N'HR')
    INSERT INTO [dbo].[TipoAlertaSet] ([Id], [Nome]) VALUES (2, N'SPO2')
    INSERT INTO [dbo].[TipoAlertaSet] ([Id], [Nome]) VALUES (3, N'BP')
SET IDENTITY_INSERT [dbo].[TipoAlertaSet] OFF

SET IDENTITY_INSERT [dbo].[AlertaSet] ON
    INSERT INTO [dbo].[AlertaSet] ([Id], [ValorMinimo], [ValorMaximo], [ValorCriticoMinimo], [ValorCriticoMaximo], [TipoAlertas_Id]) VALUES (1, 60, 120, 30, 180, 1)
    INSERT INTO [dbo].[AlertaSet] ([Id], [ValorMinimo], [ValorMaximo], [ValorCriticoMinimo], [ValorCriticoMaximo], [TipoAlertas_Id]) VALUES (2, 90, 100, 80, 100, 2)
    INSERT INTO [dbo].[AlertaSet] ([Id], [ValorMinimo], [ValorMaximo], [ValorCriticoMinimo], [ValorCriticoMaximo], [TipoAlertas_Id]) VALUES (3, 90, 180, 60, 190, 3)
SET IDENTITY_INSERT [dbo].[AlertaSet] OFF