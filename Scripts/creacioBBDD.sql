-- Es crea la base de dades en cas de que no existeix
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'KarmaDB')
BEGIN
    CREATE DATABASE KarmaDB;
END

GO

-- Utilitzar la base de datos
USE KarmaDB;


-- Crear el login sols si no existeix
IF NOT EXISTS (SELECT name FROM sys.sql_logins WHERE name = N'smrp')
BEGIN
    CREATE LOGIN smrp WITH PASSWORD = 'R3g@l1z0';
END
GO


-- Crear l'usuari sols si no existeix
IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = N'smrp')
BEGIN
    CREATE USER smrp FOR LOGIN smrp;
    ALTER ROLE db_owner ADD MEMBER smrp;
END
GO


-- Crear taules quan no existeixquen

IF OBJECT_ID('dbo.AspNetRoleClaims', 'U') IS NULL
BEGIN
	CREATE TABLE dbo.AspNetRoleClaims(Id int IDENTITY(1,1) NOT NULL,RoleId nvarchar(450) NOT NULL,ClaimType nvarchar(max) NULL,ClaimValue nvarchar(max) NULL);
END
IF OBJECT_ID('dbo.AspNetRoles', 'U') IS NULL
BEGIN
	CREATE TABLE dbo.AspNetRoles(Id nvarchar(450) NOT NULL,Name nvarchar(256) NULL,NormalizedName nvarchar(256) NULL,ConcurrencyStamp nvarchar(max) NULL);
END
IF OBJECT_ID('dbo.AspNetUserClaims', 'U') IS NULL
BEGIN
	CREATE TABLE dbo.AspNetUserClaims(Id int IDENTITY(1,1) NOT NULL,UserId nvarchar(450) NOT NULL,ClaimType nvarchar(max) NULL,ClaimValue nvarchar(max) NULL);
END
IF OBJECT_ID('dbo.AspNetUserLogins', 'U') IS NULL
BEGIN
	CREATE TABLE dbo.AspNetUserLogins(LoginProvider nvarchar(450) NOT NULL,ProviderKey nvarchar(450) NOT NULL,ProviderDisplayName nvarchar(max) NULL,UserId nvarchar(450) NOT NULL);
END
IF OBJECT_ID('dbo.AspNetUserRoles', 'U') IS NULL
BEGIN
	CREATE TABLE dbo.AspNetUserRoles(UserId nvarchar(450) NOT NULL,RoleId nvarchar(450) NOT NULL);
END
IF OBJECT_ID('dbo.AspNetUsers', 'U') IS NULL
BEGIN
	CREATE TABLE dbo.AspNetUsers(Id nvarchar(450) NOT NULL,login nvarchar(max) NOT NULL,IsActive BIT NULL,UserName nvarchar(256) NULL,NormalizedUserName nvarchar(256) NULL,Email nvarchar(256) NULL,NormalizedEmail nvarchar(256) NULL,EmailConfirmed bit NOT NULL,PasswordHash nvarchar(max) NULL,SecurityStamp nvarchar(max) NULL,ConcurrencyStamp nvarchar(max) NULL,PhoneNumber nvarchar(max) NULL,PhoneNumberConfirmed bit NOT NULL,TwoFactorEnabled bit NOT NULL,LockoutEnd datetimeoffset(7) NULL,LockoutEnabled bit NOT NULL,AccessFailedCount int NOT NULL);
END
IF OBJECT_ID('dbo.AspNetUserTokens', 'U') IS NULL
BEGIN
	CREATE TABLE dbo.AspNetUserTokens(UserId nvarchar(450) NOT NULL,LoginProvider nvarchar(450) NOT NULL,Name nvarchar(450) NOT NULL,Value nvarchar(max) NULL);
END

-- AnyEscolar
IF OBJECT_ID('dbo.AnyEscolar', 'U') IS NULL
BEGIN
	CREATE TABLE dbo.AnyEscolar(IdAnyEscolar INT NOT NULL, DataIniciCurs DATE NOT NULL, DataFiCurs DATE NOT NULL, Actiu BIT NOT NULL, DiesPeriode INT NOT NULL);
END

IF OBJECT_ID('dbo.Alumne', 'U') IS NULL
BEGIN
	CREATE TABLE Alumne(NIA NVARCHAR(10) NOT NULL, Nom NVARCHAR(200) NOT NULL, Cognoms NVARCHAR(200) NOT NULL, Actiu BIT NOT NULL, email NVARCHAR(255) NOT NULL);
END
IF OBJECT_ID('dbo.AlumneEnGrup', 'U') IS NULL
BEGIN
	CREATE TABLE AlumneEnGrup(IdAlumneEnGrup INT IDENTITY, IdAnyEscolar INT NOT NULL, IdGrup NVARCHAR(50) NOT NULL, NIA NVARCHAR(10) NOT NULL, PuntuacioTotal INT NOT NULL, Karma NVARCHAR(10) NOT NULL);
END
IF OBJECT_ID('dbo.Categoria', 'U') IS NULL
BEGIN
	CREATE TABLE Categoria(IdCategoria INT IDENTITY, Descripcio NVARCHAR(50) NOT NULL, Activa BIT NOT NULL);
END
IF OBJECT_ID('dbo.ConfiguracioKarma', 'U') IS NULL
BEGIN
	CREATE TABLE ConfiguracioKarma(IdConfiguracioKarma INT IDENTITY, IdAnyEscolar INT NOT NULL, KarmaMinim INT NOT NULL, KarmaMaxim INT NOT NULL, ColorNivell NVARCHAR(10) NOT NULL, NivellPrivilegis INT NOT NULL);
END
IF OBJECT_ID('dbo.Grup', 'U') IS NULL
BEGIN
	CREATE TABLE Grup(IdAnyEscolar INT NOT NULL, IdGrup NVARCHAR(50) NOT NULL, Descripcio NVARCHAR(500) NOT NULL, IdProfessorTutor NVARCHAR(50) NULL, KarmaBase NVARCHAR(10) NOT NULL);
END
IF OBJECT_ID('dbo.Materia', 'U') IS NULL
BEGIN
	CREATE TABLE Materia(IdMateria INT IDENTITY, Nom NVARCHAR(150) NOT NULL, Activa BIT NOT NULL);
END
IF OBJECT_ID('dbo.Periode', 'U') IS NULL
BEGIN
	CREATE TABLE Periode(IdPeriode INT IDENTITY, IdAnyEscolar INT NOT NULL, DataInici DATE NOT NULL, DataFi DATE NOT NULL);
END
IF OBJECT_ID('dbo.Privilegi', 'U') IS NULL
BEGIN
	CREATE TABLE Privilegi(IdPrivilegi INT IDENTITY, IdAnyEscolar INT NOT NULL, Nivell INT NOT NULL, Descripcio NVARCHAR(255) NOT NULL, EsIndividualGrup NVARCHAR(1) NOT NULL);
END
IF OBJECT_ID('dbo.PrivilegiAssignat', 'U') IS NULL
BEGIN
	CREATE TABLE PrivilegiAssignat(IdPrivilegiAssignat INT IDENTITY, IdAlumneEnGrup INT NOT NULL, IdPrivilegi INT NOT NULL, Nivell INT NOT NULL, Descripcio NVARCHAR(255) NOT NULL, EsIndividualGrup NVARCHAR(1) NOT NULL, DataAssignacio DATE NOT NULL, DataExecucio DATE NULL);
END
IF OBJECT_ID('dbo.Professor', 'U') IS NULL
BEGIN
	CREATE TABLE Professor(IdProfessor NVARCHAR(50) NOT NULL, Nom NVARCHAR(200) NOT NULL, Cognoms NVARCHAR(200) NOT NULL, Actiu BIT NOT NULL, email NVARCHAR(255) NOT NULL);
END
IF OBJECT_ID('dbo.ProfessorDeGrup', 'U') IS NULL
BEGIN
	CREATE TABLE ProfessorDeGrup(IdProfessorDeGrup INT IDENTITY, IdAnyEscolar INT NOT NULL, IdGrup NVARCHAR(50) NOT NULL, IdProfessor NVARCHAR(50) NOT NULL, IdMateria INT NOT NULL);
END
IF OBJECT_ID('dbo.Puntuacio', 'U') IS NULL
BEGIN
	CREATE TABLE Puntuacio(IdPuntuacio INT IDENTITY, IdPeriode INT NOT NULL, IdAlumneEnGrup INT NOT NULL, IdCategoria INT NULL, DataEntrada DATE NOT NULL, Punts INT NOT NULL, Motiu NVARCHAR(255) NOT NULL, IdProfessorCreacio NVARCHAR(50) NOT NULL);
END

-- Crear primaries

IF NOT EXISTS (SELECT * FROM sys.key_constraints WHERE name = 'PK_AspNetRoleClaims')
BEGIN
	ALTER TABLE dbo.AspNetRoleClaims ADD CONSTRAINT PK_AspNetRoleClaims PRIMARY KEY (Id);
END
IF NOT EXISTS (SELECT * FROM sys.key_constraints WHERE name = 'PK_AspNetRoles')
BEGIN
	ALTER TABLE dbo.AspNetRoles ADD CONSTRAINT PK_AspNetRoles PRIMARY KEY (Id);
END
IF NOT EXISTS (SELECT * FROM sys.key_constraints WHERE name = 'PK_AspNetUserClaims')
BEGIN
	ALTER TABLE dbo.AspNetUserClaims ADD CONSTRAINT PK_AspNetUserClaims PRIMARY KEY (Id);
END
IF NOT EXISTS (SELECT * FROM sys.key_constraints WHERE name = 'PK_AspNetUserLogins')
BEGIN
	ALTER TABLE dbo.AspNetUserLogins ADD CONSTRAINT PK_AspNetUserLogins PRIMARY KEY (LoginProvider, ProviderKey);
END
IF NOT EXISTS (SELECT * FROM sys.key_constraints WHERE name = 'PK_AspNetUserRoles')
BEGIN
	ALTER TABLE dbo.AspNetUserRoles ADD CONSTRAINT PK_AspNetUserRoles PRIMARY KEY (UserId, RoleId);
END
IF NOT EXISTS (SELECT * FROM sys.key_constraints WHERE name = 'PK_AspNetUsers')
BEGIN
	ALTER TABLE dbo.AspNetUsers ADD CONSTRAINT PK_AspNetUsers PRIMARY KEY (Id);
END
IF NOT EXISTS (SELECT * FROM sys.key_constraints WHERE name = 'PK_AspNetUserTokens')
BEGIN
	ALTER TABLE dbo.AspNetUserTokens ADD CONSTRAINT PK_AspNetUserTokens PRIMARY KEY (UserId,LoginProvider,Name);
END

IF NOT EXISTS (SELECT * FROM sys.key_constraints WHERE name = 'pk_Alumne')
BEGIN
	ALTER TABLE dbo.Alumne ADD CONSTRAINT pk_Alumne PRIMARY KEY (NIA);
END
GO
IF NOT EXISTS (SELECT * FROM sys.key_constraints WHERE name = 'pk_AnyEscolar')
BEGIN
	ALTER TABLE dbo.AnyEscolar ADD CONSTRAINT pk_AnyEscolar PRIMARY KEY (IdAnyEscolar);
END
GO
IF NOT EXISTS (SELECT * FROM sys.key_constraints WHERE name = 'PK_AlumneEnGrup')
BEGIN
	ALTER TABLE dbo.AlumneEnGrup ADD CONSTRAINT PK_AlumneEnGrup PRIMARY KEY (IdAlumneEnGrup);
END
GO
IF NOT EXISTS (SELECT * FROM sys.key_constraints WHERE name = 'PK_Categoria')
BEGIN
	ALTER TABLE dbo.Categoria ADD CONSTRAINT PK_Categoria PRIMARY KEY (IdCategoria);
END
GO
IF NOT EXISTS (SELECT * FROM sys.key_constraints WHERE name = 'PK_ConfiguracioKarma')
BEGIN
	ALTER TABLE dbo.ConfiguracioKarma ADD CONSTRAINT PK_ConfiguracioKarma PRIMARY KEY (IdConfiguracioKarma);
END
GO
IF NOT EXISTS (SELECT * FROM sys.key_constraints WHERE name = 'PK_Grup')
BEGIN
	ALTER TABLE dbo.Grup ADD CONSTRAINT PK_Grup PRIMARY KEY (IdAnyEscolar, IdGrup);
END
GO
IF NOT EXISTS (SELECT * FROM sys.key_constraints WHERE name = 'PK_Materia')
BEGIN
	ALTER TABLE dbo.Materia ADD CONSTRAINT PK_Materia PRIMARY KEY (IdMateria);
END
GO
IF NOT EXISTS (SELECT * FROM sys.key_constraints WHERE name = 'PK_Periode')
BEGIN
	ALTER TABLE dbo.Periode ADD CONSTRAINT PK_Periode PRIMARY KEY (IdPeriode);
END
GO
IF NOT EXISTS (SELECT * FROM sys.key_constraints WHERE name = 'PK_Privilegi')
BEGIN
	ALTER TABLE dbo.Privilegi ADD CONSTRAINT PK_Privilegi PRIMARY KEY (IdPrivilegi);
END
GO
IF NOT EXISTS (SELECT * FROM sys.key_constraints WHERE name = 'PK_PrivilegiAssignat')
BEGIN
	ALTER TABLE dbo.PrivilegiAssignat ADD CONSTRAINT PK_PrivilegiAssignat PRIMARY KEY (IdPrivilegiAssignat);
END
GO
IF NOT EXISTS (SELECT * FROM sys.key_constraints WHERE name = 'PK_Professor')
BEGIN
	ALTER TABLE dbo.Professor ADD CONSTRAINT PK_Professor PRIMARY KEY (IdProfessor);
END
GO
IF NOT EXISTS (SELECT * FROM sys.key_constraints WHERE name = 'PK_ProfessorDeGrup')
BEGIN
	ALTER TABLE dbo.ProfessorDeGrup ADD CONSTRAINT PK_ProfessorDeGrup PRIMARY KEY (IdProfessorDeGrup);
END
GO
IF NOT EXISTS (SELECT * FROM sys.key_constraints WHERE name = 'PK_Puntuacio')
BEGIN
	ALTER TABLE dbo.Puntuacio ADD CONSTRAINT PK_Puntuacio PRIMARY KEY (IdPuntuacio);
END
GO

-- Crear Foreings
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_AspNetRoleClaims_AspNetRoles_RoleId')
BEGIN
	ALTER TABLE dbo.AspNetRoleClaims ADD CONSTRAINT FK_AspNetRoleClaims_AspNetRoles_RoleId FOREIGN KEY(RoleId) REFERENCES dbo.AspNetRoles (Id);
END
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_AspNetUserClaims_AspNetUsers_UserId')
BEGIN
	ALTER TABLE dbo.AspNetUserClaims ADD CONSTRAINT FK_AspNetUserClaims_AspNetUsers_UserId FOREIGN KEY(UserId) REFERENCES dbo.AspNetUsers (Id);
END
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_AspNetUserLogins_AspNetUsers_UserId')
BEGIN
	ALTER TABLE dbo.AspNetUserLogins ADD CONSTRAINT FK_AspNetUserLogins_AspNetUsers_UserId FOREIGN KEY(UserId) REFERENCES dbo.AspNetUsers (Id);
END
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_AspNetUserRoles_AspNetRoles_RoleId')
BEGIN
	ALTER TABLE dbo.AspNetUserRoles ADD CONSTRAINT FK_AspNetUserRoles_AspNetRoles_RoleId FOREIGN KEY(RoleId) REFERENCES dbo.AspNetRoles (Id)
END
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_AspNetUserRoles_AspNetUsers_UserId')
BEGIN
	ALTER TABLE dbo.AspNetUserRoles ADD CONSTRAINT FK_AspNetUserRoles_AspNetUsers_UserId FOREIGN KEY(UserId) REFERENCES dbo.AspNetUsers (Id)
END
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_AspNetUserTokens_AspNetUsers_UserId')
BEGIN
	ALTER TABLE dbo.AspNetUserTokens ADD CONSTRAINT FK_AspNetUserTokens_AspNetUsers_UserId FOREIGN KEY(UserId) REFERENCES dbo.AspNetUsers (Id)
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_AlumneEnGrup_Alumne_NIA')
BEGIN
	ALTER TABLE dbo.AlumneEnGrup ADD CONSTRAINT FK_AlumneEnGrup_Alumne_NIA FOREIGN KEY(NIA) REFERENCES dbo.Alumne;
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_AlumneEnGrup_Grup_IdAnyEscolar_IdGrup')
BEGIN
	ALTER TABLE dbo.AlumneEnGrup ADD CONSTRAINT FK_AlumneEnGrup_Grup_IdAnyEscolar_IdGrup FOREIGN KEY(IdAnyEscolar, IdGrup) REFERENCES dbo.Grup;
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_ConfiguracioKarma_AnyEscolar_IdAnyEscolar')
BEGIN
	ALTER TABLE dbo.ConfiguracioKarma ADD CONSTRAINT FK_ConfiguracioKarma_AnyEscolar_IdAnyEscolar FOREIGN KEY(IdAnyEscolar) REFERENCES dbo.AnyEscolar;
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Grup_AnyEscolar_IdAnyEscolar')
BEGIN
	ALTER TABLE dbo.Grup ADD CONSTRAINT FK_Grup_AnyEscolar_IdAnyEscolar FOREIGN KEY(IdAnyEscolar) REFERENCES dbo.AnyEscolar;
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Grup_Professor_IdProfessorTutor')
BEGIN
	ALTER TABLE dbo.Grup ADD CONSTRAINT FK_Grup_Professor_IdProfessorTutor FOREIGN KEY(IdProfessorTutor) REFERENCES dbo.Professor;
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Periode_AnyEscolar_IdAnyEscolar')
BEGIN
	ALTER TABLE dbo.Periode ADD CONSTRAINT FK_Periode_AnyEscolar_IdAnyEscolar FOREIGN KEY(IdAnyEscolar) REFERENCES dbo.AnyEscolar;
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Privilegi_AnyEscolar_IdAnyEscolar')
BEGIN
	ALTER TABLE dbo.Privilegi ADD CONSTRAINT FK_Privilegi_AnyEscolar_IdAnyEscolar FOREIGN KEY(IdAnyEscolar) REFERENCES dbo.AnyEscolar;
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_PrivilegiAssignat_AlumneEnGrup_IdAlumneEnGrup')
BEGIN
	ALTER TABLE dbo.PrivilegiAssignat ADD CONSTRAINT FK_PrivilegiAssignat_AlumneEnGrup_IdAlumneEnGrup FOREIGN KEY(IdAlumneEnGrup) REFERENCES dbo.AlumneEnGrup;
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_PrivilegiAssignat_Privilegi_IdPrivilegi')
BEGIN
	ALTER TABLE dbo.PrivilegiAssignat ADD CONSTRAINT FK_PrivilegiAssignat_Privilegi_IdPrivilegi FOREIGN KEY(IdPrivilegi) REFERENCES dbo.Privilegi;
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_ProfessorDeGrup_AnyEscolar_IdAnyEscolar')
BEGIN
	ALTER TABLE dbo.ProfessorDeGrup ADD CONSTRAINT FK_ProfessorDeGrup_AnyEscolar_IdAnyEscolar FOREIGN KEY(IdAnyEscolar) REFERENCES dbo.AnyEscolar;
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_ProfessorDeGrup_Grup_IdAnyEscolar_IdGrup')
BEGIN
	ALTER TABLE dbo.ProfessorDeGrup ADD CONSTRAINT FK_ProfessorDeGrup_Grup_IdAnyEscolar_IdGrup FOREIGN KEY(IdAnyEscolar, IdGrup) REFERENCES dbo.Grup;
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_ProfessorDeGrup_Materia_IdMateria')
BEGIN
	ALTER TABLE dbo.ProfessorDeGrup ADD CONSTRAINT FK_ProfessorDeGrup_Materia_IdMateria FOREIGN KEY(IdMateria) REFERENCES dbo.Materia;
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_ProfessorDeGrup_Professor_IdProfessor')
BEGIN
	ALTER TABLE dbo.ProfessorDeGrup ADD CONSTRAINT FK_ProfessorDeGrup_Professor_IdProfessor FOREIGN KEY(IdProfessor) REFERENCES dbo.Professor;
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Puntuacio_AlumneEnGrup_IdAlumneEnGrup')
BEGIN
	ALTER TABLE dbo.Puntuacio ADD CONSTRAINT FK_Puntuacio_AlumneEnGrup_IdAlumneEnGrup FOREIGN KEY(IdAlumneEnGrup) REFERENCES dbo.AlumneEnGrup;
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Puntuacio_Categoria_IdCategoria')
BEGIN
	ALTER TABLE dbo.Puntuacio ADD CONSTRAINT FK_Puntuacio_Categoria_IdCategoria FOREIGN KEY(IdCategoria) REFERENCES dbo.Categoria;
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Puntuacio_Periode_IdPeriode')
BEGIN
	ALTER TABLE dbo.Puntuacio ADD CONSTRAINT FK_Puntuacio_Periode_IdPeriode FOREIGN KEY(IdPeriode) REFERENCES dbo.Periode;
END
GO

-- índex
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_AlumneEnGrup_IdAnyEscolar_IdGrup')
BEGIN
	CREATE INDEX IX_AlumneEnGrup_IdAnyEscolar_IdGrup ON AlumneEnGrup(IdAnyEscolar, IdGrup) WITH PAD_INDEX;
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_AlumneEnGrup_NIA')
BEGIN
	CREATE INDEX IX_AlumneEnGrup_NIA ON AlumneEnGrup(NIA) WITH PAD_INDEX;
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ConfiguracioKarma_IdAnyEscolar')
BEGIN
	CREATE INDEX IX_ConfiguracioKarma_IdAnyEscolar ON ConfiguracioKarma(IdAnyEscolar) WITH PAD_INDEX;
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Grup_IdAnyEscolar')
BEGIN
	CREATE INDEX IX_Grup_IdAnyEscolar ON Grup(IdAnyEscolar) WITH PAD_INDEX;
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Grup_IdProfessorTutor')
BEGIN
	CREATE INDEX IX_Grup_IdProfessorTutor ON Grup(IdProfessorTutor) WITH PAD_INDEX;
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Periode_IdAnyEscolar')
BEGIN
	CREATE INDEX IX_Periode_IdAnyEscolar ON Periode(IdAnyEscolar) WITH PAD_INDEX;
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Privilegi_IdAnyEscolar')
BEGIN
	CREATE INDEX IX_Privilegi_IdAnyEscolar ON Privilegi(IdAnyEscolar) WITH PAD_INDEX;
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_PrivilegiAssignat_IdAlumneEnGrup')
BEGIN
	CREATE INDEX IX_PrivilegiAssignat_IdAlumneEnGrup ON PrivilegiAssignat(IdAlumneEnGrup) WITH PAD_INDEX;
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_PrivilegiAssignat_IdPrivilegi')
BEGIN
	CREATE INDEX IX_PrivilegiAssignat_IdPrivilegi ON PrivilegiAssignat(IdPrivilegi) WITH PAD_INDEX;
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ProfessorDeGrup_IdAnyEscolar_IdGrup')
BEGIN
	CREATE INDEX IX_ProfessorDeGrup_IdAnyEscolar_IdGrup ON ProfessorDeGrup(IdAnyEscolar, IdGrup) WITH PAD_INDEX;
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ProfessorDeGrup_IdMateria')
BEGIN
	CREATE INDEX IX_ProfessorDeGrup_IdMateria ON ProfessorDeGrup(IdMateria) WITH PAD_INDEX;
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ProfessorDeGrup_IdProfessor')
BEGIN
	CREATE INDEX IX_ProfessorDeGrup_IdProfessor ON ProfessorDeGrup(IdProfessor) WITH PAD_INDEX;
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Puntuacio_IdAlumneEnGrup')
BEGIN
	CREATE INDEX IX_Puntuacio_IdAlumneEnGrup ON Puntuacio(IdAlumneEnGrup) WITH PAD_INDEX;
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Puntuacio_IdCategoria')
BEGIN
	CREATE INDEX IX_Puntuacio_IdCategoria ON Puntuacio(IdCategoria) WITH PAD_INDEX;
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Puntuacio_IdPeriode')
BEGIN
	CREATE INDEX IX_Puntuacio_IdPeriode ON Puntuacio(IdPeriode) WITH PAD_INDEX;
END
GO

