using Microsoft.EntityFrameworkCore;
using FinanceManagement.Domain.Entities;
using System.Text;
using FinanceManagement.Application.DTOs;

namespace FinanceManagement.Infrastructure.Data;

public class FinanceDbContext : DbContext
{
    public FinanceDbContext(DbContextOptions<FinanceDbContext> options) : base(options)
    {
    }

    public DbSet<Asset> Asset { get; set; }
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
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<DocType> DocTypes { get; set; }
    public DbSet<Documents> Documents { get; set; }
    public DbSet<EmployeeDocument> EmployeeDocuments { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Revenue> Revenue { get; set; }
    // Store Procedure 

    public DbSet<TotalExpenseDTO> TotalExpenseDTO { get; set; }

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
            entity.HasIndex(e => e.EmployeeCode).IsUnique();
            entity.HasIndex(e => e.UserId);
            entity.HasOne(e => e.Branch)
                  .WithMany(b => b.Employees)
                  .HasForeignKey(e => e.BranchId);

            // BUG: Missing unique constraint on EmployeeCode
            // PERFORMANCE ISSUE: Missing index on UserId
        });

        // Project configuration
        modelBuilder.Entity<Project>(entity =>
        {
            entity.ToTable("Projects");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name)
                  .IsRequired()
                  .HasMaxLength(150);
            entity.Property(p => p.StartDate)
                  .IsRequired();
            entity.Property(p => p.TechnologyStack)
                  .HasMaxLength(150);
            entity.Property(p => p.ManagerName)
                  .HasMaxLength(50);
            entity.Property(p => p.ManagerEmail)
                  .HasMaxLength(50);
            entity.Property(p => p.ManagerContact)
                  .HasMaxLength(20);
            entity.Property(p => p.LeaveApplyWay)
                  .HasMaxLength(255);
            entity.Property(p => p.IsSmooth)
                  .HasDefaultValue(false);
            entity.Property(p => p.ProjectValue)
                  .HasColumnType("int");
            entity.Property(p => p.ClientManagerName)
                  .HasMaxLength(50);
            entity.Property(p => p.ClientManagerEmail)
                  .HasMaxLength(50);
            entity.Property(p => p.ClientManagerContact)
                  .HasMaxLength(20);
            entity.Property(p => p.MobileNumberUsed)
                  .HasMaxLength(25);
            entity.HasOne(p => p.Profile)
                  .WithMany()
                  .HasForeignKey(p => p.ProfileId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(p => p.InterviewingUser)
                  .WithMany()
                  .HasForeignKey(p => p.InterviewingUserId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(p => p.Name);
            entity.HasIndex(p => p.ProfileId);
            entity.HasIndex(p => p.InterviewingUserId);
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
                  .WithMany(emp => emp.ProjectEmployee)
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
            entity.Property(e => e.Month)
                  .IsRequired();
            entity.Property(e => e.Year)
                  .IsRequired();
            entity.Property(e => e.ExpectedAmount)
                  .HasPrecision(18, 2)
                  .IsRequired();
            entity.Property(e => e.ActualAmount)
                  .HasPrecision(18, 2)
                  .IsRequired();
            entity.Property(e => e.SettlementAmount)
                  .HasPrecision(18, 2)
                  .IsRequired();
            entity.Property(e => e.TotalExpense)
                  .HasPrecision(18, 2);
            entity.Property(e => e.GrossProfit)
                  .HasPrecision(18, 2);
            entity.Property(e => e.NetProfit)
                  .HasPrecision(18, 2);
            entity.Property(e => e.Status)
                  .HasConversion<int>()  // Enum as int
                  .IsRequired();
            entity.Property(e => e.Notes)
                  .HasMaxLength(1000);
            entity.Property(e => e.IsSetteled)
                  .HasDefaultValue(0);
            // Relationship
            entity.HasOne(e => e.Partner)
                  .WithMany(p => p.Settlements)
                  .HasForeignKey(e => e.PartnerId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Unique constraint (Partner + Month + Year)
            entity.HasIndex(e => new { e.PartnerId, e.Month, e.Year })
                  .IsUnique();
        });


        // MonthlyExpense configuration
        modelBuilder.Entity<MonthlyExpense>(entity =>
        {

            entity.HasKey(me => me.Id);
            entity.Property(me => me.Description)
                  .IsRequired()
                  .HasMaxLength(500);
            entity.Property(me => me.Amount)
                  .HasPrecision(18, 2)
                  .IsRequired();
            entity.Property(me => me.IsRecurring)
                  .HasDefaultValue(false);
            entity.Property(me => me.ApprovedBy)
                  .HasMaxLength(150);
            entity.HasOne(me => me.Partner)
                  .WithMany(p => p.MonthlyExpenses)
                  .HasForeignKey(me => me.PartnerId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(me => me.Employee)
                  .WithMany(e => e.MonthlyExpenses)
                  .HasForeignKey(me => me.EmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(me => me.Asset)
                  .WithMany(a => a.MonthlyExpenses)
                  .HasForeignKey(me => me.AssetId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Category)
                    .WithMany(c => c.MonthlyExpenses)
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            // PERFORMANCE ISSUE: Missing index on Month + Year for monthly reports
            // MonthlyExpense configuration
        });

        // BankAccount configuration
        modelBuilder.Entity<BankAccount>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.AccountNumber).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Balance).HasPrecision(18, 2);

            // BUG: Missing unique constraint on AccountNumber
        });
        // Doc-Type configuration
        modelBuilder.Entity<DocType>(entity =>
        {
            entity.ToTable("DocTypes");

            entity.HasKey(dt => dt.Id);

            entity.Property(dt => dt.TypeName)
                  .IsRequired()
                  .HasMaxLength(150);
            entity.HasIndex(dt => dt.TypeName)
                  .IsUnique();

            entity.HasMany(dt => dt.Documents)
                  .WithOne(d => d.DocType)
                  .HasForeignKey(d => d.DocType_Id)
                  .OnDelete(DeleteBehavior.Restrict);
        });
        //Documents configuration
        modelBuilder.Entity<Documents>(entity =>
        {
            entity.ToTable("Documents");

            entity.HasKey(d => d.Id);
            entity.Property(d => d.Link)
                  .HasMaxLength(500);

            entity.HasOne(d => d.DocType)
                  .WithMany(dt => dt.Documents)
                  .HasForeignKey(d => d.DocType_Id)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(d => d.DocType_Id);
        });

        //Renvenue to Project one-to-one configuration
        modelBuilder.Entity<Revenue>(entity =>
        {
            entity.HasOne(p => p.Project)
                .WithOne(r => r.Revenue)
                .HasForeignKey<Revenue>(r => r.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Revenue>(entity =>
        {
            entity.HasIndex(r => r.ProjectId).IsUnique();
        });

        //Revenue to Partner one-to-many configuration
        modelBuilder.Entity<Revenue>(entity =>
        {
            entity.HasOne(r => r.Partner)
            .WithMany(p => p.Revenues)
            .HasForeignKey(r => r.PartnerId);
        });

        //asset
        modelBuilder.Entity<Asset>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name)
                  .HasMaxLength(200);
            entity.Property(e => e.Amount)
                  .HasPrecision(18, 2);
        });
        // Profile configuration
        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.Property(p => p.IsPaid)
                  .HasDefaultValue(false);

            entity.Property(p => p.Amount)
                  .HasPrecision(18, 2);

            entity.HasOne(p => p.User)
                  .WithOne(u => u.Profile)
                  .HasForeignKey<Profile>(p => p.UserId);

            entity.HasMany(p => p.Projects)
                  .WithOne(pr => pr.Profile)
                  .HasForeignKey(pr => pr.ProfileId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(p => p.UserId)
                  .IsUnique();
        });
        // Employee_Document Configuration
        modelBuilder.Entity<EmployeeDocument>(entity =>
        {
            entity.HasKey(e => new { e.EmployeeId, e.DocumentId });
            entity.HasIndex(e => new { e.EmployeeId, e.DocumentId }).IsUnique();

            entity.HasOne(e => e.Employee)
            .WithMany(e => e.EmployeeDocuments)
            .HasForeignKey(e => e.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Documents)
            .WithMany(e => e.EmployeeDocuments)
            .HasForeignKey(e => e.DocumentId)
            .OnDelete(DeleteBehavior.Restrict);
        });
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CategoryName)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(e => e.IsRecurring)
                  .IsRequired();
        });
        // store procedure method
        modelBuilder.Entity<TotalExpenseDTO>().HasNoKey();
    }
}

// PERFORMANCE ISSUE: No query splitting configuration for related data
// BUG: Missing soft delete global query filter
// BUG: No audit trail configuration