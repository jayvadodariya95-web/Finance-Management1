using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatedProjectModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Revenue_Projects_ProjectId",
                table: "Revenue");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectValue",
                table: "Projects",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Projects",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "ClientName",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<string>(
                name: "ClientManagerContact",
                table: "Projects",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientManagerEmail",
                table: "Projects",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientManagerName",
                table: "Projects",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InterviewingUserId",
                table: "Projects",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSmooth",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsToolUsed",
                table: "Projects",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LeaveApplyWay",
                table: "Projects",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ManagerContact",
                table: "Projects",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ManagerEmail",
                table: "Projects",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ManagerName",
                table: "Projects",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MobileNumberUsed",
                table: "Projects",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TechnologyStack",
                table: "Projects",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_InterviewingUserId",
                table: "Projects",
                column: "InterviewingUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Name",
                table: "Projects",
                column: "Name");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Users_InterviewingUserId",
                table: "Projects",
                column: "InterviewingUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Revenue_Projects_ProjectId",
                table: "Revenue",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Users_InterviewingUserId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Revenue_Projects_ProjectId",
                table: "Revenue");

            migrationBuilder.DropIndex(
                name: "IX_Projects_InterviewingUserId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_Name",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ClientManagerContact",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ClientManagerEmail",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ClientManagerName",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "InterviewingUserId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "IsSmooth",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "IsToolUsed",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "LeaveApplyWay",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ManagerContact",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ManagerEmail",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ManagerName",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "MobileNumberUsed",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "TechnologyStack",
                table: "Projects");

            migrationBuilder.AlterColumn<decimal>(
                name: "ProjectValue",
                table: "Projects",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Projects",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "ClientName",
                table: "Projects",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_Revenue_Projects_ProjectId",
                table: "Revenue",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");
        }
    }
}
