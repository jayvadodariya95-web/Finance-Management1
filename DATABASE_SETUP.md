# 🗄️ Database Setup Guide

## Prerequisites
- SQL Server 2019+ or SQL Server Express
- SQL Server Management Studio (SSMS) - Optional but recommended
- .NET 8 SDK with EF Core tools

## Option 1: Code First with Migrations (Recommended)

### Step 1: Install EF Core Tools
```bash
dotnet tool install --global dotnet-ef
```

### Step 2: Navigate to Infrastructure Project
```bash
cd src/FinanceManagement.Infrastructure
```

### Step 3: Create Initial Migration
```bash
dotnet ef migrations add InitialCreate --startup-project ../FinanceManagement.API
```

### Step 4: Update Database
```bash
dotnet ef database update --startup-project ../FinanceManagement.API
```

### Step 5: Verify Database Creation
Check that `FinanceManagementDB` database is created with all tables.

---

## Option 2: Database First with SQL Scripts

### Complete Database Creation Script
```sql
-- Create Database
CREATE DATABASE FinanceManagementDB;
GO

USE FinanceManagementDB;
GO

-- Create Users Table
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL,
    PasswordHash NVARCHAR(MAX) NOT NULL,
    Role INT NOT NULL, -- 1=Admin, 2=Partner, 3=Employee
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NULL,
    CreatedBy NVARCHAR(255) NULL,
    UpdatedBy NVARCHAR(255) NULL,
    IsDeleted BIT NOT NULL DEFAULT 0
);

-- Create Branches Table
CREATE TABLE Branches (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    Location NVARCHAR(500) NOT NULL,
    Description NVARCHAR(1000) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NULL,
    CreatedBy NVARCHAR(255) NULL,
    UpdatedBy NVARCHAR(255) NULL,
    IsDeleted BIT NOT NULL DEFAULT 0
);

-- Create Partners Table
CREATE TABLE Partners (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    PartnershipType NVARCHAR(50) NOT NULL,
    SharePercentage DECIMAL(5,2) NOT NULL,
    BranchId INT NULL,
    IsMainPartner BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NULL,
    CreatedBy NVARCHAR(255) NULL,
    UpdatedBy NVARCHAR(255) NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    FOREIGN KEY (BranchId) REFERENCES Branches(Id)
);

-- Create Employees Table
CREATE TABLE Employees (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    EmployeeCode NVARCHAR(50) NOT NULL,
    Department NVARCHAR(100) NOT NULL,
    Position NVARCHAR(100) NOT NULL,
    MonthlySalary DECIMAL(18,2) NOT NULL,
    JoinDate DATETIME2 NOT NULL,
    BranchId INT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NULL,
    CreatedBy NVARCHAR(255) NULL,
    UpdatedBy NVARCHAR(255) NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    FOREIGN KEY (BranchId) REFERENCES Branches(Id)
);

-- Create Projects Table
CREATE TABLE Projects (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    Description NVARCHAR(1000) NULL,
    ClientName NVARCHAR(200) NOT NULL,
    ProjectValue DECIMAL(18,2) NOT NULL,
    StartDate DATETIME2 NOT NULL,
    EndDate DATETIME2 NULL,
    Status INT NOT NULL, -- 1=Active, 2=Completed, 3=OnHold, 4=Cancelled
    ManagedByPartnerId INT NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NULL,
    CreatedBy NVARCHAR(255) NULL,
    UpdatedBy NVARCHAR(255) NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (ManagedByPartnerId) REFERENCES Partners(Id)
);

-- Create ProjectEmployees Table (Many-to-Many)
CREATE TABLE ProjectEmployees (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ProjectId INT NOT NULL,
    EmployeeId INT NOT NULL,
    AssignedDate DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UnassignedDate DATETIME2 NULL,
    Role NVARCHAR(100) NULL,
    HourlyRate DECIMAL(18,2) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NULL,
    CreatedBy NVARCHAR(255) NULL,
    UpdatedBy NVARCHAR(255) NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (ProjectId) REFERENCES Projects(Id),
    FOREIGN KEY (EmployeeId) REFERENCES Employees(Id)
);

-- Create BankAccounts Table
CREATE TABLE BankAccounts (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    AccountNumber NVARCHAR(50) NOT NULL,
    BankName NVARCHAR(200) NOT NULL,
    AccountHolderName NVARCHAR(200) NOT NULL,
    Balance DECIMAL(18,2) NOT NULL DEFAULT 0,
    Currency NVARCHAR(10) NOT NULL DEFAULT 'USD',
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NULL,
    CreatedBy NVARCHAR(255) NULL,
    UpdatedBy NVARCHAR(255) NULL,
    IsDeleted BIT NOT NULL DEFAULT 0
);

-- Create BankTransactions Table
CREATE TABLE BankTransactions (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    BankAccountId INT NOT NULL,
    ProjectId INT NULL,
    Amount DECIMAL(18,2) NOT NULL,
    Type INT NOT NULL, -- 1=Income, 2=Expense, 3=Settlement
    Description NVARCHAR(500) NOT NULL,
    Reference NVARCHAR(100) NULL,
    TransactionDate DATETIME2 NOT NULL,
    IsProcessed BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NULL,
    CreatedBy NVARCHAR(255) NULL,
    UpdatedBy NVARCHAR(255) NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (BankAccountId) REFERENCES BankAccounts(Id),
    FOREIGN KEY (ProjectId) REFERENCES Projects(Id)
);

-- Create MonthlyExpenses Table
CREATE TABLE MonthlyExpenses (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Description NVARCHAR(500) NOT NULL,
    Amount DECIMAL(18,2) NOT NULL,
    Category INT NOT NULL, -- 1=Office, 2=Tools, 3=Marketing, 4=Travel, 5=Utilities, 6=Other
    Month INT NOT NULL,
    Year INT NOT NULL,
    IsRecurring BIT NOT NULL DEFAULT 0,
    ApprovedBy NVARCHAR(255) NULL,
    ApprovedDate DATETIME2 NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NULL,
    CreatedBy NVARCHAR(255) NULL,
    UpdatedBy NVARCHAR(255) NULL,
    IsDeleted BIT NOT NULL DEFAULT 0
);

-- Create Settlements Table
CREATE TABLE Settlements (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PartnerId INT NOT NULL,
    Month INT NOT NULL,
    Year INT NOT NULL,
    ExpectedAmount DECIMAL(18,2) NOT NULL,
    ActualAmount DECIMAL(18,2) NOT NULL,
    SettlementAmount DECIMAL(18,2) NOT NULL,
    Status INT NOT NULL, -- 1=Pending, 2=Completed, 3=Failed
    ProcessedDate DATETIME2 NULL,
    Notes NVARCHAR(1000) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NULL,
    CreatedBy NVARCHAR(255) NULL,
    UpdatedBy NVARCHAR(255) NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (PartnerId) REFERENCES Partners(Id)
);

-- Create Indexes for Performance (MISSING IN CURRENT IMPLEMENTATION - BUG!)
-- These indexes should be added to fix performance issues

-- Critical indexes for authentication and user lookup
CREATE INDEX IX_Users_Email ON Users(Email);
CREATE INDEX IX_Users_Role ON Users(Role);

-- Indexes for partner and employee lookups
CREATE INDEX IX_Partners_UserId ON Partners(UserId);
CREATE INDEX IX_Employees_UserId ON Employees(UserId);
CREATE INDEX IX_Employees_EmployeeCode ON Employees(EmployeeCode);

-- Indexes for project management
CREATE INDEX IX_Projects_ManagedByPartnerId ON Projects(ManagedByPartnerId);
CREATE INDEX IX_Projects_Status ON Projects(Status);
CREATE INDEX IX_ProjectEmployees_ProjectId ON ProjectEmployees(ProjectId);
CREATE INDEX IX_ProjectEmployees_EmployeeId ON ProjectEmployees(EmployeeId);

-- Critical indexes for financial queries
CREATE INDEX IX_BankTransactions_TransactionDate ON BankTransactions(TransactionDate);
CREATE INDEX IX_BankTransactions_ProjectId ON BankTransactions(ProjectId);
CREATE INDEX IX_BankTransactions_Type ON BankTransactions(Type);
CREATE INDEX IX_MonthlyExpenses_Month_Year ON MonthlyExpenses(Month, Year);
CREATE INDEX IX_Settlements_PartnerId ON Settlements(PartnerId);
CREATE INDEX IX_Settlements_Month_Year ON Settlements(Month, Year);

-- Composite indexes for common query patterns
CREATE INDEX IX_BankTransactions_Date_Type ON BankTransactions(TransactionDate, Type);
CREATE INDEX IX_Projects_Partner_Status ON Projects(ManagedByPartnerId, Status);

-- Unique constraints (MISSING IN CURRENT IMPLEMENTATION - BUG!)
ALTER TABLE Users ADD CONSTRAINT UQ_Users_Email UNIQUE (Email);
ALTER TABLE Employees ADD CONSTRAINT UQ_Employees_Code UNIQUE (EmployeeCode);
ALTER TABLE BankAccounts ADD CONSTRAINT UQ_BankAccounts_Number UNIQUE (AccountNumber);
ALTER TABLE ProjectEmployees ADD CONSTRAINT UQ_ProjectEmployees_Project_Employee UNIQUE (ProjectId, EmployeeId);
ALTER TABLE Settlements ADD CONSTRAINT UQ_Settlements_Partner_Month_Year UNIQUE (PartnerId, Month, Year);

-- Check constraints for data validation (MISSING IN CURRENT IMPLEMENTATION - BUG!)
ALTER TABLE Partners ADD CONSTRAINT CK_Partners_SharePercentage CHECK (SharePercentage >= 0 AND SharePercentage <= 100);
ALTER TABLE Projects ADD CONSTRAINT CK_Projects_ProjectValue CHECK (ProjectValue >= 0);
ALTER TABLE Employees ADD CONSTRAINT CK_Employees_MonthlySalary CHECK (MonthlySalary >= 0);
ALTER TABLE MonthlyExpenses ADD CONSTRAINT CK_MonthlyExpenses_Amount CHECK (Amount >= 0);
ALTER TABLE MonthlyExpenses ADD CONSTRAINT CK_MonthlyExpenses_Month CHECK (Month >= 1 AND Month <= 12);
ALTER TABLE BankTransactions ADD CONSTRAINT CK_BankTransactions_Amount CHECK (Amount > 0);

GO

-- Insert Sample Data
-- Default Admin User
INSERT INTO Users (FirstName, LastName, Email, PasswordHash, Role, IsActive)
VALUES 
('Admin', 'User', 'admin@company.com', '$2a$11$8gE7mKKW8yF5vF5vF5vF5uF5vF5vF5vF5vF5vF5vF5vF5vF5vF5vF', 1, 1),
('John', 'Partner', 'partner1@company.com', '$2a$11$8gE7mKKW8yF5vF5vF5vF5uF5vF5vF5vF5vF5vF5vF5vF5vF5vF5vF', 2, 1),
('Jane', 'Partner', 'partner2@company.com', '$2a$11$8gE7mKKW8yF5vF5vF5vF5uF5vF5vF5vF5vF5vF5vF5vF5vF5vF5vF', 2, 1),
('Mike', 'Partner', 'partner3@company.com', '$2a$11$8gE7mKKW8yF5vF5vF5vF5uF5vF5vF5vF5vF5vF5vF5vF5vF5vF5vF', 2, 1),
('Alice', 'Developer', 'employee1@company.com', '$2a$11$8gE7mKKW8yF5vF5vF5vF5uF5vF5vF5vF5vF5vF5vF5vF5vF5vF5vF', 3, 1),
('Bob', 'Designer', 'employee2@company.com', '$2a$11$8gE7mKKW8yF5vF5vF5vF5uF5vF5vF5vF5vF5vF5vF5vF5vF5vF5vF', 3, 1);

-- Branches
INSERT INTO Branches (Name, Location, Description, IsActive)
VALUES 
('Main Office', 'New York, NY', 'Headquarters and main development center', 1),
('West Coast Branch', 'San Francisco, CA', 'West coast operations and client services', 1);

-- Partners
INSERT INTO Partners (UserId, PartnershipType, SharePercentage, BranchId, IsMainPartner)
VALUES 
(2, 'Main', 33.33, NULL, 1),
(3, 'Main', 33.33, NULL, 1),
(4, 'Main', 33.34, NULL, 1);

-- Employees
INSERT INTO Employees (UserId, EmployeeCode, Department, Position, MonthlySalary, JoinDate, BranchId, IsActive)
VALUES 
(5, 'EMP001', 'Development', 'Senior Developer', 8000.00, '2023-01-15', 1, 1),
(6, 'EMP002', 'Design', 'UI/UX Designer', 6500.00, '2023-02-01', 1, 1);

-- Bank Accounts
INSERT INTO BankAccounts (AccountNumber, BankName, AccountHolderName, Balance, Currency, IsActive)
VALUES 
('1234567890', 'First National Bank', 'Finance Management Company LLC', 250000.00, 'USD', 1),
('0987654321', 'Business Bank', 'Finance Management Company LLC', 150000.00, 'USD', 1);

-- Sample Projects
INSERT INTO Projects (Name, Description, ClientName, ProjectValue, StartDate, EndDate, Status, ManagedByPartnerId)
VALUES 
('E-commerce Platform', 'Complete e-commerce solution with payment integration', 'ABC Corporation', 75000.00, '2023-01-01', '2023-06-30', 2, 1),
('Mobile Banking App', 'iOS and Android banking application', 'XYZ Bank', 120000.00, '2023-03-01', '2023-12-31', 1, 2),
('CRM System', 'Customer relationship management system', 'DEF Company', 95000.00, '2023-06-01', '2024-02-29', 1, 3),
('Inventory Management', 'Warehouse inventory tracking system', 'GHI Logistics', 60000.00, '2023-09-01', '2024-03-31', 1, 1),
('Data Analytics Dashboard', 'Business intelligence and reporting dashboard', 'JKL Analytics', 85000.00, '2023-11-01', '2024-05-31', 1, 2);

-- Project Employee Assignments
INSERT INTO ProjectEmployees (ProjectId, EmployeeId, AssignedDate, Role, HourlyRate, IsActive)
VALUES 
(1, 1, '2023-01-01', 'Lead Developer', 75.00, 1),
(1, 2, '2023-01-15', 'UI Designer', 60.00, 1),
(2, 1, '2023-03-01', 'Senior Developer', 75.00, 1),
(3, 1, '2023-06-01', 'Technical Lead', 80.00, 1),
(3, 2, '2023-06-15', 'UX Designer', 60.00, 1);

-- Sample Bank Transactions
INSERT INTO BankTransactions (BankAccountId, ProjectId, Amount, Type, Description, TransactionDate, IsProcessed)
VALUES 
(1, 1, 37500.00, 1, 'E-commerce Platform - First milestone payment', '2023-02-15', 1),
(1, 1, 37500.00, 1, 'E-commerce Platform - Final payment', '2023-06-30', 1),
(1, 2, 60000.00, 1, 'Mobile Banking App - Initial payment', '2023-03-15', 1),
(1, 3, 47500.00, 1, 'CRM System - Milestone 1', '2023-07-15', 1),
(2, 4, 30000.00, 1, 'Inventory Management - Down payment', '2023-09-15', 1),
(1, NULL, 5000.00, 2, 'Office rent - January 2023', '2023-01-01', 1),
(1, NULL, 2500.00, 2, 'Software licenses', '2023-01-15', 1);

-- Monthly Expenses
INSERT INTO MonthlyExpenses (Description, Amount, Category, Month, Year, IsRecurring, ApprovedBy, ApprovedDate)
VALUES 
('Office Rent', 5000.00, 1, 1, 2023, 1, 'admin@company.com', '2023-01-01'),
('Software Licenses', 2500.00, 2, 1, 2023, 1, 'admin@company.com', '2023-01-01'),
('Marketing Campaign', 3000.00, 3, 2, 2023, 0, 'admin@company.com', '2023-02-01'),
('Office Supplies', 800.00, 1, 2, 2023, 0, 'admin@company.com', '2023-02-15'),
('Utilities', 1200.00, 5, 2, 2023, 1, 'admin@company.com', '2023-02-01');

-- Sample Settlements (with intentional calculation bugs for practice)
INSERT INTO Settlements (PartnerId, Month, Year, ExpectedAmount, ActualAmount, SettlementAmount, Status, ProcessedDate, Notes)
VALUES 
(1, 1, 2023, 200000.00, 175000.00, -25000.00, 2, '2023-02-01', 'January settlement - below expected'),
(2, 1, 2023, 200000.00, 225000.00, 25000.00, 2, '2023-02-01', 'January settlement - above expected'),
(3, 1, 2023, 200000.00, 200000.00, 0.00, 2, '2023-02-01', 'January settlement - on target');

GO

PRINT 'Database setup completed successfully!';
PRINT 'Total Tables Created: 10';
PRINT 'Total Indexes Created: 15';
PRINT 'Total Constraints Added: 8';
PRINT 'Sample Data Inserted: Ready for testing';
```

## Verification Queries

### Check All Tables
```sql
SELECT TABLE_NAME 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_NAME;
```

### Check Sample Data
```sql
-- Verify users
SELECT Id, FirstName, LastName, Email, Role FROM Users;

-- Verify projects
SELECT Id, Name, ClientName, ProjectValue, Status FROM Projects;

-- Verify financial data
SELECT COUNT(*) as TransactionCount FROM BankTransactions;
SELECT SUM(Amount) as TotalIncome FROM BankTransactions WHERE Type = 1;
```

### Performance Check
```sql
-- Check indexes
SELECT 
    i.name AS IndexName,
    t.name AS TableName,
    c.name AS ColumnName
FROM sys.indexes i
INNER JOIN sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
INNER JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
INNER JOIN sys.tables t ON i.object_id = t.object_id
WHERE i.is_primary_key = 0
ORDER BY t.name, i.name;
```

## Connection String Examples

### LocalDB (Default)
```
Server=(localdb)\\mssqllocaldb;Database=FinanceManagementDB;Trusted_Connection=true;MultipleActiveResultSets=true
```

### SQL Server Express
```
Server=.\\SQLEXPRESS;Database=FinanceManagementDB;Trusted_Connection=true;MultipleActiveResultSets=true
```

### SQL Server with Authentication
```
Server=localhost;Database=FinanceManagementDB;User Id=sa;Password=YourPassword;MultipleActiveResultSets=true
```

## Troubleshooting

### Common Issues
1. **Connection Failed**: Check SQL Server service is running
2. **Database Not Found**: Verify connection string and database creation
3. **Permission Denied**: Ensure user has appropriate database permissions
4. **Migration Errors**: Delete migrations folder and recreate if needed

### Reset Database
```sql
USE master;
GO
DROP DATABASE IF EXISTS FinanceManagementDB;
GO
-- Then run the creation script again
```