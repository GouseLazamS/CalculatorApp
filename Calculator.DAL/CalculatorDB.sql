IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'CalculatorDB')
BEGIN
    CREATE DATABASE CalculatorDB;
END
GO

USE CalculatorDB
GO

IF OBJECT_ID('CalculationHistory') IS NOT NULL DROP TABLE CalculationHistory
GO

IF OBJECT_ID('dbo.fn_Factorial') IS NOT NULL DROP FUNCTION dbo.fn_Factorial
GO

IF OBJECT_ID('sp_ExecuteAndLogCalculation') IS NOT NULL DROP PROCEDURE sp_ExecuteAndLogCalculation
GO
-- Create Table for History with Performance Indexing
CREATE TABLE CalculationHistory (
    CalculationID INT IDENTITY(1,1) PRIMARY KEY,
    Expression NVARCHAR(MAX) NOT NULL,
    Result FLOAT NOT NULL,
    OperationType NVARCHAR(20), -- e.g., 'Scientific', 'Standard'
    CreatedAt DATETIMEOFFSET DEFAULT SYSDATETIMEOFFSET()
);
GO
-- Index for faster retrieval of recent calculations
CREATE INDEX IX_CreatedAt ON CalculationHistory (CreatedAt DESC);
GO

-- Function for Factorial (Scientific)
CREATE FUNCTION dbo.fn_Factorial (@n INT)
RETURNS BIGINT
AS
BEGIN
    DECLARE @i INT = 1, @res BIGINT = 1;
    WHILE @i <= @n BEGIN
        SET @res = @res * @i;
        SET @i = @i + 1;
    END
    RETURN @res;
END;
GO

CREATE PROCEDURE sp_ExecuteAndLogCalculation
    @Expression NVARCHAR(MAX),
    @Result FLOAT,
    @OpType NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON; -- Improves performance by stopping "rows affected" messages
    
    BEGIN TRANSACTION;
    BEGIN TRY
        -- Insert into history
        INSERT INTO CalculationHistory (Expression, Result, OperationType)
        VALUES (@Expression, @Result, @OpType);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW 50001, 'Failed to log calculation. Transaction rolled back.', 1;
    END CATCH
END;
GO
