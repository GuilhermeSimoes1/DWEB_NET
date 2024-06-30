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
        public DbSet<TblMoedas> Moedas { get; set; }
        public DbSet<TblOrcamentos> Orcamentos { get; set; }
        public DbSet<TblTransacoesCategorias> TransacoesCategorias { get; set; }



    }
}
