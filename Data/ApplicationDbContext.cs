using DWEB_NET.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DWEB_NET.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<TblUtilizadores> Utilizadores { get; set; }
        public DbSet<TblContas> Contas { get; set; }
        public DbSet<TblTransacoes> Transacoes { get; set; }
        public DbSet<TblCategorias> Categorias { get; set; }
        public DbSet<TblOrcamentos> Orcamentos { get; set; }
        public DbSet<TblTransacoesCategorias> TransacoesCategorias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TblCategorias>().HasData(
                new TblCategorias { CategoriaID = 1, NomeCategoria = "Saúde" },
                new TblCategorias { CategoriaID = 2, NomeCategoria = "Lazer" },
                new TblCategorias { CategoriaID = 3, NomeCategoria = "Casa" },
                new TblCategorias { CategoriaID = 4, NomeCategoria = "Educação" },
                new TblCategorias { CategoriaID = 5, NomeCategoria = "Alimentação" },
                new TblCategorias { CategoriaID = 6, NomeCategoria = "Outros" },
                new TblCategorias { CategoriaID = 7, NomeCategoria = "Salário" },
                new TblCategorias { CategoriaID = 8, NomeCategoria = "Investimentos" }
            );
        }


    }
}
