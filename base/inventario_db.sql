CREATE DATABASE IF NOT EXISTS inventario_db CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci;
USE inventario_db;

DROP TABLE IF EXISTS detalle_venta;
DROP TABLE IF EXISTS venta;
DROP TABLE IF EXISTS producto;
DROP TABLE IF EXISTS cliente;

CREATE TABLE cliente
(
    id        INT AUTO_INCREMENT PRIMARY KEY,
    nombre    VARCHAR(120) NOT NULL,
    nit       VARCHAR(20)  NOT NULL,
    creado_en TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE KEY uk_cliente_nit (nit)
) ENGINE = InnoDB;

ALTER TABLE cliente
ADD COLUMN correo     VARCHAR(100) NOT NULL AFTER nit,
ADD COLUMN telefono   VARCHAR(20)  NOT NULL AFTER correo,
ADD COLUMN direccion  VARCHAR(200) NOT NULL AFTER telefono;

CREATE TABLE producto
(
    id        INT AUTO_INCREMENT PRIMARY KEY,
    nombre    VARCHAR(120)   NOT NULL,
    precio    DECIMAL(10, 2) NOT NULL CHECK (precio >= 0),
    stock     INT            NOT NULL CHECK (stock >= 0),
    creado_en TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    KEY idx_producto_nombre (nombre)
) ENGINE = InnoDB;

INSERT INTO producto (nombre, precio, stock)
VALUES
('Laptop Dell 14"', 4500.00, 10),
('Mouse Inalámbrico', 120.00, 50),
('Teclado Mecánico', 380.00, 25);

CREATE TABLE venta
(
    id         INT AUTO_INCREMENT PRIMARY KEY,
    cliente_id INT            NOT NULL,
    fecha      DATETIME       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    total      DECIMAL(12, 2) NOT NULL DEFAULT 0,
    creado_en  TIMESTAMP               DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_venta_cliente FOREIGN KEY (cliente_id) REFERENCES cliente (id)
        ON UPDATE CASCADE ON DELETE RESTRICT
) ENGINE = InnoDB;

CREATE TABLE detalle_venta
(
    id          INT AUTO_INCREMENT PRIMARY KEY,
    venta_id    INT            NOT NULL,
    producto_id INT            NOT NULL,
    cantidad    INT            NOT NULL CHECK (cantidad > 0),
    precio_unit DECIMAL(10, 2) NOT NULL CHECK (precio_unit >= 0),
    subtotal    DECIMAL(12, 2) NOT NULL,
    CONSTRAINT fk_detalle_venta FOREIGN KEY (venta_id) REFERENCES venta (id)
        ON UPDATE CASCADE ON DELETE CASCADE,
    CONSTRAINT fk_detalle_producto FOREIGN KEY (producto_id) REFERENCES producto (id)
        ON UPDATE CASCADE ON DELETE RESTRICT
) ENGINE = InnoDB;






