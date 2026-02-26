using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addMissingRevenueAndAssetFluentAPIs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MonthlyExpenses_Asset_AssetId",
                table: "MonthlyExpenses");

            migrationBuilder.DropForeignKey(
                name: "FK_Revenue_Partners_PartnerId",
                table: "Revenue");

            migrationBuilder.DropForeignKey(
                name: "FK_Revenue_Projects_ProjectId",
                table: "Revenue");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Revenue",
                table: "Revenue");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Asset",
                table: "Asset");

            migrationBuilder.RenameTable(
                name: "Revenue",
                newName: "Revenues");

            migrationBuilder.RenameTable(
                name: "Asset",
                newName: "Assets");

            migrationBuilder.RenameIndex(
                name: "IX_Revenue_ProjectId",
                table: "Revenues",
                newName: "IX_Revenues_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Revenue_PartnerId",
                table: "Revenues",
                newName: "IX_Revenues_PartnerId");

            migrationBuilder.AlterColumn<bool>(
                name: "Revenue_From",
                table: "Revenues",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Revenues",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Revenues",
                table: "Revenues",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Assets",
                table: "Assets",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MonthlyExpenses_Assets_AssetId",
                table: "MonthlyExpenses",
                column: "AssetId",
                principalTable: "Assets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Revenues_Partners_PartnerId",
                table: "Revenues",
                column: "PartnerId",
                principalTable: "Partners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Revenues_Projects_ProjectId",
                table: "Revenues",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MonthlyExpenses_Assets_AssetId",
                table: "MonthlyExpenses");

            migrationBuilder.DropForeignKey(
                name: "FK_Revenues_Partners_PartnerId",
                table: "Revenues");

            migrationBuilder.DropForeignKey(
                name: "FK_Revenues_Projects_ProjectId",
                table: "Revenues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Revenues",
                table: "Revenues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Assets",
                table: "Assets");

            migrationBuilder.RenameTable(
                name: "Revenues",
                newName: "Revenue");

            migrationBuilder.RenameTable(
                name: "Assets",
                newName: "Asset");

            migrationBuilder.RenameIndex(
                name: "IX_Revenues_ProjectId",
                table: "Revenue",
                newName: "IX_Revenue_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Revenues_PartnerId",
                table: "Revenue",
                newName: "IX_Revenue_PartnerId");

            migrationBuilder.AlterColumn<bool>(
                name: "Revenue_From",
                table: "Revenue",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Revenue",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Revenue",
                table: "Revenue",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Asset",
                table: "Asset",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MonthlyExpenses_Asset_AssetId",
                table: "MonthlyExpenses",
                column: "AssetId",
                principalTable: "Asset",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Revenue_Partners_PartnerId",
                table: "Revenue",
                column: "PartnerId",
                principalTable: "Partners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Revenue_Projects_ProjectId",
                table: "Revenue",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
