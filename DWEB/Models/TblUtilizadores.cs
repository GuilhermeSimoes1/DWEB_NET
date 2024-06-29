using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DWEB.Models
{
    public class TblUtilizadores
    {
        [Key]
        public int UserID { get; set; }


        [StringLength(100)]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório.")]
        public string UserName { get; set; }


        [StringLength(50)]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório.")]
        public string Email { get; set; }


        [StringLength(20)]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório.")]
        public string Passwd { get; set; }



        [StringLength(30)]
        public string FirstName { get; set; }

        [StringLength(30)]
        public string LastName { get; set; }



        [StringLength(255)]
        public string Descricao { get; set; }


       
    }
}