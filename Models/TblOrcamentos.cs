using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DWEB_NET.Models
{
    public class TblOrcamentos
    {
        [Key]
        public int OrcamentoID { get; set; }

        [StringLength(100)]
        public required string NomeOrcamento {  get; set; }

        public required decimal ValorNecessario { get; set; }

        public DateTime DataInicial { get; set; }

        public DateTime? DataFinal { get; set; }

        public decimal ValorAtual { get; set; } = 0;

        [ForeignKey(nameof(User))]
        [Display(Name = "Utilizador associado")]
        public int UserFK { get; set; }
        public TblUtilizadores User { get; set; }

    }
}