using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWEB_NET.Models
{
	public class TblContas
	{
		public TblContas() { 

			ListaTransacoes = new HashSet<TblTransacoes>(); 
		
		}

		[Key]
		public int ContaID { get; set; }

		
		public string NomeConta { get; set; } = "Conta";


		public decimal Saldo { get; set; }
        

        [ForeignKey(nameof(UserID))]
		[Display(Name = "Utilizador associado")]
		public int UserFK { get; set; }
		public TblUtilizadores UserID { get; set; }

        public ICollection<TblTransacoes> ListaTransacoes { get; set; }
    }
}