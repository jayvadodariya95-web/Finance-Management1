using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Remove old Category column
            migrationBuilder.DropColumn(
                name: "Category",
                table: "MonthlyExpenses");

            // Add new CategoryId column (nullable if you want)
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "MonthlyExpenses",
                type: "int",
                nullable: false);

            // Create Categories table if not exists
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsRecurring = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            // Create index on CategoryId
            migrationBuilder.CreateIndex(
                name: "IX_MonthlyExpenses_CategoryId",
                table: "MonthlyExpenses",
                column: "CategoryId");

            // Add foreign key constraint
            migrationBuilder.AddForeignKey(
                name: "FK_MonthlyExpenses_Categories_CategoryId",
                table: "MonthlyExpenses",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MonthlyExpenses_Categories_CategoryId",
                table: "MonthlyExpenses");

            migrationBuilder.DropIndex(
                name: "IX_MonthlyExpenses_CategoryId",
                table: "MonthlyExpenses");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "MonthlyExpenses");

            // Re-add old Category column in Down for rollback
            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "MonthlyExpenses",
                type: "int",
                nullable: false,
                defaultValue: 0); // or whatever default you want
        }
    }
}
