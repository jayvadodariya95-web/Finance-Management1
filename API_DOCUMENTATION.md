# 🔗 API Documentation - Finance Management System

## Base URL
```
https://localhost:7001/api
```

## Authentication
All endpoints (except auth endpoints) require JWT Bearer token in Authorization header:
```
Authorization: Bearer <your-jwt-token>
```

## Response Format
All responses follow standardized format:
```json
{
  "success": true,
  "message": "Success message",
  "data": { /* response data */ },
  "errors": [],
  "timestamp": "2023-12-01T10:00:00Z"
}
```

---

## 🔐 Authentication Endpoints

### POST /api/auth/login
Login with email and password
```json
{
  "email": "admin@company.com",
  "password": "Admin123!"
}
```
**Response:**
```json
{
  "success": true,
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "user": {
      "id": 1,
      "firstName": "Admin",
      "lastName": "User",
      "email": "admin@company.com",
      "role": "Admin",
      "isActive": true
    }
  }
}
```

### POST /api/auth/refresh
Refresh JWT token
```json
{
  "token": "existing-jwt-token"
}
```

### POST /api/auth/register
Register new user
```json
{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john@company.com",
  "password": "Password123!",
  "role": 2
}
```

---

## 👥 User Management Endpoints

### GET /api/users
Get all users (Admin only)

### GET /api/users/{id}
Get user by ID

### PUT /api/users/{id}
Update user information

### DELETE /api/users/{id}
Delete user (soft delete)

---

## 🤝 Partner Management Endpoints

### GET /api/partners
Get all partners
**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": 1,
      "firstName": "John",
      "lastName": "Partner",
      "email": "partner1@company.com",
      "partnershipType": "Main",
      "sharePercentage": 33.33,
      "isMainPartner": true,
      "branchName": null
    }
  ]
}
```

### GET /api/partners/{id}
Get partner by ID

### POST /api/partners
Create new partner
```json
{
  "userId": 2,
  "partnershipType": "Main",
  "sharePercentage": 33.33,
  "isMainPartner": true,
  "branchId": null
}
```

### PUT /api/partners/{id}
Update partner information

### GET /api/partners/{id}/projects
Get all projects managed by specific partner

---

## 📊 Project Management Endpoints

### GET /api/projects
Get all projects
**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": 1,
      "name": "E-commerce Website",
      "clientName": "ABC Corp",
      "projectValue": 50000.00,
      "startDate": "2023-01-01T00:00:00Z",
      "endDate": "2023-06-30T00:00:00Z",
      "status": "Active",
      "managedByPartner": "John Partner",
      "assignedEmployees": []
    }
  ]
}
```

### GET /api/projects/{id}
Get project by ID

### POST /api/projects
Create new project
```json
{
  "name": "Mobile App Development",
  "description": "iOS and Android app",
  "clientName": "XYZ Company",
  "projectValue": 75000.00,
  "startDate": "2023-12-01T00:00:00Z",
  "endDate": "2024-05-31T00:00:00Z",
  "managedByPartnerId": 1
}
```

### PUT /api/projects/{id}
Update project information

### POST /api/projects/{id}/assign-employee
Assign employee to project
```json
{
  "employeeId": 3,
  "role": "Developer"
}
```

### GET /api/projects/partner/{partnerId}
Get all projects managed by specific partner

---

## 👨‍💼 Employee Management Endpoints

### GET /api/employees
Get all employees

### GET /api/employees/{id}
Get employee by ID

### POST /api/employees
Create new employee
```json
{
  "userId": 4,
  "employeeCode": "EMP001",
  "department": "Development",
  "position": "Senior Developer",
  "monthlySalary": 8000.00,
  "joinDate": "2023-01-15T00:00:00Z",
  "branchId": 1
}
```

### PUT /api/employees/{id}
Update employee information

### GET /api/employees/{id}/projects
Get all projects assigned to employee

---

## 🏢 Branch Management Endpoints

### GET /api/branches
Get all branches

### GET /api/branches/{id}
Get branch by ID

### POST /api/branches
Create new branch
```json
{
  "name": "New York Office",
  "location": "New York, NY",
  "description": "Main development office"
}
```

### PUT /api/branches/{id}
Update branch information

---

## 💰 Financial Management Endpoints

### GET /api/financial/monthly-report/{month}/{year}
Generate monthly financial report
**Example:** `/api/financial/monthly-report/12/2023`
**Response:**
```json
{
  "success": true,
  "data": {
    "month": 12,
    "year": 2023,
    "totalIncome": 150000.00,
    "totalExpenses": 25000.00,
    "totalSalaries": 45000.00,
    "netIncome": 80000.00,
    "partnerIncomes": [
      {
        "partnerId": 1,
        "partnerName": "John Partner",
        "expectedIncome": 200000.00,
        "actualIncome": 50000.00,
        "settlementAmount": -150000.00,
        "projectsManaged": 3
      }
    ],
    "expenses": []
  }
}
```

### GET /api/financial/partner-income/{partnerId}/{month}/{year}
Get specific partner's income for month
**Example:** `/api/financial/partner-income/1/12/2023`

### POST /api/financial/settlements/{month}/{year}
Process monthly settlements
**Example:** `/api/financial/settlements/12/2023`

### GET /api/financial/net-income/{month}/{year}
Calculate net income for month
**Example:** `/api/financial/net-income/12/2023`

### GET /api/financial/transactions
Get bank transactions with optional date filtering
**Query Parameters:**
- `startDate`: Filter from date (optional)
- `endDate`: Filter to date (optional)

**Example:** `/api/financial/transactions?startDate=2023-12-01&endDate=2023-12-31`

### POST /api/financial/transactions
Create new bank transaction
```json
{
  "bankAccountId": 1,
  "projectId": 2,
  "amount": 25000.00,
  "type": 1,
  "description": "Project payment received",
  "transactionDate": "2023-12-01T00:00:00Z"
}
```

---

## 🏦 Bank Account Endpoints

### GET /api/bankaccounts
Get all bank accounts

### GET /api/bankaccounts/{id}
Get bank account by ID

### POST /api/bankaccounts
Create new bank account
```json
{
  "accountNumber": "1234567890",
  "bankName": "First National Bank",
  "accountHolderName": "Company Name LLC",
  "balance": 100000.00,
  "currency": "USD"
}
```

### PUT /api/bankaccounts/{id}
Update bank account information

---

## 💸 Expense Management Endpoints

### GET /api/expenses
Get monthly expenses

### GET /api/expenses/{month}/{year}
Get expenses for specific month/year

### POST /api/expenses
Create new expense
```json
{
  "description": "Office rent",
  "amount": 5000.00,
  "category": 1,
  "month": 12,
  "year": 2023,
  "isRecurring": true
}
```

### PUT /api/expenses/{id}
Update expense

### POST /api/expenses/{id}/approve
Approve expense (Admin/Partner only)

---

## 🔄 Settlement Management Endpoints

### GET /api/settlements
Get all settlements

### GET /api/settlements/{partnerId}
Get settlements for specific partner

### GET /api/settlements/{month}/{year}
Get settlements for specific month/year

### POST /api/settlements
Create manual settlement
```json
{
  "partnerId": 1,
  "month": 12,
  "year": 2023,
  "expectedAmount": 200000.00,
  "actualAmount": 180000.00,
  "notes": "Manual adjustment for December"
}
```

### PUT /api/settlements/{id}/status
Update settlement status
```json
{
  "status": 2,
  "notes": "Settlement completed via bank transfer"
}
```

---

## 📈 Reporting Endpoints

### GET /api/reports/partner-performance/{partnerId}
Get partner performance report

### GET /api/reports/project-profitability/{projectId}
Get project profitability analysis

### GET /api/reports/monthly-summary/{month}/{year}
Get comprehensive monthly summary

### GET /api/reports/yearly-summary/{year}
Get yearly financial summary

---

## 🔍 Search and Filter Endpoints

### GET /api/search/projects
Search projects with filters
**Query Parameters:**
- `name`: Project name (partial match)
- `clientName`: Client name (partial match)
- `status`: Project status
- `partnerId`: Managed by partner ID
- `startDate`: Projects started after date
- `endDate`: Projects ended before date

### GET /api/search/employees
Search employees with filters
**Query Parameters:**
- `name`: Employee name (partial match)
- `department`: Department name
- `position`: Position title
- `branchId`: Branch ID

---

## 📊 Dashboard Data Endpoints

### GET /api/dashboard/summary
Get dashboard summary data
**Response:**
```json
{
  "success": true,
  "data": {
    "totalProjects": 15,
    "activeProjects": 8,
    "totalEmployees": 25,
    "monthlyRevenue": 150000.00,
    "monthlyExpenses": 70000.00,
    "pendingSettlements": 3,
    "recentTransactions": []
  }
}
```

### GET /api/dashboard/charts/revenue-trend
Get revenue trend data for charts

### GET /api/dashboard/charts/expense-breakdown
Get expense breakdown by category

### GET /api/dashboard/charts/partner-performance
Get partner performance comparison

---

## 🔧 System Administration Endpoints

### GET /api/admin/system-health
Check system health status (Admin only)

### GET /api/admin/audit-logs
Get system audit logs (Admin only)

### POST /api/admin/backup
Trigger system backup (Admin only)

### GET /api/admin/users/inactive
Get inactive users for cleanup (Admin only)

---

## 📝 Notes

### HTTP Status Codes
- `200 OK`: Successful GET, PUT requests
- `201 Created`: Successful POST requests
- `400 Bad Request`: Invalid request data
- `401 Unauthorized`: Missing or invalid authentication
- `403 Forbidden`: Insufficient permissions
- `404 Not Found`: Resource not found
- `500 Internal Server Error`: Server error

### Error Response Format
```json
{
  "success": false,
  "message": "Error description",
  "data": null,
  "errors": ["Detailed error 1", "Detailed error 2"],
  "timestamp": "2023-12-01T10:00:00Z"
}
```

### Pagination (where applicable)
```json
{
  "success": true,
  "data": {
    "items": [],
    "totalCount": 100,
    "pageNumber": 1,
    "pageSize": 20,
    "totalPages": 5
  }
}
```

### Rate Limiting
- 100 requests per minute per IP
- 1000 requests per hour per authenticated user
- Special limits for financial operations: 10 per minute

## Total Available Endpoints: 50+ APIs for comprehensive system management