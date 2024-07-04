using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWEB_NET.Models
{
    public class TblTransacoes
    {

        public TblTransacoes()
        {
            ListaTransacoesCategorias = new HashSet<TblTransacoesCategorias>();
        }


        [Key]
        public int TransacaoID { get; set; }

        public DateTime DataTime { get; set; }

       
        public enum Tipo
        {
            Ganho,
            Gasto
        }

        [StringLength(100)]
        public string? Descricao { get; set; }

        public required double ValorTransacao { get; set; }


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


        // Coleção de categorias associadas à transação
        public ICollection<TblTransacoesCategorias> ListaTransacoesCategorias { get; set; }


    }
}
