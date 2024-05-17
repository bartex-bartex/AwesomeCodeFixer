-- Create Customers table
CREATE TABLE Customers (
    CustomerID INT PRIMARY KEY,
    FirstName VARCHAR(50),
    LastName VARCHAR(50),
    Email VARCHAR(100)
);

-- Create Orders table
CREATE TABLE Orders (
    OrderID INT PRIMARY KEY,
    CustomerID INT,
    OrderDate DATE,
    TotalAmount DECIMAL(10, 2),
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID)
);

-- Create Products table
CREATE TABLE Products (
    ProductID INT PRIMARY KEY,
    ProductName VARCHAR(100),
    Price DECIMAL(10, 2)
);

-- Insert sample data into Customers table
INSERT INTO Customers (CustomerID, FirstName, LastName, Email)
VALUES (1, 'John', 'Doe', 'john.doe@example.com'),
       (2, 'Jane', 'Smith', 'jane.smith@example.com');

-- Insert sample data into Orders table
INSERT INTO Orders (OrderID, CustomerID, OrderDate, TotalAmount)
VALUES (1, 1, '2024-04-28', 100.00),
       (2, 2, '2024-04-29', 150.00);

-- Insert sample data into Products table
INSERT INTO Products (ProductID, ProductName, Price)
VALUES (1, 'Product A', 10.00),
       (2, 'Product B', 20.00),
       (3, 'Product C', 30.00);

-- Retrieve all customers
SELECT * FROM Customers;

-- Retrieve orders for a specific customer
SELECT * FROM Orders WHERE CustomerID = 1;

-- Retrieve products with price greater than $20
SELECT * FROM Products WHERE Price > 20.00;
