-- =============================================
-- Script: CreateWebTrackerTable.sql
-- Description: Creates the WebTracker table for logging all site requests
-- Author: Technical Interview Task
-- Database: Northwind
-- =============================================

USE [Northwind]
GO

-- Check if table exists and drop it
IF OBJECT_ID('dbo.WebTracker', 'U') IS NOT NULL
BEGIN
    DROP TABLE dbo.WebTracker;
    PRINT 'Existing WebTracker table dropped.';
END
GO

-- Create WebTracker table
CREATE TABLE dbo.WebTracker
(
    Id              INT IDENTITY(1,1)   NOT NULL,
    URLRequest      NVARCHAR(500)       NOT NULL,
    SourceIp        NVARCHAR(50)        NOT NULL,
    TimeOfAction    DATETIME            NOT NULL DEFAULT GETUTCDATE(),

    CONSTRAINT PK_WebTracker PRIMARY KEY CLUSTERED (Id ASC)
);
GO

-- Create index on TimeOfAction for efficient querying by date
CREATE NONCLUSTERED INDEX IX_WebTracker_TimeOfAction
ON dbo.WebTracker (TimeOfAction DESC);
GO

-- Create index on SourceIp for querying by IP address
CREATE NONCLUSTERED INDEX IX_WebTracker_SourceIp
ON dbo.WebTracker (SourceIp);
GO

PRINT 'WebTracker table created successfully.';
GO

-- =============================================
-- Sample data for testing (optional)
-- =============================================
/*
INSERT INTO dbo.WebTracker (URLRequest, SourceIp, TimeOfAction)
VALUES
    ('customers/country/USA', '192.168.1.1', GETUTCDATE()),
    ('customers/ALFKI', '192.168.1.1', GETUTCDATE()),
    ('orders/customer/ALFKI', '192.168.1.1', GETUTCDATE());
*/

-- =============================================
-- Query to view recent entries
-- =============================================
/*
SELECT TOP 100
    Id,
    URLRequest,
    SourceIp,
    TimeOfAction
FROM dbo.WebTracker
ORDER BY TimeOfAction DESC;
*/
