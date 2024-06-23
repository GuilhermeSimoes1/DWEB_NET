using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Orcamento.Models
{
    public class TblOrcamentos
    {
        [Key]
        public int OrcamentoID { get; set; }

        public DateTime DataInicial { get; set; }

        public DateTime DataFinal { get; set; }

        public string ValorOrcamento { get; set; }

        [ForeignKey(nameof(MoedaID))]
        [Display(Name = "Moeda associada")]
        public int Moeda { get; set; }
        public TblMoedas MoedaID { get; set; }

    }
}