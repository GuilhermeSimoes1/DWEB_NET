namespace DWEB_NET.Models.DTO
{
    public class OrcamentoModel
    {
        public int OrcamentoID { get; set; }
        public string NomeOrcamento { get; set; }
        public decimal ValorNecessario { get; set; }
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
        public decimal ValorAtual { get; set; }
        public int UserFK { get; set; }
    }
}
