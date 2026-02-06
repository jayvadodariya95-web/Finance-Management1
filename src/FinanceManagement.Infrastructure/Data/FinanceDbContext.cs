using Microsoft.EntityFrameworkCore;
using FinanceManagement.Domain.Entities;

namespace FinanceManagement.Infrastructure.Data;

public class FinanceDbContext : DbContext
{
    public FinanceDbContext(DbContextOptions<FinanceDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Partner> Partners { get; set; }
    public DbSet<Branch> Branches { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectEmployee> ProjectEmployees { get; set; }
    public DbSet<BankAccount> BankAccounts { get; set; }
    public DbSet<BankTransaction> BankTransactions { get; set; }
    public DbSet<MonthlyExpense> MonthlyExpenses { get; set; }
    public DbSet<Settlement> Settlements { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.PasswordHash).IsRequired();
            
            // BUG: Missing unique constraint on Email
            // PERFORMANCE ISSUE: Missing index on Email for login queries
        });

        // Partner configuration
        modelBuilder.Entity<Partner>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.User)
                  .WithOne(u => u.Partner)
                  .HasForeignKey<Partner>(e => e.UserId);
            entity.HasOne(e => e.Branch)
                  .WithMany(b => b.Partners)
                  .HasForeignKey(e => e.BranchId);
            
            // BUG: Missing index on UserId - will cause slow queries
            // BUG: No validation for SharePercentage range
        });

        // Employee configuration
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.User)
                  .WithOne(u => u.Employee)
                  .HasForeignKey<Employee>(e => e.UserId);
            entity.HasOne(e => e.Branch)
                  .WithMany(b => b.Employees)
                  .HasForeignKey(e => e.BranchId);
            
            // BUG: Missing unique constraint on EmployeeCode
            // PERFORMANCE ISSUE: Missing index on UserId
        });

        // Project configuration
        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.ClientName).IsRequired().HasMaxLength(200);
            entity.HasOne(e => e.ManagedByPartner)
                  .WithMany(p => p.ManagedProjects)
                  .HasForeignKey(e => e.ManagedByPartnerId);
            
            // PERFORMANCE ISSUE: Missing index on ManagedByPartnerId
            // BUG: No check constraint for ProjectValue > 0
        });

        // ProjectEmployee many-to-many configuration
        modelBuilder.Entity<ProjectEmployee>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Project)
                  .WithMany(p => p.ProjectEmployees)
                  .HasForeignKey(e => e.ProjectId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Employee)
                  .WithMany(emp => emp.ProjectAssignments)
                  .HasForeignKey(e => e.EmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);
            
            // BUG: Missing unique constraint on ProjectId + EmployeeId combination
            // PERFORMANCE ISSUE: Missing composite index
        });

        // BankTransaction configuration
        modelBuilder.Entity<BankTransaction>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Amount).HasPrecision(18, 2);
            entity.HasOne(e => e.BankAccount)
                  .WithMany(ba => ba.Transactions)
                  .HasForeignKey(e => e.BankAccountId);
            entity.HasOne(e => e.Project)
                  .WithMany(p => p.BankTransactions)
                  .HasForeignKey(e => e.ProjectId);
            
            // PERFORMANCE ISSUE: Missing index on TransactionDate for date range queries
            // BUG: No concurrency token for preventing double processing
        });

        // Settlement configuration
        modelBuilder.Entity<Settlement>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ExpectedAmount).HasPrecision(18, 2);
            entity.Property(e => e.ActualAmount).HasPrecision(18, 2);
            entity.Property(e => e.SettlementAmount).HasPrecision(18, 2);
            entity.HasOne(e => e.Partner)
                  .WithMany(p => p.Settlements)
                  .HasForeignKey(e => e.PartnerId);
            
            // BUG: Missing unique constraint on PartnerId + Month + Year
        });

        // MonthlyExpense configuration
        modelBuilder.Entity<MonthlyExpense>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Amount).HasPrecision(18, 2);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(500);
            
            // PERFORMANCE ISSUE: Missing index on Month + Year for monthly reports
        });

        // BankAccount configuration
        modelBuilder.Entity<BankAccount>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.AccountNumber).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Balance).HasPrecision(18, 2);
            
            // BUG: Missing unique constraint on AccountNumber
        });
    }
}

// PERFORMANCE ISSUE: No query splitting configuration for related data
// BUG: Missing soft delete global query filter
// BUG: No audit trail configuration