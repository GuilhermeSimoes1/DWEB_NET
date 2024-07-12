using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DWEB_NET.Models
{
    public class TblOrcamentos
    {
        [Key]
        public int OrcamentoID { get; set; }

        [StringLength(100)]
        [Display(Name = "Nome do Orçamento:")]
        public required string NomeOrcamento {  get; set; }

        [Display(Name = "Valor Necessário:")]
        public required decimal ValorNecessario { get; set; }

        [Display(Name = "Data Inicial:")]
        public DateTime DataInicial { get; set; }

        [Display(Name = "Data Final:")]
        public DateTime? DataFinal { get; set; }

        [Display(Name = "Valor Atual:")]
        public decimal ValorAtual { get; set; } = 0;

        [ForeignKey(nameof(User))]
        [Display(Name = "Utilizador Associado:")]
        public int UserFK { get; set; }
        public TblUtilizadores User { get; set; }

    }
}