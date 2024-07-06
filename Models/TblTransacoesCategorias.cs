using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace DWEB_NET.Models
{

    [PrimaryKey(nameof(TransacaoFK), nameof(CategoriaFK))]
    public class TblTransacoesCategorias
    {
        
        
            [ForeignKey(nameof(Transacao))]
            [Display(Name = "Transacao associada")]
            public int TransacaoFK { get; set; }

            public TblTransacoes Transacao { get; set; }

            

            [ForeignKey(nameof(Categoria))]
            [Display(Name = "Categoria associada")]
            public int CategoriaFK { get; set; }
            public TblCategorias Categoria { get; set; }

            public decimal Valor { get; set; }  

    }
}
