using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace DWEB_NET.Models
{

    [PrimaryKey(nameof(TransacaoFK), nameof(CategoriaFK))]
    public class TblTransacoesCategorias
    {
        
        
            [ForeignKey(nameof(TransacaoID))]
            [Display(Name = "Transacao associada")]
            public int TransacaoFK { get; set; }

            public TblTransacoes TransacaoID { get; set; }

            

            [ForeignKey(nameof(CategoriaID))]
            [Display(Name = "Categoria associada")]
            public int CategoriaFK { get; set; }
            public TblCategorias CategoriaID { get; set; }

            
       
    }
}
