using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWEB_NET.Models
{
    public class TblCategorias
    {

        public TblCategorias()


        {
            ListaTransacoesCategorias = new HashSet<TblTransacoesCategorias>();
        }

        [Key]
        public int CategoriaID { get; set; }


        public string NomeCategoria { get; set; }

        // Cole��o de transa��es associadas � categoria
        public ICollection<TblTransacoesCategorias> ListaTransacoesCategorias { get; set; }

    }
}