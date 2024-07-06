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

        public enum Tipo
        {
            Ganho,
            Gasto
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //Incrementa automaticamente
        public int TransacaoID { get; set; }

        public DateTime DataTime { get; set; }

        public Tipo TipoTransacao { get; set; }         

        [StringLength(100)]
        public string? Descricao { get; set; }

        public required decimal ValorTransacao { get; set; }

        [ForeignKey(nameof(Conta))]
        [Display(Name = "Conta associada")]
        public int ContaFK { get; set; }
        public TblContas Conta { get; set; }

        [ForeignKey(nameof(Categoria))]
        [Display(Name = "Categoria associada")]
        public int CategoriaFK { get; set; }
        public TblCategorias Categoria{ get; set; }

        [ForeignKey(nameof(User))]
        [Display(Name = "Utilizador associado")]
        public int UserFK { get; set; }
        public TblUtilizadores User { get; set; }

        public ICollection<TblTransacoesCategorias> ListaTransacoesCategorias { get; set; }
    }


}
