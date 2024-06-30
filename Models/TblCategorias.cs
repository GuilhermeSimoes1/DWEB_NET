using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWEB_NET.Models
{
    public class TblCategorias
    {
        [Key]
        public int CategoriaID { get; set; }

        public string NomeCategoria { get; set; }

    }
}