
# Procedimientos Almacenados y Triggers

## 1. Procedimientos Almacenados

### a. Alta, Baja, y Actualización de Productos

#### Alta de Producto

```sql
CREATE PROCEDURE usp_AltaProducto
    @Nombre VARCHAR(100),
    @Precio DECIMAL(10, 2),
    @Descripcion VARCHAR(255),
    @ID_Proveedor INT,
    @Resultado INT OUTPUT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO Producto (Nombre, Precio, Descripcion, ID_Proveedor)
        VALUES (@Nombre, @Precio, @Descripcion, @ID_Proveedor);

        COMMIT;
        SET @Resultado = 1;  -- Producto agregado exitosamente
    END TRY
    BEGIN CATCH
        ROLLBACK;
        SET @Resultado = 0;  -- Error al agregar producto
    END CATCH
END
GO
```

#### Baja de Producto

```sql
CREATE PROCEDURE usp_BajaProducto
    @ID_Producto INT,
    @Resultado INT OUTPUT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        DELETE FROM Producto WHERE ID_Producto = @ID_Producto;

        COMMIT;
        SET @Resultado = 1;  -- Producto eliminado exitosamente
    END TRY
    BEGIN CATCH
        ROLLBACK;
        SET @Resultado = 0;  -- Error al eliminar producto
    END CATCH
END
GO
```

#### Actualización de Producto

```sql
CREATE PROCEDURE usp_ActualizarProducto
    @ID_Producto INT,
    @Nombre VARCHAR(100),
    @Precio DECIMAL(10, 2),
    @Descripcion VARCHAR(255),
    @ID_Proveedor INT,
    @Resultado INT OUTPUT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE Producto
        SET Nombre = @Nombre, Precio = @Precio, Descripcion = @Descripcion, ID_Proveedor = @ID_Proveedor
        WHERE ID_Producto = @ID_Producto;

        COMMIT;
        SET @Resultado = 1;  -- Producto actualizado exitosamente
    END TRY
    BEGIN CATCH
        ROLLBACK;
        SET @Resultado = 0;  -- Error al actualizar producto
    END CATCH
END
GO
```

### b. Alta, Baja, y Actualización de Inventarios e Inventario Detalles

#### Alta de Inventario

```sql
CREATE PROCEDURE usp_AltaInventario
    @Fecha_Registro DATE,
    @Observaciones VARCHAR(255),
    @Importe DECIMAL(10, 2),
    @IVA DECIMAL(10, 2),
    @Total DECIMAL(10, 2),
    @ID_Proveedor INT,
    @Resultado INT OUTPUT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO Inventario (Fecha_Registro, Observaciones, Importe, IVA, Total, ID_Proveedor)
        VALUES (@Fecha_Registro, @Observaciones, @Importe, @IVA, @Total, @ID_Proveedor);

        COMMIT;
        SET @Resultado = 1;  -- Inventario agregado exitosamente
    END TRY
    BEGIN CATCH
        ROLLBACK;
        SET @Resultado = 0;  -- Error al agregar inventario
    END CATCH
END
GO
```

#### Alta de Detalle Inventario

```sql
CREATE PROCEDURE usp_AltaDetalleInventario
    @ID_Inventario INT,
    @ID_Producto INT,
    @Cantidad_Entrante INT,
    @Costo_Unitario DECIMAL(10, 2),
    @Subtotal DECIMAL(10, 2),
    @Resultado INT OUTPUT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO DetalleInventario (ID_Inventario, ID_Producto, Cantidad_Entrante, Costo_Unitario, Subtotal)
        VALUES (@ID_Inventario, @ID_Producto, @Cantidad_Entrante, @Costo_Unitario, @Subtotal);

        -- Actualizar stock en la tabla Saldos
        UPDATE Saldos
        SET Cantidad_Entrante = Cantidad_Entrante + @Cantidad_Entrante
        WHERE ID_Producto = @ID_Producto;

        COMMIT;
        SET @Resultado = 1;  -- Detalle de inventario agregado exitosamente
    END TRY
    BEGIN CATCH
        ROLLBACK;
        SET @Resultado = 0;  -- Error al agregar detalle inventario
    END CATCH
END
GO
```

#### Baja de Inventario

```sql
CREATE PROCEDURE usp_BajaInventario
    @ID_Inventario INT,
    @Resultado INT OUTPUT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        DELETE FROM DetalleInventario WHERE ID_Inventario = @ID_Inventario;
        DELETE FROM Inventario WHERE ID_Inventario = @ID_Inventario;

        COMMIT;
        SET @Resultado = 1;  -- Inventario eliminado exitosamente
    END TRY
    BEGIN CATCH
        ROLLBACK;
        SET @Resultado = 0;  -- Error al eliminar inventario
    END CATCH
END
GO
```

### c. Alta, Baja, y Actualización de Proveedores

#### Alta de Proveedor

```sql
CREATE PROCEDURE usp_AltaProveedor
    @Nombre VARCHAR(100),
    @Telefono VARCHAR(20),
    @Direccion VARCHAR(255),
    @Resultado INT OUTPUT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO Proveedor (Nombre, Telefono, Direccion)
        VALUES (@Nombre, @Telefono, @Direccion);

        COMMIT;
        SET @Resultado = 1;  -- Proveedor agregado exitosamente
    END TRY
    BEGIN CATCH
        ROLLBACK;
        SET @Resultado = 0;  -- Error al agregar proveedor
    END CATCH
END
GO
```

#### Baja de Proveedor

```sql
CREATE PROCEDURE usp_BajaProveedor
    @ID_Proveedor INT,
    @Resultado INT OUTPUT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        DELETE FROM Proveedor WHERE ID_Proveedor = @ID_Proveedor;

        COMMIT;
        SET @Resultado = 1;  -- Proveedor eliminado exitosamente
    END TRY
    BEGIN CATCH
        ROLLBACK;
        SET @Resultado = 0;  -- Error al eliminar proveedor
    END CATCH
END
GO
```

#### Actualización de Proveedor

```sql
CREATE PROCEDURE usp_ActualizarProveedor
    @ID_Proveedor INT,
    @Nombre VARCHAR(100),
    @Telefono VARCHAR(20),
    @Direccion VARCHAR(255),
    @Resultado INT OUTPUT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE Proveedor
        SET Nombre = @Nombre, Telefono = @Telefono, Direccion = @Direccion
        WHERE ID_Proveedor = @ID_Proveedor;

        COMMIT;
        SET @Resultado = 1;  -- Proveedor actualizado exitosamente
    END TRY
    BEGIN CATCH
        ROLLBACK;
        SET @Resultado = 0;  -- Error al actualizar proveedor
    END CATCH
END
GO
```

## 2. Triggers

### a. Actualizar stock del producto cuando se agrega al inventario

```sql
CREATE TRIGGER trg_ActualizarStockInventario
ON DetalleInventario
AFTER INSERT
AS
BEGIN
    DECLARE @ID_Producto INT, @Cantidad_Entrante INT;

    SELECT @ID_Producto = ID_Producto, @Cantidad_Entrante = Cantidad_Entrante FROM INSERTED;

    -- Actualizar stock
    UPDATE Saldos
    SET Cantidad_Entrante = Cantidad_Entrante + @Cantidad_Entrante
    WHERE ID_Producto = @ID_Producto;
END
GO
```

### b. Actualizar stock del producto cuando se vende

```sql
CREATE TRIGGER trg_ActualizarStockVenta
ON DetalleVenta
AFTER INSERT
AS
BEGIN
    DECLARE @ID_Producto INT, @Cantidad INT;

    SELECT @ID_Producto = ID_Producto, @Cantidad = Cantidad FROM INSERTED;

    -- Actualizar stock
    UPDATE Saldos
    SET Cantidad_Salida = Cantidad_Salida + @Cantidad
    WHERE ID_Producto = @ID_Producto;
END
GO
```

### c. Auditoría de eliminaciones

```sql
CREATE TABLE Auditoria (
    ID_Auditoria INT IDENTITY(1,1) PRIMARY KEY,
    Usuario_SQL VARCHAR(100),
    Fecha DATETIME,
    Tabla_Afectada VARCHAR(100),
    Folio_Eliminado INT
);

CREATE TRIGGER trg_AuditoriaEliminacion
ON Producto, Proveedor, Cliente, Venta, Inventario
FOR DELETE
AS
BEGIN
    DECLARE @Usuario_SQL VARCHAR(100), @Fecha DATETIME, @Tabla_Afectada VARCHAR(100), @Folio_Eliminado INT;

    SET @Usuario_SQL = USER_NAME();
    SET @Fecha = GETDATE();
    SET @Tabla_Afectada = 'Producto';  -- Cambiar según la tabla afectada
    SET @Folio_Eliminado = (SELECT ID_Producto FROM DELETED);

    -- Registrar en la tabla de auditoría
    INSERT INTO Auditoria (Usuario_SQL, Fecha, Tabla_Afectada, Fol

io_Eliminado)
    VALUES (@Usuario_SQL, @Fecha, @Tabla_Afectada, @Folio_Eliminado);
END
GO
```

### d. Registrar movimientos de dinero

```sql
CREATE TABLE MovimientosDinero (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Fecha DATETIME,
    Movimiento VARCHAR(50),
    MontoTotal DECIMAL(10, 2)
);

CREATE TRIGGER trg_MovimientoDinero
ON VentaDetalle
AFTER INSERT, DELETE
AS
BEGIN
    DECLARE @Fecha DATETIME = GETDATE(),
            @Movimiento VARCHAR(50),
            @MontoTotal DECIMAL(10,2);

    IF EXISTS (SELECT * FROM INSERTED)
    BEGIN
        SET @Movimiento = 'INGRESO';
        SET @MontoTotal = (SELECT SUM(PrecioTotal) FROM INSERTED);
    END
    ELSE
    BEGIN
        SET @Movimiento = 'EGRESO';
        SET @MontoTotal = (SELECT SUM(PrecioTotal) FROM DELETED);
    END

    -- Guardar movimiento de dinero
    INSERT INTO MovimientosDinero (Fecha, Movimiento, MontoTotal)
    VALUES (@Fecha, @Movimiento, @MontoTotal);
END
GO
```