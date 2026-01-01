# Finance & Company Management System - .NET Practice Project

## 🎯 Project Overview
Real-world .NET Core Web API practice project simulating a company financial & project management system with **subtle runtime bugs and performance issues** for advanced learning and debugging practice.

## 🏗️ Architecture
- **Clean Architecture** (Domain, Application, Infrastructure, API)
- **CQRS** with MediatR
- **JWT Authentication & Role-based Authorization**
- **Entity Framework Core** with SQL Server
- **Global Exception Handling**
- **Standardized API Response Wrapper**

## 🚀 Quick Setup

### Prerequisites
- .NET 8 SDK
- SQL Server (LocalDB or Express)
- Visual Studio 2022 or VS Code

### Setup Steps
1. Clone/Download the project
2. Navigate to project directory
3. Update connection string in `appsettings.json`
4. Run migrations:
   ```bash
   dotnet ef database update --project src/FinanceManagement.Infrastructure --startup-project src/FinanceManagement.API
   ```
5. Run the project:
   ```bash
   dotnet run --project src/FinanceManagement.API
   ```
6. Access Swagger: `https://localhost:7001/swagger`

### Default Login Credentials
- **Admin**: admin@company.com / Admin123!
- **Partner**: partner1@company.com / Partner123!

## 📊 Domain Rules

### Company Structure
- 3 main partners + 2 branches (each can have additional partner)
- Partners share responsibility for bank accounts, project income, expense settlement
- Employees work on multiple projects (many-to-many)

### Financial Logic
```
Net Income = Total Project Income - Monthly Expenses - Employee Salaries
```

### Settlement Rules
- Each partner manages different projects
- Monthly settlement handles unequal income distribution
- Example: Expected 200k, Actual 250k → Extra 50k split correctly

## 🐛 Intentional Issues (For Advanced Learning)

### Runtime Exception Bugs
- **Null Reference Exceptions** in partner data access
- **Index Out of Range** in date operations
- **Division by Zero** in financial calculations
- **Type Conversion Failures** in data processing

### Performance Issues
- **N+1 Query Problems** in project listings
- **Missing Database Indexes** causing slow queries
- **Memory Leaks** in large dataset operations
- **Client-Side Calculations** instead of database aggregation

### Business Logic Bugs
- **Incorrect Settlement Calculations** ignoring partner shares
- **Double Expense Counting** in financial reports
- **Race Conditions** in concurrent operations
- **Data Integrity Issues** with duplicate records

### Security & Authorization
- **Missing Role-Based Authorization** on sensitive endpoints
- **Data Access Control** gaps allowing unauthorized access
- **Input Validation** weaknesses
- **Async/Await** misuse causing deadlocks

## 📋 Available APIs (30+ endpoints)

### Authentication
- POST /api/auth/login
- POST /api/auth/refresh
- POST /api/auth/register

### Partners & Projects
- GET /api/partners
- GET /api/projects
- POST /api/projects
- POST /api/projects/{id}/assign-employee

### Financial Management
- GET /api/financial/monthly-report/{month}/{year}
- POST /api/financial/settlements/{month}/{year}
- GET /api/financial/transactions
- POST /api/financial/transactions

## 🎯 Learning Objectives

### Bug Detection & Fixing
- **Runtime Exception Handling**: Identify and fix null reference exceptions
- **Performance Optimization**: Resolve N+1 queries and memory issues
- **Business Logic Debugging**: Correct financial calculation errors
- **Security Hardening**: Implement proper authorization controls

### Advanced Concepts
- **Async/Await Patterns**: Fix blocking operations and deadlocks
- **Database Optimization**: Add indexes and optimize queries
- **Concurrency Control**: Handle race conditions and data integrity
- **Error Handling**: Implement robust exception management

## 📁 Project Structure
```
src/
├── FinanceManagement.API/          # Web API layer
├── FinanceManagement.Application/  # Business logic & CQRS
├── FinanceManagement.Domain/       # Domain entities & rules
└── FinanceManagement.Infrastructure/ # Data access & external services
```

## 🔧 Technologies Used
- .NET 8 Web API
- Entity Framework Core
- MediatR (CQRS)
- FluentValidation
- JWT Authentication
- Serilog
- SQL Server

## 📖 Practice Materials
- **BUGS.md**: 22+ runtime and subtle bugs with reproduction steps
- **TASKS.md**: 55+ development tasks for skill building
- **API_DOCUMENTATION.md**: Complete API reference
- **DATABASE_SETUP.md**: Database scripts and setup guide

## ⚠️ Important Notes

### This is a Learning Project
- Contains **intentional bugs** for educational purposes
- **Not production-ready** - requires bug fixes and security improvements
- Designed for **debugging practice** and **skill development**

### Build Status
✅ **Project builds successfully** with warnings (intentional)
✅ **Runtime exceptions** will occur during testing (by design)
✅ **Performance issues** present for optimization practice

### Getting Started with Bug Hunting
1. Run the application and test various endpoints
2. Monitor for null reference exceptions and performance issues
3. Check the BUGS.md file for detailed reproduction steps
4. Use debugging tools to identify root causes
5. Implement fixes following best practices

This project provides **real-world complexity** with intentional issues that mirror actual production scenarios developers encounter daily.