using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Orcamento.Models
{
	public class TblContas
	{
		[Key]
		public int ContaID { get; set; }

		public string Tipo { get; set; }

		public string Saldo { get; set; }

		public string Nome { get; set; }

		//FALTA FAZER A PARTE DAS FKs
		public int OrcamentoFK { get; set; }
		
		public int Transacao { get; set; }

		public double Saldo { get; set; }
	}
}