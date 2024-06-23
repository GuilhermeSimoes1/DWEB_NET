using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Orcamento.Models
{
    public class TblUtilizadores
    {
        [Key]
        public int UserID { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório.")]
        public string Nome { get; set; }

        [StringLength(20)]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório.")]
        public string Passwd { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório.")]
        public string Email { get; set; }


        [ForeignKey(nameof(ContaID))]
        [Display(Name = "Conta Associada")]
        public int ContaFK { get; set; }
        public TblContas ContaID { get; set; }
       
    }
}