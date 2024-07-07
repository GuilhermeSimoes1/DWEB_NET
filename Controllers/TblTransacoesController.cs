using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DWEB_NET.Data;
using DWEB_NET.Models;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using SQLitePCL;

namespace DWEB_NET.Controllers
{
    public class TblTransacoesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TblTransacoesController> _logger;

        // Dictionary para mapear tipos de transação aos IDs das categorias correspondentes
        private readonly Dictionary<TblTransacoes.Tipo, List<int>> categoriasPorTipo = new Dictionary<TblTransacoes.Tipo, List<int>>
        {
            { TblTransacoes.Tipo.Ganho, new List<int> { 6, 7, 8 } },   // IDs das categorias para transações de Ganho
            { TblTransacoes.Tipo.Gasto, new List<int> { 1, 2, 3, 4, 5, 6 } }  // IDs das categorias para transações de Gasto
        };
        
        public TblTransacoesController(ApplicationDbContext context, ILogger<TblTransacoesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Método para obter o valor da conta selecionada
        public JsonResult GetAccountValue(int accountId)
        {
            var account = _context.Contas.FirstOrDefault(a => a.ContaID == accountId);
            if (account == null)
            {
                _logger.LogError($"Conta não encontrada para AccountID: {accountId}");
                return Json(new { success = false, message = "Conta não encontrada" });
            }
            _logger.LogInformation($"Valor da conta {account.NomeConta} encontrada: {account.Saldo}");
            return Json(new { success = true, valorConta = account.Saldo });
        }

        // Método para obter as categorias por tipo de transação
        public JsonResult GetCategoriasPorTipo(string tipoTransacao)
        {
            var tipo = Enum.Parse<TblTransacoes.Tipo>(tipoTransacao, true);
            if (categoriasPorTipo.TryGetValue(tipo, out var categoriaIds))
            {
                var categorias = _context.Categorias
                    .Where(c => categoriaIds.Contains(c.CategoriaID))
                    .Select(c => new { c.CategoriaID, c.NomeCategoria })
                    .ToList();

                _logger.LogInformation($"Categorias encontradas para tipo de transação {tipoTransacao}: {categorias.Count}");
                return Json(categorias);
            }

            _logger.LogWarning($"Nenhuma categoria encontrada para tipo de transação {tipoTransacao}");
            return Json(new List<object>());  // Retorna uma lista vazia se não encontrar categorias correspondentes
        }

        // Método para criar uma nova transação
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TblTransacoes transacao, [FromForm] Dictionary<int, decimal> CategoriaValores)
        {
            _logger.LogInformation("Iniciando o processo de criação de uma nova transação.");
            Console.WriteLine(transacao.User);
            try
            {
                _logger.LogInformation("Modelo de transação é válido.");

                // Obter o Id do utilizador logado
                var Id = User.FindFirstValue(ClaimTypes.NameIdentifier);



                // Encontrar o utilizador associado ao email
                var userAutent = await _context.Utilizadores.FirstOrDefaultAsync(u => u.UserAutent == Id);

                if (userAutent == null)
                {
                    _logger.LogError($"Utilizador não encontrado para o user: {Id}");
                    ModelState.AddModelError(string.Empty, "Utilizador não encontrado.");
                    ViewData["ContaFK"] = new SelectList(_context.Contas, "ContaID", "NomeConta", transacao.ContaFK);
                    ViewData["TipoTransacao"] = new SelectList(new List<string> { "Ganho", "Gasto" });
                    ViewData["CategoriaFK"] = new SelectList(_context.Categorias, "CategoriaID", "NomeCategoria");
                    return View(transacao);
                }

                _logger.LogInformation($"Utilizador encontrado: {userAutent.UserName}");

                // Atribuir UserID à transação
                transacao.UserFK = userAutent.UserID;
                ViewBag.UserFK = transacao.UserFK;

                // Verificar se a conta existe
                var account = await _context.Contas.FindAsync(transacao.ContaFK);
                if (account == null)
                {
                    _logger.LogError($"Conta não encontrada para ContaFK: {transacao.ContaFK}");
                    ModelState.AddModelError(string.Empty, "Conta não encontrada.");
                    ViewData["ContaFK"] = new SelectList(_context.Contas, "ContaID", "NomeConta", transacao.ContaFK);
                    ViewData["TipoTransacao"] = new SelectList(new List<string> { "Ganho", "Gasto" });
                    ViewData["CategoriaFK"] = new SelectList(_context.Categorias, "CategoriaID", "NomeCategoria");
                    return View(transacao);
                }

                _logger.LogInformation($"Conta encontrada: {account.NomeConta}");

                // Verifica se a soma dos valores das categorias é igual ao valor da transação
                decimal somaCategorias = CategoriaValores.Sum(cv => cv.Value);
                if (somaCategorias != transacao.ValorTransacao)
                {
                    _logger.LogError("A soma dos valores das categorias não é igual ao valor da transação.");
                    ModelState.AddModelError(string.Empty, "A soma dos valores das categorias deve ser igual ao valor da transação.");
                    ViewData["ContaFK"] = new SelectList(_context.Contas, "ContaID", "NomeConta", transacao.ContaFK);
                    ViewData["TipoTransacao"] = new SelectList(new List<string> { "Ganho", "Gasto" });
                    ViewData["CategoriaFK"] = new SelectList(_context.Categorias, "CategoriaID", "NomeCategoria");
                    return View(transacao);
                }
                var TransacaoID = _context.Transacoes.Count();
                TransacaoID = TransacaoID + 1;

                foreach (KeyValuePair<int, decimal> entry in CategoriaValores)
                {
                    decimal value = entry.Value; 

                    if (decimal.Compare(value, 0) != 0) { 
                    TblTransacoesCategorias data = new TblTransacoesCategorias();
                    data.TransacaoFK = TransacaoID;
                    data.CategoriaFK = entry.Key;
                    data.Valor = entry.Value;
                    _context.TransacoesCategorias.Add(data);
                    }
                }
            _logger.LogInformation("Soma dos valores das categorias é igual ao valor da transação.");
                
                // Atualiza o saldo da conta
                if (transacao.TipoTransacao == TblTransacoes.Tipo.Ganho)
                {
                    account.Saldo += transacao.ValorTransacao;
                }
                else if (transacao.TipoTransacao == TblTransacoes.Tipo.Gasto)
                {
                    account.Saldo -= transacao.ValorTransacao;
                }

                _logger.LogInformation($"Saldo da conta {account.NomeConta} atualizado para: {account.Saldo}");

                try
                {
                    // Guardar a transação na base de dados
                    _context.Add(transacao);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Transação salva com sucesso.");

                    // Guardar os valores das categorias selecionadas
                    foreach (var categoriaValor in CategoriaValores)
                    {
                        var transacaoCategoria = new TblTransacoesCategorias
                        {
                            TransacaoFK = transacao.TransacaoID,
                            CategoriaFK = categoriaValor.Key,
                            Valor = categoriaValor.Value
                        };
                        _context.Add(transacaoCategoria);
                        _logger.LogInformation($"Categoria adicionada: CategoriaFK: {categoriaValor.Key}, Valor: {categoriaValor.Value}");
                    }

                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Categorias salvas com sucesso.");

                    // Atualizar o saldo da conta na base de dados
                    _context.Update(account);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Saldo da conta atualizado com sucesso.");

                    _logger.LogInformation("Transação criada e saldo da conta atualizado com sucesso.");

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao salvar a transação.");
                    ModelState.AddModelError(string.Empty, "Erro ao salvar a transação.");
                }
            }
            catch (Exception ex)
         
            {
                _logger.LogWarning("Modelo de transação inválido.");
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        _logger.LogWarning($"Erro no campo {state.Key}: {error.ErrorMessage}");
                    }
                }
            }

            // Em caso de falha, recarrega a View com os dados de ViewBag
            ViewData["ContaFK"] = new SelectList(_context.Contas, "ContaID", "NomeConta", transacao.ContaFK);
            ViewData["TipoTransacao"] = new SelectList(new List<string> { "Ganho", "Gasto" });
            ViewData["CategoriaFK"] = new SelectList(_context.Categorias, "CategoriaID", "NomeCategoria");
            return View(transacao);
        }
    


        // GET: TblTransacoes/Index
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Acessando a página Index.");

            var transacoes = _context.Transacoes.Include(t => t.ContaFK).Include(t => t.ListaTransacoesCategorias).ThenInclude(tc => tc.Categoria);
            var transacoesList = await transacoes.ToListAsync();

            _logger.LogInformation($"Total de transações encontradas: {transacoesList.Count}");

            return View(transacoesList);
        }

        // GET: TblTransacoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            _logger.LogInformation($"Acessando os detalhes da transação ID: {id}");

            if (id == null)
            {
                _logger.LogWarning("ID de transação não fornecido.");
                return NotFound();
            }

            var transacao = await _context.Transacoes
                .Include(t => t.ContaFK)
                .Include(t => t.ListaTransacoesCategorias)
                .ThenInclude(tc => tc.Categoria)
                .FirstOrDefaultAsync(m => m.TransacaoID == id);

            if (transacao == null)
            {
                _logger.LogWarning($"Transação ID {id} não encontrada.");
                return NotFound();
            }

            return View(transacao);
        }

        // GET: TblTransacoes/Create
        public IActionResult Create()
        {
            _logger.LogInformation("Acessando a página de criação de transação.");

            ViewData["ContaFK"] = new SelectList(_context.Contas, "ContaID", "NomeConta");
            ViewData["TipoTransacao"] = new SelectList(new List<string> { "Ganho", "Gasto" });
            ViewData["CategoriaFK"] = new SelectList(_context.Categorias, "CategoriaID", "NomeCategoria");
            return View();
        }
    }
}
