using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Orcamento.Models
{
    public class TblTransacoes
    {
        [Key]
        public int TransacaoID { get; set; }

        public DateTime Data { get; set; }

        public string Tipo { get; set; }

        public string Descricao { get; set; }


        //FALTA A PARTE DAS FKs
        public double Valor { get; set; }

        public int Categoria { get; set; }

    }
}