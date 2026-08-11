CREATE DATABASE GreenLifeDB

USE GreenLifeDB

CREATE TABLE Products (
    ProductId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100),
    Category NVARCHAR(100),
    Price DECIMAL(10,2),
    Stock INT,
    Discount FLOAT
);

CREATE TABLE Orders (
    OrderId INT IDENTITY(1,1) PRIMARY KEY,
    CustomerId INT,
    Status NVARCHAR(50),
    OrderDate DATETIME
);

CREATE TABLE OrderItems (
    OrderItemId INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT,
    ProductId INT,
	Quantity INT NOT NULL
);

CREATE TABLE ProductReviews (
    ReviewId INT PRIMARY KEY IDENTITY(1,1),
    ProductId INT FOREIGN KEY REFERENCES Products(ProductId),
    UserId INT FOREIGN KEY REFERENCES Users(UserId),
    Rating INT CHECK (Rating BETWEEN 1 AND 5),
    Comment NVARCHAR(500),
    ReviewDate DATETIME DEFAULT GETDATE()
);

SELECT * FROM Users

SELECT * FROM Products

SELECT * FROM Orders

SELECT * FROM OrderItems

SELECT * FROM ProductReviews