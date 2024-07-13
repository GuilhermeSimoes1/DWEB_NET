using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DWEB_NET.Migrations
{
    /// <inheritdoc />
    public partial class nparamajustes2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TransacoesCategorias",
                table: "TransacoesCategorias");

            migrationBuilder.DropIndex(
                name: "IX_TransacoesCategorias_TransacaoFK",
                table: "TransacoesCategorias");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "TransacoesCategorias");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransacoesCategorias",
                table: "TransacoesCategorias",
                columns: new[] { "TransacaoFK", "CategoriaFK" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TransacoesCategorias",
                table: "TransacoesCategorias");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "TransacoesCategorias",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransacoesCategorias",
                table: "TransacoesCategorias",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TransacoesCategorias_TransacaoFK",
                table: "TransacoesCategorias",
                column: "TransacaoFK");
        }
    }
}
