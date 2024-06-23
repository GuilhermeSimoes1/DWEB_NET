using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Orcamento.Models
{
    public class TblTransacoes
    {
        [Key]
        public int TransacaoID { get; set; }

        public DateTime Data { get; set; }

        public string Tipo { get; set; }

        public string Descricao { get; set; }

        public double ValorTransacao { get; set; }


        
        [ForeignKey(nameof(MoedaID))]
        [Display(Name = "Moeda associada")]
        public int Moeda { get; set; }
        public TblMoedas MoedaID { get; set; }

        [ForeignKey(nameof(CategoriaID))]
        [Display(Name = "Categoria associada")]
        public int Categoria { get; set; }
        public TblCategorias CategoriaID { get; set; }

    }
}