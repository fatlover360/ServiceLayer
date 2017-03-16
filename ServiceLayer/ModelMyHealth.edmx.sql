
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 03/16/2017 16:29:03
-- Generated from EDMX file: C:\Users\j17vi\Source\Repos\ServiceLayer\ServiceLayer\ModelMyHealth.edmx
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

IF OBJECT_ID(N'[dbo].[FK_UtenteFrequenciaCardiacaValores]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FrequenciaCardiacaValores] DROP CONSTRAINT [FK_UtenteFrequenciaCardiacaValores];
GO
IF OBJECT_ID(N'[dbo].[FK_UtenteSaturacaoValores]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SaturacaoValores] DROP CONSTRAINT [FK_UtenteSaturacaoValores];
GO
IF OBJECT_ID(N'[dbo].[FK_UtentePressaoSanguineaValores]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PressaoSanguineaValores] DROP CONSTRAINT [FK_UtentePressaoSanguineaValores];
GO
IF OBJECT_ID(N'[dbo].[FK_FrequenciaCardiacaValoresAlerta]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FrequenciaCardiacaValores] DROP CONSTRAINT [FK_FrequenciaCardiacaValoresAlerta];
GO
IF OBJECT_ID(N'[dbo].[FK_SaturacaoValoresAlerta]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SaturacaoValores] DROP CONSTRAINT [FK_SaturacaoValoresAlerta];
GO
IF OBJECT_ID(N'[dbo].[FK_PressaoSanguineaValoresAlerta]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PressaoSanguineaValores] DROP CONSTRAINT [FK_PressaoSanguineaValoresAlerta];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Utente]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Utente];
GO
IF OBJECT_ID(N'[dbo].[Alerta]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Alerta];
GO
IF OBJECT_ID(N'[dbo].[FrequenciaCardiacaValores]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FrequenciaCardiacaValores];
GO
IF OBJECT_ID(N'[dbo].[SaturacaoValores]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SaturacaoValores];
GO
IF OBJECT_ID(N'[dbo].[PressaoSanguineaValores]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PressaoSanguineaValores];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Utente'
CREATE TABLE [dbo].[Utente] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nome] nvarchar(max)  NOT NULL,
    [Apelido] nvarchar(max)  NOT NULL,
    [NIF] int  NOT NULL,
    [Telefone] nvarchar(max)  NULL,
    [Email] nvarchar(max)  NULL,
    [NumeroEmergencia] nvarchar(max)  NOT NULL,
    [NomeEmergencia] nvarchar(max)  NULL,
    [Morada] nvarchar(max)  NULL,
    [Sexo] nvarchar(max)  NOT NULL,
    [Alergias] nvarchar(max)  NULL,
    [Peso] float  NULL,
    [Altura] int  NULL,
    [SNS] int  NOT NULL
);
GO

-- Creating table 'Alerta'
CREATE TABLE [dbo].[Alerta] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Tipo] nvarchar(max)  NOT NULL,
    [ValorMinimo] int  NOT NULL,
    [ValorMaximo] int  NOT NULL,
    [ValorCriticoMinimo] int  NOT NULL,
    [ValorCriticoMaximo] int  NOT NULL
);
GO

-- Creating table 'FrequenciaCardiacaValores'
CREATE TABLE [dbo].[FrequenciaCardiacaValores] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Data] datetime  NOT NULL,
    [Hora] time  NOT NULL,
    [Frequencia] int  NOT NULL,
    [Utente_Id] int  NOT NULL,
    [Alertas_Id] int  NOT NULL
);
GO

-- Creating table 'SaturacaoValores'
CREATE TABLE [dbo].[SaturacaoValores] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Data] datetime  NOT NULL,
    [Hora] time  NOT NULL,
    [Saturacao] int  NOT NULL,
    [Utente_Id] int  NOT NULL,
    [Alertas_Id] int  NOT NULL
);
GO

-- Creating table 'PressaoSanguineaValores'
CREATE TABLE [dbo].[PressaoSanguineaValores] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Data] datetime  NOT NULL,
    [Hora] time  NOT NULL,
    [Distolica] int  NOT NULL,
    [Sistolica] int  NOT NULL,
    [Utente_Id] int  NOT NULL,
    [Alertas_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Utente'
ALTER TABLE [dbo].[Utente]
ADD CONSTRAINT [PK_Utente]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Alerta'
ALTER TABLE [dbo].[Alerta]
ADD CONSTRAINT [PK_Alerta]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'FrequenciaCardiacaValores'
ALTER TABLE [dbo].[FrequenciaCardiacaValores]
ADD CONSTRAINT [PK_FrequenciaCardiacaValores]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SaturacaoValores'
ALTER TABLE [dbo].[SaturacaoValores]
ADD CONSTRAINT [PK_SaturacaoValores]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PressaoSanguineaValores'
ALTER TABLE [dbo].[PressaoSanguineaValores]
ADD CONSTRAINT [PK_PressaoSanguineaValores]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Utente_Id] in table 'FrequenciaCardiacaValores'
ALTER TABLE [dbo].[FrequenciaCardiacaValores]
ADD CONSTRAINT [FK_UtenteFrequenciaCardiacaValores]
    FOREIGN KEY ([Utente_Id])
    REFERENCES [dbo].[Utente]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UtenteFrequenciaCardiacaValores'
CREATE INDEX [IX_FK_UtenteFrequenciaCardiacaValores]
ON [dbo].[FrequenciaCardiacaValores]
    ([Utente_Id]);
GO

-- Creating foreign key on [Utente_Id] in table 'SaturacaoValores'
ALTER TABLE [dbo].[SaturacaoValores]
ADD CONSTRAINT [FK_UtenteSaturacaoValores]
    FOREIGN KEY ([Utente_Id])
    REFERENCES [dbo].[Utente]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UtenteSaturacaoValores'
CREATE INDEX [IX_FK_UtenteSaturacaoValores]
ON [dbo].[SaturacaoValores]
    ([Utente_Id]);
GO

-- Creating foreign key on [Utente_Id] in table 'PressaoSanguineaValores'
ALTER TABLE [dbo].[PressaoSanguineaValores]
ADD CONSTRAINT [FK_UtentePressaoSanguineaValores]
    FOREIGN KEY ([Utente_Id])
    REFERENCES [dbo].[Utente]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UtentePressaoSanguineaValores'
CREATE INDEX [IX_FK_UtentePressaoSanguineaValores]
ON [dbo].[PressaoSanguineaValores]
    ([Utente_Id]);
GO

-- Creating foreign key on [Alertas_Id] in table 'FrequenciaCardiacaValores'
ALTER TABLE [dbo].[FrequenciaCardiacaValores]
ADD CONSTRAINT [FK_FrequenciaCardiacaValoresAlerta]
    FOREIGN KEY ([Alertas_Id])
    REFERENCES [dbo].[Alerta]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FrequenciaCardiacaValoresAlerta'
CREATE INDEX [IX_FK_FrequenciaCardiacaValoresAlerta]
ON [dbo].[FrequenciaCardiacaValores]
    ([Alertas_Id]);
GO

-- Creating foreign key on [Alertas_Id] in table 'SaturacaoValores'
ALTER TABLE [dbo].[SaturacaoValores]
ADD CONSTRAINT [FK_SaturacaoValoresAlerta]
    FOREIGN KEY ([Alertas_Id])
    REFERENCES [dbo].[Alerta]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SaturacaoValoresAlerta'
CREATE INDEX [IX_FK_SaturacaoValoresAlerta]
ON [dbo].[SaturacaoValores]
    ([Alertas_Id]);
GO

-- Creating foreign key on [Alertas_Id] in table 'PressaoSanguineaValores'
ALTER TABLE [dbo].[PressaoSanguineaValores]
ADD CONSTRAINT [FK_PressaoSanguineaValoresAlerta]
    FOREIGN KEY ([Alertas_Id])
    REFERENCES [dbo].[Alerta]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PressaoSanguineaValoresAlerta'
CREATE INDEX [IX_FK_PressaoSanguineaValoresAlerta]
ON [dbo].[PressaoSanguineaValores]
    ([Alertas_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------