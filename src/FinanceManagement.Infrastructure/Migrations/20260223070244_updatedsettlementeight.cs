using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatedsettlementeight : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Settlements_Partners_PartnerId",
                table: "Settlements");

            migrationBuilder.DropIndex(
                name: "IX_Settlements_PartnerId",
                table: "Settlements");

            migrationBuilder.AlterColumn<int>(
                name: "SettlementAmount",
                table: "Settlements",
                type: "int",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Settlements",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ExpectedAmount",
                table: "Settlements",
                type: "int",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<int>(
                name: "ActualAmount",
                table: "Settlements",
                type: "int",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AddColumn<int>(
                name: "GrossProfit",
                table: "Settlements",
                type: "int",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IsSetteled",
                table: "Settlements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NetProfit",
                table: "Settlements",
                type: "int",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "SettledDate",
                table: "Settlements",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalExpense",
                table: "Settlements",
                type: "int",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsBench",
                table: "ProjectEmployees",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Settlements_PartnerId_Month_Year",
                table: "Settlements",
                columns: new[] { "PartnerId", "Month", "Year" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Settlements_Partners_PartnerId",
                table: "Settlements",
                column: "PartnerId",
                principalTable: "Partners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Settlements_Partners_PartnerId",
                table: "Settlements");

            migrationBuilder.DropIndex(
                name: "IX_Settlements_PartnerId_Month_Year",
                table: "Settlements");

            migrationBuilder.DropColumn(
                name: "GrossProfit",
                table: "Settlements");

            migrationBuilder.DropColumn(
                name: "IsSetteled",
                table: "Settlements");

            migrationBuilder.DropColumn(
                name: "NetProfit",
                table: "Settlements");

            migrationBuilder.DropColumn(
                name: "SettledDate",
                table: "Settlements");

            migrationBuilder.DropColumn(
                name: "TotalExpense",
                table: "Settlements");

            migrationBuilder.DropColumn(
                name: "IsBench",
                table: "ProjectEmployees");

            migrationBuilder.AlterColumn<decimal>(
                name: "SettlementAmount",
                table: "Settlements",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Settlements",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ExpectedAmount",
                table: "Settlements",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualAmount",
                table: "Settlements",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.CreateIndex(
                name: "IX_Settlements_PartnerId",
                table: "Settlements",
                column: "PartnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Settlements_Partners_PartnerId",
                table: "Settlements",
                column: "PartnerId",
                principalTable: "Partners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
