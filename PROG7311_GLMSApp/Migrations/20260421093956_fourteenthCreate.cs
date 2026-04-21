using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PROG7311_GLMSApp.Migrations
{
    /// <inheritdoc />
    public partial class fourteenthCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "Contract",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "Contract");
        }
    }
}
