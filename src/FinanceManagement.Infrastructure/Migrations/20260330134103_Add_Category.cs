using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Category : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ❌ REMOVE RenameColumn (it was wrong)

            // ✅ ADD CategoryId column
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "MonthlyExpenses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // ✅ Alter column (keep as is)
            migrationBuilder.AlterColumn<DateTime>(
                name: "RelievingDate",
                table: "Employees",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            // ✅ Create Categories table
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

            // ✅ Create Index
            migrationBuilder.CreateIndex(
                name: "IX_MonthlyExpenses_CategoryId",
                table: "MonthlyExpenses",
                column: "CategoryId");

            // ✅ Add Foreign Key
            migrationBuilder.AddForeignKey(
                name: "FK_MonthlyExpenses_Categories_CategoryId",
                table: "MonthlyExpenses",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MonthlyExpenses_Categories_CategoryId",
                table: "MonthlyExpenses");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_MonthlyExpenses_CategoryId",
                table: "MonthlyExpenses");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "MonthlyExpenses");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RelievingDate",
                table: "Employees",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
