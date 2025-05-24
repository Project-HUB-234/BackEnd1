using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_Hub.Migrations
{
    /// <inheritdoc />
    public partial class Brif : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Brif",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Brif",
                table: "Users");
        }
    }
}
