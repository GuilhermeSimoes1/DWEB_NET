using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWEB.Models
{
    public class TblTransacoes
    {
        [Key]
        public int TransacaoID { get; set; }

        public DateTime DataTime { get; set; }

       
        public enum Tipo
        {
            Ganho,
            Gasto
        }

        [StringLength(100)]
        public string Descricao { get; set; }

        public double ValorTransacao { get; set; }

        [ForeignKey(nameof(ContaID))]
        [Display(Name = "Conta associada")]
        public int ContaFK { get; set; }
        public TblContas ContaID { get; set; }

        [ForeignKey(nameof(CategoriaID))]
        [Display(Name = "Categoria associada")]
        public int CategoriaFK { get; set; }
        public TblCategorias CategoriaID { get; set; }

        [ForeignKey(nameof(UserID))]
        [Display(Name = "Utilizador associado")]
        public int UserFK { get; set; }
        public TblUtilizadores UserID { get; set; }

        [ForeignKey(nameof(Saldo))]
        [Display(Name = "Saldo associado")]
        public double SaldoFK { get; set; }
        public TblContas Saldo { get; set; }
    }
}
