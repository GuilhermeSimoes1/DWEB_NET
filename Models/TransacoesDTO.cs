using System.ComponentModel.DataAnnotations;

namespace DWEB_NET.Models
{
    public class TransacoesDTO
    {
        public enum Tipo
        {
            Ganho,
            Gasto
        }

        public DateTime DataTime { get; set; }
        public Tipo TipoTransacao { get; set; }
        public string? Descricao { get; set; }
        public required decimal ValorTransacao { get; set; }
        public int ContaFK { get; set; }
        public int UserFK { get; set; }

        public List<TransacaoCategoriaDTO> Categorias { get; set; }
    }

    public class TransacaoCategoriaDTO
    {
        public int CategoriaFK { get; set; }
        public decimal Valor { get; set; }
    }
}
