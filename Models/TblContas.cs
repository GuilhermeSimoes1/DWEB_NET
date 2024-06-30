using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DWEB_NET.Models.TblTransacoes;


namespace DWEB_NET.Models
{
	public class TblContas
	{
		public TblContas() { TransacoesList = new HashSet<TblTransacoes>(); }

		[Key]
		public int ContaID { get; set; }

		
		public string NomeConta { get; set; } = "Conta";


		public double Saldo { get; set; }
        public ICollection<TblTransacoes> TransacoesList { get; set; }

        [ForeignKey(nameof(UserID))]
		[Display(Name = "Orcamento associado")]
		public int UserFK { get; set; }
		public TblUtilizadores UserID { get; set; }


	}
}