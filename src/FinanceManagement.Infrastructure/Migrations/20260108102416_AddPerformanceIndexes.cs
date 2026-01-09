using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPerformanceIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Settlements_PartnerId",
                table: "Settlements");

            migrationBuilder.DropIndex(
                name: "IX_ProjectEmployees_ProjectId",
                table: "ProjectEmployees");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeCode",
                table: "Employees",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Settlements_PartnerId_Month_Year",
                table: "Settlements",
                columns: new[] { "PartnerId", "Month", "Year" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectEmployees_ProjectId_EmployeeId",
                table: "ProjectEmployees",
                columns: new[] { "ProjectId", "EmployeeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyExpenses_Month_Year",
                table: "MonthlyExpenses",
                columns: new[] { "Month", "Year" });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EmployeeCode",
                table: "Employees",
                column: "EmployeeCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BankTransactions_TransactionDate",
                table: "BankTransactions",
                column: "TransactionDate");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_AccountNumber",
                table: "BankAccounts",
                column: "AccountNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Settlements_PartnerId_Month_Year",
                table: "Settlements");

            migrationBuilder.DropIndex(
                name: "IX_ProjectEmployees_ProjectId_EmployeeId",
                table: "ProjectEmployees");

            migrationBuilder.DropIndex(
                name: "IX_MonthlyExpenses_Month_Year",
                table: "MonthlyExpenses");

            migrationBuilder.DropIndex(
                name: "IX_Employees_EmployeeCode",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_BankTransactions_TransactionDate",
                table: "BankTransactions");

            migrationBuilder.DropIndex(
                name: "IX_BankAccounts_AccountNumber",
                table: "BankAccounts");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeCode",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Settlements_PartnerId",
                table: "Settlements",
                column: "PartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectEmployees_ProjectId",
                table: "ProjectEmployees",
                column: "ProjectId");
        }
    }
}
