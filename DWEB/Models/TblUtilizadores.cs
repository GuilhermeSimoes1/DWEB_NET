using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Orcamento.Models
{
    public class TblUtilizadores
    {
        [Key]
        public int UserID { get; set; }

        public string Nome { get; set; }

        public string Passwd { get; set; }

        public string Email { get; set; }

        [ForeignKey(nameof(Conta))]
        [Display(Name ="Curso")]
        public int ContaFK { get; set; }
        public TblContas Conta { get; set; }
       
    }
}