using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DWEB.Models
{
    public class TblMoedas
    {
        [Key]
        public int MoedaID { get; set; }

        [StringLength(3)]
        public string NomeMoeda { get; set; }

        public static readonly Dictionary<string, Dictionary<string, double>> ExchangeRates = new Dictionary<string, Dictionary<string, double>>
        {
            { "euro", new Dictionary<string, double> { { "dolar", 1.10 }, { "iene", 130 }, { "libra", 0.85 } } },
            { "dolar", new Dictionary<string, double> { { "euro", 0.91 }, { "iene", 118.18 }, { "libra", 0.77 } } },
            { "iene", new Dictionary<string, double> { { "euro", 0.0077 }, { "dolar", 0.0085 }, { "libra", 0.0065 } } },
            { "libra", new Dictionary<string, double> { { "euro", 1.18 }, { "dolar", 1.29 }, { "iene", 152.94 } } }
        };

        public double ConverterPara(string moedaDestino, double valor)
        {
            if (ExchangeRates.ContainsKey(NomeMoeda.ToLower()) && ExchangeRates[NomeMoeda.ToLower()].ContainsKey(moedaDestino.ToLower()))
            {
                return valor * ExchangeRates[NomeMoeda.ToLower()][moedaDestino.ToLower()];
            }
            throw new InvalidOperationException("Taxa de câmbio não disponível.");
        }
    }
}
