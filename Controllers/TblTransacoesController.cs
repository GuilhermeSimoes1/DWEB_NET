using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DWEB_NET.Data;
using DWEB_NET.Models;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using System.Security.Claims;

namespace DWEB_NET.Controllers
{
    public class TblTransacoesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TblTransacoesController> _logger;
        private readonly List<int> adminUserIds = new List<int> { 1,2,3,4,5 };

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




        // GET: TblTransacoes
        public async Task<IActionResult> Index()
        {
            // Obter o Id do utilizador logado
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Encontrar o utilizador associado ao email
            var userAutent = await _context.Utilizadores.FirstOrDefaultAsync(u => u.UserAutent == userId);
            if(userAutent.IsAdmin == true)

            {

                var applicationDbContextAdmin = _context.Transacoes.Include(t => t.Conta).Include(t => t.User);
                return View(await applicationDbContextAdmin.ToListAsync());
            }

                var applicationDbContextUser = _context.Transacoes.Where(t => t.UserFK == userAutent.UserID ).Include(t => t.Conta).Include(t => t.User);
                return View(await applicationDbContextUser.ToListAsync());

        }


        

        // GET: TblTransacoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblTransacoes = await _context.Transacoes
                .Include(t => t.Conta)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.TransacaoID == id);
            if (tblTransacoes == null)
            {
                return NotFound();
            }

            return View(tblTransacoes);
        }
        // GET: TblTransacoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblTransacoes = await _context.Transacoes.FindAsync(id);
            if (tblTransacoes == null)
            {
                return NotFound();
            }
            ViewData["ContaFK"] = new SelectList(_context.Contas, "ContaID", "ContaID", tblTransacoes.ContaFK);
            ViewData["UserFK"] = new SelectList(_context.Utilizadores, "UserID", "Email", tblTransacoes.UserFK);
            return View(tblTransacoes);
        }

        // POST: TblTransacoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TransacaoID,DataTime,TipoTransacao,Descricao,ValorTransacao,ContaFK,UserFK")] TblTransacoes tblTransacoes)
        {
            if (id != tblTransacoes.TransacaoID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblTransacoes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblTransacoesExists(tblTransacoes.TransacaoID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ContaFK"] = new SelectList(_context.Contas, "ContaID", "ContaID", tblTransacoes.ContaFK);
            ViewData["UserFK"] = new SelectList(_context.Utilizadores, "UserID", "Email", tblTransacoes.UserFK);
            return View(tblTransacoes);
        }

        // GET: TblTransacoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblTransacoes = await _context.Transacoes
                .Include(t => t.Conta)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.TransacaoID == id);
            if (tblTransacoes == null)
            {
                return NotFound();
            }

            return View(tblTransacoes);
        }

        // POST: TblTransacoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tblTransacoes = await _context.Transacoes.FindAsync(id);
            if (tblTransacoes != null)
            {
                _context.Transacoes.Remove(tblTransacoes);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblTransacoesExists(int id)
        {
            return _context.Transacoes.Any(e => e.TransacaoID == id);
        }






//UsersNormais-------------------------------------------------------------------------------------------------------------------------------
  
        // GET: TblTransacoes/Create
        public async Task<IActionResult> Create()
        {
            // Obter o Id do utilizador logado
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Encontrar o utilizador associado ao email
            var userAutent = await _context.Utilizadores.FirstOrDefaultAsync(u => u.UserAutent == userId);

            // Filtrar as contas associadas ao utilizador logado
            var contas = await _context.Contas
                .Where(c => c.UserFK == userAutent.UserID)
                .ToListAsync();

            // Construir o SelectList das contas filtradas
            var selectList = contas.Select(c => new SelectListItem
            {
                Value = c.ContaID.ToString(),
                Text = c.NomeConta
            }).ToList();

            // Passar o SelectList para a ViewBag
            ViewBag.ContaFK = new SelectList(selectList, "Value", "Text");

            return View();
        }


        // POST: TblTransacoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                    ModelState.AddModelError(string.Empty, "C   onta não encontrada.");
                    ViewData["ContaFK"] = new SelectList(_context.Contas, "ContaID", "NomeConta", transacao.ContaFK);
                    ViewData["TipoTransacao"] = new SelectList(new List<string> { "Ganho", "Gasto" });
                    ViewData["CategoriaFK"] = new SelectList(_context.Categorias, "CategoriaID", "NomeCategoria");
                    return View(transacao);
                }

                //A conta existe então podemos usar o nome dela
                var NomeDaConta = account.NomeConta;
                ViewBag.NomeConta = NomeDaConta;

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

                    if (decimal.Compare(value, 0) != 0)
                    {
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

                //Data de quando foi feita a transação
                var DataAtual = DateTime.Now;
                transacao.DataTime = DataAtual;

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

//UsersNormais-------------------------------------------------------------------------------------------------------------------------------
        }

    }
}
