using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Orcamento.Models
{
	public class TblContas
	{
		[Key]
		public int ContaID { get; set; }

		
		public string Nome { get; set; } = "Conta";


		public double SaldoConta { get; set; }


		[ForeignKey(nameof(OrcamentoID))]
		[Display(Name = "Orcamento associado")]
		public int OrcamentoFK { get; set; }
		public TblOrcamentos OrcamentoID { get; set; }


		[ForeignKey(nameof(TransacaoID))]
        [Display(Name = "Transacao associada")]
        public int TransacaoFK { get; set; }
		public TblTransacoes TransacaoID { get; set; }

	}
}