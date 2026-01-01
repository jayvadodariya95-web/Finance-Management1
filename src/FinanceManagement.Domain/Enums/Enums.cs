namespace FinanceManagement.Domain.Enums;

public enum UserRole
{
    Admin = 1,
    Partner = 2,
    Employee = 3
}

public enum ProjectStatus
{
    Active = 1,
    Completed = 2,
    OnHold = 3,
    Cancelled = 4
}

public enum TransactionType
{
    Income = 1,
    Expense = 2,
    Settlement = 3
}

public enum SettlementStatus
{
    Pending = 1,
    Completed = 2,
    Failed = 3
}

public enum ExpenseCategory
{
    Office = 1,
    Tools = 2,
    Marketing = 3,
    Travel = 4,
    Utilities = 5,
    Other = 6
}