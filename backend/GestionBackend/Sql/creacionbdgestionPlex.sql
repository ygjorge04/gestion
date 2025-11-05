-- 1️⃣ Crear base de datos si no existe
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'Gestion')
BEGIN
    CREATE DATABASE Gestion;
END;
GO

-- 2️⃣ Usar la base de datos
USE Gestion;
GO

-- 3️⃣ Crear tabla Usuario
CREATE TABLE Usuario (
    id INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    numeroDoc VARCHAR(50) UNIQUE NOT NULL,
    contrasena VARCHAR(255) NOT NULL,
    rol VARCHAR(20) NOT NULL DEFAULT 'usuario'
);
GO

-- 4️⃣ Crear tabla Espacio
CREATE TABLE Espacio (
    id INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    tipo VARCHAR(50) NOT NULL,
    capacidad INT,
    ubicacion VARCHAR(255)
);
GO

-- 5️⃣ Crear tabla Reserva
CREATE TABLE Reserva (
    id INT IDENTITY(1,1) PRIMARY KEY,
    id_usuario INT NOT NULL,
    id_espacio INT NOT NULL,
    fecha DATE NOT NULL,
    hora_inicio TIME NOT NULL,
    duracion_minutos INT NOT NULL,
    estado VARCHAR(20) DEFAULT 'pendiente',
    CONSTRAINT fk_usuario FOREIGN KEY (id_usuario) REFERENCES Usuario(id),
    CONSTRAINT fk_espacio FOREIGN KEY (id_espacio) REFERENCES Espacio(id)
);
GO

-- 6️⃣ Crear índices sobre Reserva
CREATE INDEX idx_reserva_usuario ON Reserva(id_usuario);
CREATE INDEX idx_reserva_espacio ON Reserva(id_espacio);
CREATE INDEX idx_reserva_fecha ON Reserva(fecha);
GO