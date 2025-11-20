CREATE TABLE Customers (
    Id INT IDENTITY PRIMARY KEY,
    Name NVARCHAR(100),
    Email NVARCHAR(100) UNIQUE
);

CREATE TABLE Products (
    Id INT IDENTITY PRIMARY KEY,
    Name NVARCHAR(100),
    Price DECIMAL(10,2)
);

CREATE TABLE Orders (
    Id INT IDENTITY PRIMARY KEY,
    CustomerId INT,
    Status NVARCHAR(50),
    CreatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (CustomerId) REFERENCES Customers(Id)
);

CREATE TABLE OrderItems (
    OrderId INT,
    ProductId INT,
    Quantity INT CHECK(Quantity > 0),
    PRIMARY KEY (OrderId, ProductId),
    FOREIGN KEY (OrderId) REFERENCES Orders(Id),
    FOREIGN KEY (ProductId) REFERENCES Products(Id)
);
