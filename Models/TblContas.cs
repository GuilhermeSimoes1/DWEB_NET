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

        [Display(Name = "Conta")]
        public string NomeConta { get; set; } = "Conta";

        [Display(Name = "Saldo")]
        public decimal Saldo { get; set; }
        
        [ForeignKey(nameof(User))]
		[Display(Name = "Utilizador associado")]
		public int UserFK { get; set; }

        [Display(Name = "Utilizador")]
        public TblUtilizadores User { get; set; }

        public ICollection<TblTransacoes> ListaTransacoes { get; set; }
    }
}