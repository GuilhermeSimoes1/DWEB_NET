using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Orcamento.Models
{
    public class TblCategorias
    {
        [Key]
        public int CategoriaID { get; set; }

        public string Nome { get; set; }

    }
}