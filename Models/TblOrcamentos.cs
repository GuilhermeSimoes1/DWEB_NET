using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DWEB_NET.Models
{
    public class TblOrcamentos
    {
        [Key]
        public int OrcamentoID { get; set; }

        [StringLength(100)]
        public string NomeOrcamento {  get; set; }

        public double ValorNecessario { get; set; }

        public DateTime DataInicial { get; set; }

        public DateTime DataFinal { get; set; }

        public double ValorAtual { get; set; }

        [ForeignKey(nameof(UserID))]
        [Display(Name = "Utilizador associado")]
        public int UserFK { get; set; }
        public TblUtilizadores UserID { get; set; }

    }
}