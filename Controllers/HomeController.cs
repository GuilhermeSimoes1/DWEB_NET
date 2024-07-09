using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using DWEB_NET.Models; 

namespace DWEB_NET.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly Dictionary<string, Dictionary<string, double>> exchangeRates = new Dictionary<string, Dictionary<string, double>>
        {
            { "euro", new Dictionary<string, double> { { "dolar", 1.10 }, { "iene", 130 }, { "libra", 0.85 } } },
            { "dolar", new Dictionary<string, double> { { "euro", 0.91 }, { "iene", 118.18 }, { "libra", 0.77 } } },
            { "iene", new Dictionary<string, double> { { "euro", 0.0077 }, { "dolar", 0.0085 }, { "libra", 0.0065 } } },
            { "libra", new Dictionary<string, double> { { "euro", 1.18 }, { "dolar", 1.29 }, { "iene", 152.94 } } }
        };



        [HttpGet]
        public IActionResult Moedas()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Moedas(string moeda, string targetMoeda, double valor)
        {
            if (!exchangeRates.ContainsKey(moeda) || !exchangeRates[moeda].ContainsKey(targetMoeda))
            {
                ViewBag.Resultado = "Conversão inválida.";
            }
            else
            {
                double taxa = exchangeRates[moeda][targetMoeda];
                double resultadoConversao = valor * taxa;
                ViewBag.Resultado = $"{valor:F2} {moeda.ToUpper()} é igual a {resultadoConversao:F2} {targetMoeda.ToUpper()}";
            }

            return View();
        }

    public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Informacoes()
        {
            return View();
        }

        public IActionResult Index()
        {
            if(User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Create", "TblTransacoes");
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
