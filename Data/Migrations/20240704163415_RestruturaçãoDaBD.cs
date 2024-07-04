using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DWEB_NET.Data.Migrations
{
    /// <inheritdoc />
    public partial class RestruturaçãoDaBD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    CategoriaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeCategoria = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.CategoriaID);
                });

            migrationBuilder.CreateTable(
                name: "Utilizadores",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Descricao = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UserAutent = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilizadores", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "Contas",
                columns: table => new
                {
                    ContaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeConta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Saldo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UserFK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contas", x => x.ContaID);
                    table.ForeignKey(
                        name: "FK_Contas_Utilizadores_UserFK",
                        column: x => x.UserFK,
                        principalTable: "Utilizadores",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orcamentos",
                columns: table => new
                {
                    OrcamentoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeOrcamento = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ValorNecessario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DataInicial = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataFinal = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ValorAtual = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UserFK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orcamentos", x => x.OrcamentoID);
                    table.ForeignKey(
                        name: "FK_Orcamentos_Utilizadores_UserFK",
                        column: x => x.UserFK,
                        principalTable: "Utilizadores",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transacoes",
                columns: table => new
                {
                    TransacaoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ValorTransacao = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ContaFK = table.Column<int>(type: "int", nullable: false),
                    CategoriaFK = table.Column<int>(type: "int", nullable: false),
                    UserFK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transacoes", x => x.TransacaoID);
                    table.ForeignKey(
                        name: "FK_Transacoes_Categorias_CategoriaFK",
                        column: x => x.CategoriaFK,
                        principalTable: "Categorias",
                        principalColumn: "CategoriaID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transacoes_Contas_ContaFK",
                        column: x => x.ContaFK,
                        principalTable: "Contas",
                        principalColumn: "ContaID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transacoes_Utilizadores_UserFK",
                        column: x => x.UserFK,
                        principalTable: "Utilizadores",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "TransacoesCategorias",
                columns: table => new
                {
                    TransacaoFK = table.Column<int>(type: "int", nullable: false),
                    CategoriaFK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransacoesCategorias", x => new { x.TransacaoFK, x.CategoriaFK });
                    table.ForeignKey(
                        name: "FK_TransacoesCategorias_Categorias_CategoriaFK",
                        column: x => x.CategoriaFK,
                        principalTable: "Categorias",
                        principalColumn: "CategoriaID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransacoesCategorias_Transacoes_TransacaoFK",
                        column: x => x.TransacaoFK,
                        principalTable: "Transacoes",
                        principalColumn: "TransacaoID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "CategoriaID", "NomeCategoria" },
                values: new object[,]
                {
                    { 1, "Saúde" },
                    { 2, "Lazer" },
                    { 3, "Casa" },
                    { 4, "Educação" },
                    { 5, "Alimentação" },
                    { 6, "Outros" },
                    { 7, "Salário" },
                    { 8, "Investimentos" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contas_UserFK",
                table: "Contas",
                column: "UserFK");

            migrationBuilder.CreateIndex(
                name: "IX_Orcamentos_UserFK",
                table: "Orcamentos",
                column: "UserFK");

            migrationBuilder.CreateIndex(
                name: "IX_Transacoes_CategoriaFK",
                table: "Transacoes",
                column: "CategoriaFK");

            migrationBuilder.CreateIndex(
                name: "IX_Transacoes_ContaFK",
                table: "Transacoes",
                column: "ContaFK");

            migrationBuilder.CreateIndex(
                name: "IX_Transacoes_UserFK",
                table: "Transacoes",
                column: "UserFK");

            migrationBuilder.CreateIndex(
                name: "IX_TransacoesCategorias_CategoriaFK",
                table: "TransacoesCategorias",
                column: "CategoriaFK");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orcamentos");

            migrationBuilder.DropTable(
                name: "TransacoesCategorias");

            migrationBuilder.DropTable(
                name: "Transacoes");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Contas");

            migrationBuilder.DropTable(
                name: "Utilizadores");
        }
    }
}
