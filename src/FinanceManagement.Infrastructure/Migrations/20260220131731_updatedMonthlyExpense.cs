using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatedMonthlyExpense : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsRecurring",
                table: "MonthlyExpenses",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "ApprovedBy",
                table: "MonthlyExpenses",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AssetId",
                table: "MonthlyExpenses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "MonthlyExpenses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PartnerId",
                table: "MonthlyExpenses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyExpenses_AssetId",
                table: "MonthlyExpenses",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyExpenses_EmployeeId",
                table: "MonthlyExpenses",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyExpenses_PartnerId",
                table: "MonthlyExpenses",
                column: "PartnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_MonthlyExpenses_Asset_AssetId",
                table: "MonthlyExpenses",
                column: "AssetId",
                principalTable: "Asset",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MonthlyExpenses_Employees_EmployeeId",
                table: "MonthlyExpenses",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MonthlyExpenses_Partners_PartnerId",
                table: "MonthlyExpenses",
                column: "PartnerId",
                principalTable: "Partners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MonthlyExpenses_Asset_AssetId",
                table: "MonthlyExpenses");

            migrationBuilder.DropForeignKey(
                name: "FK_MonthlyExpenses_Employees_EmployeeId",
                table: "MonthlyExpenses");

            migrationBuilder.DropForeignKey(
                name: "FK_MonthlyExpenses_Partners_PartnerId",
                table: "MonthlyExpenses");

            migrationBuilder.DropIndex(
                name: "IX_MonthlyExpenses_AssetId",
                table: "MonthlyExpenses");

            migrationBuilder.DropIndex(
                name: "IX_MonthlyExpenses_EmployeeId",
                table: "MonthlyExpenses");

            migrationBuilder.DropIndex(
                name: "IX_MonthlyExpenses_PartnerId",
                table: "MonthlyExpenses");

            migrationBuilder.DropColumn(
                name: "AssetId",
                table: "MonthlyExpenses");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "MonthlyExpenses");

            migrationBuilder.DropColumn(
                name: "PartnerId",
                table: "MonthlyExpenses");

            migrationBuilder.AlterColumn<bool>(
                name: "IsRecurring",
                table: "MonthlyExpenses",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "ApprovedBy",
                table: "MonthlyExpenses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldNullable: true);
        }
    }
}
