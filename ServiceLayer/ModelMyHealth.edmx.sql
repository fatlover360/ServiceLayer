
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 03/10/2017 10:53:39
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


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Utente]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Utente];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Utente'
CREATE TABLE [dbo].[Utente] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nome] nvarchar(max)  NOT NULL,
    [PressaoSanguinea] nvarchar(max)  NULL,
    [SaturacaoOxigenio] nvarchar(max)  NULL,
    [FrequenciaCardiaca] nvarchar(max)  NULL,
    [SNS] bigint  NOT NULL
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

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------