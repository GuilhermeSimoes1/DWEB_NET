using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DWEB_NET.Migrations
{
    /// <inheritdoc />
    public partial class PasswordTblUtilizadores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Utilizadores",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Utilizadores");
        }
    }
}
