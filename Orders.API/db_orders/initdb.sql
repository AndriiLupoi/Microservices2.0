-- Створення бази, якщо її ще немає
IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'OrdersDb')
BEGIN
    CREATE DATABASE OrdersDb;
END

-- Використовуємо OrdersDb
DECLARE @sql NVARCHAR(MAX) = N'USE OrdersDb; ' + 
N'
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = ''Customers'')
BEGIN
    CREATE TABLE Customers (
        Id INT IDENTITY PRIMARY KEY,
        Name NVARCHAR(100),
        Email NVARCHAR(100) UNIQUE
    );
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = ''Products'')
BEGIN
    CREATE TABLE Products (
        Id INT IDENTITY PRIMARY KEY,
        Name NVARCHAR(100),
        Price DECIMAL(10,2)
    );
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = ''Orders'')
BEGIN
    CREATE TABLE Orders (
        Id INT IDENTITY PRIMARY KEY,
        CustomerId INT,
        Status NVARCHAR(50),
        CreatedAt DATETIME DEFAULT GETDATE(),
        FOREIGN KEY (CustomerId) REFERENCES Customers(Id)
    );
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = ''OrderItems'')
BEGIN
    CREATE TABLE OrderItems (
        OrderId INT,
        ProductId INT,
        Quantity INT CHECK(Quantity > 0),
        PRIMARY KEY (OrderId, ProductId),
        FOREIGN KEY (OrderId) REFERENCES Orders(Id),
        FOREIGN KEY (ProductId) REFERENCES Products(Id)
    );
END
';

EXEC sp_executesql @sql;
