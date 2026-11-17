/*
   SISTEMA DE GESTION DE CLIENTES
   Base de datos: SistemaClientesDB
*/

-- CREACION DE DB
IF DB_ID('SistemaClientesDB') IS NULL
BEGIN
    CREATE DATABASE SistemaClientesDB;
END
GO

USE SistemaClientesDB;
GO

-- TABLA USUARIOS
IF OBJECT_ID('dbo.Usuarios', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Usuarios (
        IDUsuario      INT IDENTITY(1,1) NOT NULL,
        NombreUsuario  VARCHAR(50)  NOT NULL,
        NombreCompleto VARCHAR(100) NOT NULL,
        PasswordHash   VARCHAR(64)  NOT NULL,   -- SHA-256 en hexadecimal (64 caracteres)
        Salt           VARCHAR(32)  NOT NULL,   -- Valor aleatorio unico por usuario
        Activo         BIT          NOT NULL CONSTRAINT DF_Usuarios_Activo DEFAULT (1),
        FechaCreacion  DATETIME     NOT NULL CONSTRAINT DF_Usuarios_Fecha DEFAULT (GETDATE()),
        CONSTRAINT PK_Usuarios PRIMARY KEY (IDUsuario),
        CONSTRAINT UQ_Usuarios_NombreUsuario UNIQUE (NombreUsuario)
    );
END
GO

-- TABLA CLIENTES
IF OBJECT_ID('dbo.Clientes', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Clientes (
        IDCliente      INT IDENTITY(1,1) NOT NULL,
        Nombre         VARCHAR(80)  NOT NULL,
        Apellido       VARCHAR(80)  NOT NULL,
        DUI            CHAR(10)     NOT NULL,   -- Formato 00000000-0
        Telefono       VARCHAR(15)  NOT NULL,
        Correo         VARCHAR(100) NOT NULL,
        Direccion      VARCHAR(200) NOT NULL,
        FechaRegistro  DATETIME     NOT NULL CONSTRAINT DF_Clientes_Fecha DEFAULT (GETDATE()),
        CONSTRAINT PK_Clientes PRIMARY KEY (IDCliente),
        CONSTRAINT UQ_Clientes_DUI UNIQUE (DUI),
        CONSTRAINT CK_Clientes_DUI CHECK (DUI LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]-[0-9]'),
        CONSTRAINT CK_Clientes_Correo CHECK (Correo LIKE '%_@_%._%')
    );
END
GO

-- TABLA BITACORA
IF OBJECT_ID('dbo.Bitacora', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Bitacora (
        IDBitacora     INT IDENTITY(1,1) NOT NULL,
        Accion         VARCHAR(10)  NOT NULL,
        IDCliente      INT          NOT NULL,   -- Sin FK a proposito: debe conservarse aunque el cliente se elimine
        FechaHora      DATETIME     NOT NULL CONSTRAINT DF_Bitacora_Fecha DEFAULT (GETDATE()),
        IDUsuario      INT          NOT NULL,
        NombreUsuario  VARCHAR(50)  NOT NULL,
        Descripcion    VARCHAR(500) NULL,
        CONSTRAINT PK_Bitacora PRIMARY KEY (IDBitacora),
        CONSTRAINT FK_Bitacora_Usuarios FOREIGN KEY (IDUsuario) REFERENCES dbo.Usuarios (IDUsuario),
        CONSTRAINT CK_Bitacora_Accion CHECK (Accion IN ('AGREGAR', 'EDITAR', 'ELIMINAR'))
    );
END
GO

-- USUARIO INICIAL
--    Usuario:    admin
--    Contrasena: Admin123*
IF NOT EXISTS (SELECT 1 FROM dbo.Usuarios WHERE NombreUsuario = 'admin')
BEGIN
    DECLARE @Salt VARCHAR(32) = CONVERT(VARCHAR(32), CRYPT_GEN_RANDOM(16), 2);

    INSERT INTO dbo.Usuarios (NombreUsuario, NombreCompleto, PasswordHash, Salt)
    VALUES (
        'admin',
        'Administrador del Sistema',
        CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', @Salt + 'Admin123*'), 2),
        @Salt
    );
END
GO