using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Orcamento.Models
{
    public class TblOrcamentos
    {
        [Key]
        public int OrcamentoID { get; set; }

        public DateTime DataInicial { get; set; }

        public string DataFinal { get; set; }


        //Falta a parte das FKs
        public string Valor { get; set; }

        public int Moeda { get; set; }
    }
}