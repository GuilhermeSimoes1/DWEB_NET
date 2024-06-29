using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWEB.Models
{
    public class TransacoesCategorias
    {
        [Key, Column(Order = 1)]
        [ForeignKey(nameof(TransacaoID))]
        [Display(Name = "Transacao associada")]
        public int TransacaoFK {  get; set; }

        public TblTransacoes TransacaoID { get; set; }

        [Key, Column(Order = 2)]
        [ForeignKey(nameof(CategoriaID))]
        [Display(Name = "Categoria associada")]
        public int CategoriaFK { get; set; }
        public TblCategorias CategoriaID { get; set; }


    }
}
