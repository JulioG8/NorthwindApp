-- =============================================
-- Script: 01-CreateNorthwindDatabase.sql
-- Description: Creates Northwind database with Customers and Orders tables
-- =============================================

-- Create database
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'Northwind')
BEGIN
    CREATE DATABASE Northwind;
END
GO

USE Northwind;
GO

-- =============================================
-- Create Customers table
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Customers')
BEGIN
    CREATE TABLE Customers (
        CustomerID NCHAR(5) NOT NULL PRIMARY KEY,
        CompanyName NVARCHAR(40) NOT NULL,
        ContactName NVARCHAR(30) NULL,
        ContactTitle NVARCHAR(30) NULL,
        Address NVARCHAR(60) NULL,
        City NVARCHAR(15) NULL,
        Region NVARCHAR(15) NULL,
        PostalCode NVARCHAR(10) NULL,
        Country NVARCHAR(15) NULL,
        Phone NVARCHAR(24) NULL,
        Fax NVARCHAR(24) NULL
    );
END
GO

-- =============================================
-- Create Orders table
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Orders')
BEGIN
    CREATE TABLE Orders (
        OrderID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
        CustomerID NCHAR(5) NULL,
        EmployeeID INT NULL,
        OrderDate DATETIME NULL,
        RequiredDate DATETIME NULL,
        ShippedDate DATETIME NULL,
        ShipVia INT NULL,
        Freight MONEY NULL DEFAULT 0,
        ShipName NVARCHAR(40) NULL,
        ShipAddress NVARCHAR(60) NULL,
        ShipCity NVARCHAR(15) NULL,
        ShipRegion NVARCHAR(15) NULL,
        ShipPostalCode NVARCHAR(10) NULL,
        ShipCountry NVARCHAR(15) NULL,
        CONSTRAINT FK_Orders_Customers FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID)
    );
END
GO

-- =============================================
-- Create WebTracker table
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'WebTracker')
BEGIN
    CREATE TABLE WebTracker (
        Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
        URLRequest NVARCHAR(500) NOT NULL,
        SourceIp NVARCHAR(50) NOT NULL,
        TimeOfAction DATETIME NOT NULL DEFAULT GETUTCDATE()
    );

    CREATE NONCLUSTERED INDEX IX_WebTracker_TimeOfAction ON WebTracker (TimeOfAction DESC);
    CREATE NONCLUSTERED INDEX IX_WebTracker_SourceIp ON WebTracker (SourceIp);
END
GO

PRINT 'Database and tables created successfully!';
GO
