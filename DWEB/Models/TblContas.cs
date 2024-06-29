using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DWEB.Models
{
	public class TblContas
	{
		[Key]
		public int ContaID { get; set; }

		
		public string NomeConta { get; set; } = "Conta";


		public double Saldo { get; set; }


		[ForeignKey(nameof(UserID))]
		[Display(Name = "Orcamento associado")]
		public int UserFK { get; set; }
		public TblUtilizadores UserID { get; set; }


	}
}