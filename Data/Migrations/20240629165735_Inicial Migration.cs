using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DWEB_NET.Data.Migrations
{
    /// <inheritdoc />
    public partial class InicialMigration : Migration
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
                name: "Moedas",
                columns: table => new
                {
                    MoedaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeMoeda = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Moedas", x => x.MoedaID);
                });

            migrationBuilder.CreateTable(
                name: "Utilizadores",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Passwd = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
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
                    Saldo = table.Column<double>(type: "float", nullable: false),
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
                    ValorNecessario = table.Column<double>(type: "float", nullable: false),
                    DataInicial = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataFinal = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValorAtual = table.Column<double>(type: "float", nullable: false),
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
                    Descricao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ValorTransacao = table.Column<double>(type: "float", nullable: false),
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
                name: "Moedas");

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
