using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PROG7311_GLMSApp.Migrations
{
    /// <inheritdoc />
    public partial class eleventhCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Cost",
                table: "ServiceRequest",
                type: "float",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<double>(
                name: "ZarAmount",
                table: "ServiceRequest",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ZarAmount",
                table: "ServiceRequest");

            migrationBuilder.AlterColumn<string>(
                name: "Cost",
                table: "ServiceRequest",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
