using Microsoft.EntityFrameworkCore;
using FinanceManagement.Domain.Entities;

namespace FinanceManagement.Infrastructure.Data;

public class FinanceDbContext : DbContext
{
    public FinanceDbContext(DbContextOptions<FinanceDbContext> options) : base(options)
    {
    }

    public DbSet<EmployeeDocument> EmployeeDocument { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<DocType> DocTypes { get; set; }
    public DbSet<Documents> Documents { get; set; }
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<Revenue> Revenues { get; set; }
    public DbSet<Asset> Assets { get; set; }
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
            entity.Property(e => e.UserName)
                  .HasMaxLength(50);
            entity.Property(e => e.FirstName)
                  .IsRequired()
                  .HasMaxLength(100);
            entity.Property(e => e.LastName)
                  .IsRequired()
                  .HasMaxLength(100);
            entity.Property(e => e.Email)
                  .IsRequired()
                  .HasMaxLength(255);
            entity.Property(e => e.PasswordHash)
                  .IsRequired();
            entity.Property(e => e.EmergencyMobileNumber)
                  .HasMaxLength(15);
            entity.Property(e => e.IsActive)
                  .HasDefaultValue(true);
            entity.Property(e => e.Gender)
                  .HasConversion<int>();
            entity.Property(e => e.Role)
                  .HasConversion<int>();
            entity.HasIndex(e => e.Email)
                  .IsUnique();
            entity.HasIndex(e => e.UserName);
            entity.HasOne(e => e.Partner)
                  .WithOne(p => p.User)
                  .HasForeignKey<Partner>(p => p.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Employee)
                  .WithOne(emp => emp.User)
                  .HasForeignKey<Employee>(emp => emp.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Partner configuration
        modelBuilder.Entity<Partner>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PartnershipType)
                  .IsRequired()
                  .HasMaxLength(100);
            entity.Property(e => e.SharePercentage)
                  .HasPrecision(5, 2)
                  .IsRequired();
            entity.Property(e => e.IsMainPartner)
                  .HasDefaultValue(false);
            entity.HasIndex(e => e.UserId)
                  .IsUnique();
            entity.HasIndex(e => e.BranchId);
            entity.HasOne(e => e.User)
                  .WithOne(u => u.Partner)
                  .HasForeignKey<Partner>(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Branch)
                  .WithMany(b => b.Partners)
                  .HasForeignKey(e => e.BranchId)
                  .OnDelete(DeleteBehavior.SetNull);
            entity.HasMany(e => e.ManagedProjects)
                  .WithOne(p => p.ManagedByPartner)
                  .HasForeignKey(p => p.ManagedByPartnerId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasMany(e => e.Settlements)
                  .WithOne(s => s.Partner)
                  .HasForeignKey(s => s.PartnerId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(e => e.MonthlyExpenses)
                  .WithOne(me => me.Partner)
                  .HasForeignKey(me => me.PartnerId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        //Assest configuration
        modelBuilder.Entity<Asset>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name)
                  .IsRequired()
                  .HasMaxLength(150);
            entity.Property(e => e.Description)
                  .HasMaxLength(500);
            entity.Property(e => e.Amount)
                  .HasPrecision(18, 2)
                  .IsRequired();
            entity.Property(e => e.Purchase_Date)
                  .IsRequired();
            entity.HasMany(e => e.MonthlyExpenses)
                  .WithOne(me => me.Asset)
                  .HasForeignKey(me => me.AssetId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Employee configuration
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("Employees");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.EmployeeCode)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.HasIndex(e => e.EmployeeCode)
                  .IsUnique();

            entity.Property(e => e.Position)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(e => e.MonthlySalary)
                  .IsRequired();

            entity.Property(e => e.Previous_CTC);

            entity.Property(e => e.Current_CTC)
                  .IsRequired();

            entity.Property(e => e.JoinDate)
                  .IsRequired();

            entity.Property(e => e.Taken_Leave);

            entity.Property(e => e.IsActive)
                  .HasDefaultValue(true);

            // 🔹 User relationship (1-1)
            entity.HasOne(e => e.User)
                  .WithOne()
                  .HasForeignKey<Employee>(e => e.UserId)
                  .OnDelete(DeleteBehavior.Restrict);

            // 🔹 Branch relationship (Many employees → One branch)
            entity.HasOne(e => e.Branch)
                  .WithMany(b => b.Employees)
                  .HasForeignKey(e => e.BranchId)
                  .OnDelete(DeleteBehavior.Restrict);

            // 🔹 ProjectEmployee (Many-to-Many via join table)
            entity.HasMany(e => e.ProjectAssignments)
                  .WithOne(pe => pe.Employee)
                  .HasForeignKey(pe => pe.EmployeeId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.Property(e => e.MonthlySalary).HasPrecision(18, 2);
            entity.Property(e => e.Previous_CTC).HasPrecision(18, 2);
            entity.Property(e => e.Current_CTC).HasPrecision(18, 2);

        });

        // Project configuration
        modelBuilder.Entity<Project>(entity =>
        {
            entity.ToTable("Projects");

            entity.HasKey(p => p.Id);

            entity.Property(p => p.Name)
                  .IsRequired()
                  .HasMaxLength(200);

            entity.Property(p => p.TechnologyStack)
                  .HasMaxLength(200);

            entity.Property(p => p.Description)
                  .HasMaxLength(1000);

            entity.Property(p => p.ManagerName)
                  .IsRequired()
                  .HasMaxLength(150);

            entity.Property(p => p.ManagerEmail)
                  .IsRequired()
                  .HasMaxLength(150);

            entity.Property(p => p.ManagerContact)
                  .IsRequired()
                  .HasMaxLength(20);

            entity.Property(p => p.ClientManagerName)
                  .IsRequired()
                  .HasMaxLength(150);

            entity.Property(p => p.ClientManagerEmail)
                  .IsRequired()
                  .HasMaxLength(150);

            entity.Property(p => p.ClientManagerContact)
                  .IsRequired()
                  .HasMaxLength(20);

            entity.Property(p => p.MobileNumberUsed)
                  .HasMaxLength(20);

            entity.Property(p => p.ProjectValue)
                  .HasPrecision(18, 2)
                  .IsRequired();

            entity.Property(p => p.LeaveApplyWay)
                  .HasMaxLength(250);

            entity.Property(p => p.Status)
                  .IsRequired()
                  .HasConversion<int>();

            entity.Property(p => p.IsSmooth)
                  .HasDefaultValue(false);

            entity.Property(p => p.IsToolUsed);

            entity.Property(p => p.StartDate)
                  .IsRequired();

            // 🔹 Profile (Many Projects → One Profile)
            entity.HasOne(p => p.Profile)
                  .WithMany(pr => pr.Projects)
                  .HasForeignKey(p => p.ProfileId)
                  .OnDelete(DeleteBehavior.Restrict);

            // 🔹 Partner (Many Projects → One Partner)
            entity.HasOne(p => p.ManagedByPartner)
                  .WithMany(pt => pt.ManagedProjects)
                  .HasForeignKey(p => p.ManagedByPartnerId)
                  .OnDelete(DeleteBehavior.Restrict);

            // 🔹 ProjectEmployees (Many-to-Many via join table)
            entity.HasMany(p => p.ProjectEmployees)
                  .WithOne(pe => pe.Project)
                  .HasForeignKey(pe => pe.ProjectId)
                  .OnDelete(DeleteBehavior.Cascade);

            // 🔹 BankTransactions (One Project → Many Transactions)
            entity.HasMany(p => p.BankTransactions)
                  .WithOne(bt => bt.Project)
                  .HasForeignKey(bt => bt.ProjectId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // ProjectEmployee many-to-many configuration
        modelBuilder.Entity<ProjectEmployee>(entity =>
        {

            entity.HasKey(pe => pe.Id);

            entity.HasIndex(pe => new { pe.ProjectId, pe.EmployeeId })
                  .IsUnique();

            entity.Property(pe => pe.Role)
                  .HasMaxLength(100);

            entity.Property(pe => pe.HourlyRate)
                  .HasPrecision(18, 2);

            entity.Property(pe => pe.IsActive)
                  .HasDefaultValue(true);

            entity.HasOne(pe => pe.Project)
                  .WithMany(p => p.ProjectEmployees)
                  .HasForeignKey(pe => pe.ProjectId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(pe => pe.Employee)
                  .WithMany(e => e.ProjectAssignments)
                  .HasForeignKey(pe => pe.EmployeeId)
                  .OnDelete(DeleteBehavior.Cascade);
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
            entity.HasIndex(e => e.TransactionDate);

            // BUG: No concurrency token for preventing double processing
        });

        // Settlement configuration
        modelBuilder.Entity<Settlement>(entity =>
        {
            entity.ToTable("Settlements");

            entity.HasKey(s => s.Id);

            // 🔹 Unique → One settlement per Partner per Month per Year
            entity.HasIndex(s => new { s.PartnerId, s.Month, s.Year })
                  .IsUnique();

            entity.Property(s => s.Month)
                  .IsRequired();

            entity.Property(s => s.Year)
                  .IsRequired();

            entity.Property(s => s.ExpectedAmount)
                  .HasPrecision(18, 2)
                  .IsRequired();

            entity.Property(s => s.ActualAmount)
                  .HasPrecision(18, 2)
                  .IsRequired();

            entity.Property(s => s.SettlementAmount)
                  .HasPrecision(18, 2)
                  .IsRequired();

            entity.Property(s => s.TotalExpenses)
                  .HasPrecision(18, 2);

            entity.Property(s => s.GrossProfit)
                  .HasPrecision(18, 2);

            entity.Property(s => s.NetProfit)
                  .HasPrecision(18, 2);

            entity.Property(s => s.Status)
                  .IsRequired()
                  .HasConversion<int>();

            entity.Property(s => s.Notes)
                  .HasMaxLength(1000);

            entity.Property(s => s.SettledDate);

            // 🔹 Relationship → Partner
            entity.HasOne(s => s.Partner)
                  .WithMany(p => p.Settlements)
                  .HasForeignKey(s => s.PartnerId)
                  .OnDelete(DeleteBehavior.Restrict);

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

            entity.Property(me => me.Category)
                  .IsRequired()
                  .HasConversion<int>();

            entity.Property(me => me.IsRecurring)
                  .HasDefaultValue(false);

            entity.Property(me => me.ApprovedBy)
                  .HasMaxLength(150);

            // 🔹 Relationships

            // Partner → required
            entity.HasOne(me => me.Partner)
                  .WithMany(p => p.MonthlyExpenses)
                  .HasForeignKey(me => me.PartnerId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Employee → optional
            entity.HasOne(me => me.Employee)
                  .WithMany(e => e.MonthlyExpenses)
                  .HasForeignKey(me => me.EmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Asset → optional
            entity.HasOne(me => me.Asset)
                  .WithMany(a => a.MonthlyExpenses)
                  .HasForeignKey(me => me.AssetId)
                  .OnDelete(DeleteBehavior.Restrict);

            // 🔹 Index for reporting (very useful)
            entity.HasIndex(me => new { me.PartnerId, me.Month, me.Year });
        });

        // BankAccount configuration
        modelBuilder.Entity<BankAccount>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.AccountNumber).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Balance).HasPrecision(18, 2);

        });

        // Doc-Type configuration
        modelBuilder.Entity<DocType>(entity =>
        {
            entity.ToTable("DocTypes");

            entity.HasKey(dt => dt.Id);

            entity.Property(dt => dt.TypeName)
                  .IsRequired()
                  .HasMaxLength(150);

            // 🔹 Unique DocType name (PAN, Aadhar, Invoice, etc.)
            entity.HasIndex(dt => dt.TypeName)
                  .IsUnique();

            // 🔹 Relationship → One DocType → Many Documents
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

            // 🔹 Relationship → Many Documents → One DocType
            entity.HasOne(d => d.DocType)
                  .WithMany(dt => dt.Documents)
                  .HasForeignKey(d => d.DocType_Id)
                  .OnDelete(DeleteBehavior.Restrict);

            // 🔹 Index for faster filtering by DocType
            entity.HasIndex(d => d.DocType_Id);
        });

        // Revenue configuration
        modelBuilder.Entity<Revenue>(entity =>
        {

            entity.HasKey(r => r.Id);

            entity.Property(r => r.Amount)
                  .HasPrecision(18, 2)
                  .IsRequired();

            entity.Property(r => r.Date)
                  .IsRequired();

            entity.Property(r => r.Revenue_From)
                  .HasDefaultValue(true);

            entity.Property(r => r.Notes)
                  .HasMaxLength(1000);

            // 🔹 Relationship → Partner (required)
            entity.HasOne(r => r.Partner)
                  .WithMany(p => p.Revenues)
                  .HasForeignKey(r => r.Partner_id)
                  .OnDelete(DeleteBehavior.Restrict);

            // 🔹 Relationship → Project (optional)
            entity.HasOne(r => r.Project)
                  .WithMany(p => p.Revenues)
                  .HasForeignKey(r => r.Project_id)
                  .OnDelete(DeleteBehavior.Restrict);

            // 🔹 Index for reporting
            entity.HasIndex(r => new { r.Partner_id, r.Date });
        });

        // EmployeeDocument configuration
        modelBuilder.Entity<EmployeeDocument>(entity =>
        {
            entity.ToTable("EmployeeDocuments");

            entity.HasKey(ed => new { ed.EmployeeId, ed.DocId });

            // 🔹 Prevent duplicate document assignment to same employee
            entity.HasIndex(ed => new { ed.EmployeeId, ed.DocId })
                  .IsUnique();

            // 🔹 Relationship → Employee
            entity.HasOne(ed => ed.Employee)
                  .WithMany(e => e.EmployeeDocuments)
                  .HasForeignKey(ed => ed.EmployeeId)
                  .OnDelete(DeleteBehavior.Cascade);

            // 🔹 Relationship → Documents
            entity.HasOne(ed => ed.Documents)
                  .WithMany(d => d.EmployeeDocuments)
                  .HasForeignKey(ed => ed.DocId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Branch configuration
        modelBuilder.Entity<Branch>(entity =>
        {
            entity.ToTable("Branches");

            entity.HasKey(b => b.Id);

            entity.Property(b => b.Name)
                  .IsRequired()
                  .HasMaxLength(150);

            entity.Property(b => b.Location)
                  .IsRequired()
                  .HasMaxLength(200);

            entity.Property(b => b.Description)
                  .HasMaxLength(500);

            entity.Property(b => b.IsActive)
                  .HasDefaultValue(true);

            entity.HasIndex(b => b.Name)
                  .IsUnique();

            entity.HasMany(b => b.Partners)
                  .WithOne(p => p.Branch)
                  .HasForeignKey(p => p.BranchId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(b => b.Employees)
                  .WithOne(e => e.Branch)
                  .HasForeignKey(e => e.BranchId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        //Profile configuration
        modelBuilder.Entity<Profile>(entity =>
        {
            entity.ToTable("Profiles");

            entity.HasKey(p => p.Id);

            entity.Property(p => p.IsPaid)
                  .HasDefaultValue(false);

            entity.Property(p => p.Amount)
                  .HasPrecision(18, 2);

            // 🔹 One User → One Profile
            entity.HasOne(p => p.User)
                  .WithOne(u => u.Profile)
                  .HasForeignKey<Profile>(p => p.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            // 🔹 One Profile → Many Projects
            entity.HasMany(p => p.Projects)
                  .WithOne(pr => pr.Profile)
                  .HasForeignKey(pr => pr.ProfileId)
                  .OnDelete(DeleteBehavior.Restrict);

            // 🔹 Prevent multiple profiles for same user
            entity.HasIndex(p => p.UserId)
                  .IsUnique();
        });
    }
}

// PERFORMANCE ISSUE: No query splitting configuration for related data
// BUG: Missing soft delete global query filter
// BUG: No audit trail configuration