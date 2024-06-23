using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Orcamento.Models
{
    public class TblMoedas
    {
        [Key]
        public int MoedaID { get; set; }

        public string Nome { get; set; }

        //FALTA A PARTE DA FK
        public double Valor { get; set; }

    }
}