using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRevenue_AndAsset_Model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Revenues_Projects_ProjectId",
                table: "Revenues");

            migrationBuilder.DropIndex(
                name: "IX_Revenues_ProjectId",
                table: "Revenues");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Revenues");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "Revenues",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Revenues_ProjectId",
                table: "Revenues",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Revenues_Projects_ProjectId",
                table: "Revenues",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");
        }
    }
}
