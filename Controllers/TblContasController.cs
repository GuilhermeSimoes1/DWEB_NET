using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DWEB_NET.Data;
using DWEB_NET.Models;
using System.Security.Claims;

namespace DWEB_NET.Controllers
{
    public class TblContasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TblContasController> _logger;

        public TblContasController(ApplicationDbContext context, ILogger<TblContasController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: TblContas
        public async Task<IActionResult> Index()
        {
            // Obter o Id do utilizador logado
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Encontrar o utilizador associado 
            var userAutent = await _context.Utilizadores.FirstOrDefaultAsync(u => u.UserAutent == userId);

            if (userAutent.IsAdmin == true)
            {
                var applicationDbContextAdmin = _context.Contas.Include(t => t.User);
                return View(await applicationDbContextAdmin.ToListAsync());
            }
            else
            {
                var applicationDbContextUser = _context.Contas
                    .Where(t => t.UserFK == userAutent.UserID)
                    .Include(t => t.User);
                return View(await applicationDbContextUser.ToListAsync());
            }
        }

        // GET: TblContas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblContas = await _context.Contas
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.ContaID == id);
            if (tblContas == null)
            {
                return NotFound();
            }

            return View(tblContas);
        }

        // GET: TblContas/Create
        public async Task<IActionResult> Create()
        {
            // Obter o Id do utilizador logado
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Encontrar o utilizador 
            var userAutent = await _context.Utilizadores.FirstOrDefaultAsync(u => u.UserAutent == userId);

            if (userAutent.IsAdmin)
            {
                ViewData["UserFK"] = new SelectList(_context.Utilizadores, "UserID", "Email");
            }
            else
            {
                ViewData["UserFK"] = new SelectList(new List<TblUtilizadores> { userAutent }, "UserID", "Email");
            }

            return View();
        }

        // POST: TblContas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContaID,NomeConta,Saldo,UserFK")] TblContas tblContas)
        {
            try
            {
                // Obter o ID do usuário autenticado
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Verificar se o usuário autenticado existe
                var userAutent = await _context.Utilizadores.FirstOrDefaultAsync(u => u.UserAutent == userId);
                if (userAutent == null)
                {
                    return NotFound("Usuário autenticado não encontrado.");
                }

                // Validação manual
                if (string.IsNullOrEmpty(tblContas.NomeConta))
                {
                    ModelState.AddModelError("NomeConta", "O campo NomeConta é obrigatório.");
                }

                if (tblContas.Saldo < 0)
                {
                    ModelState.AddModelError("Saldo", "O Saldo não pode ser negativo.");
                }

                // Verificar se há erros de validação
              
                {
                    // Adicionar a conta ao contexto e salvar no banco de dados
                    _context.Add(tblContas);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
               
                ModelState.AddModelError(string.Empty, "Ocorreu um erro ao tentar criar a conta.");
            }

            // Caso haja falha, preparar ViewData["UserFK"] para o formulário
            ViewData["UserFK"] = new SelectList(_context.Utilizadores, "UserID", "Email", tblContas.UserFK);

            // Retornar para a view com os dados preenchidos e erros de validação, se houverem
            return View(tblContas);
        }


        // GET: TblContas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Obter o Id do utilizador logado
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Encontrar o utilizador 
            var userAutent = await _context.Utilizadores.FirstOrDefaultAsync(u => u.UserAutent == userId);
            if (userAutent == null)
            {
                return NotFound();
            }

            // Encontrar a conta com base no ID
            var tblContas = await _context.Contas.FindAsync(id);
            if (tblContas == null)
            {
                return NotFound();
            }

            // Verificar se o utilizador é administrador
            if (userAutent.IsAdmin)
            {
                ViewData["UserFK"] = new SelectList(_context.Utilizadores, "UserID", "Email", tblContas.UserFK);
            }
            else
            {
                // Se não for administrador, garantir que só pode selecionar a própria conta
                ViewData["UserFK"] = new SelectList(new List<TblUtilizadores> { userAutent }, "UserID", "Email", tblContas.UserFK);
            }

            return View(tblContas);

        }

        // POST: TblContas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ContaID,NomeConta,Saldo,UserFK")] TblContas tblContas)
        {
            if (id != tblContas.ContaID)
            {
                return NotFound();
            }

            try
                {
                    _context.Update(tblContas);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblContasExists(tblContas.ContaID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            
            ViewData["UserFK"] = new SelectList(_context.Utilizadores, "UserID", "Email", tblContas.UserFK);
            return View(tblContas);
        }

        // GET: TblContas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblContas = await _context.Contas
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.ContaID == id);
            if (tblContas == null)
            {
                return NotFound();
            }

            return View(tblContas);
        }

        // POST: TblContas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tblContas = await _context.Contas.FindAsync(id);
            if (tblContas != null)
            {
                _context.Contas.Remove(tblContas);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblContasExists(int id)
        {
            return _context.Contas.Any(e => e.ContaID == id);
        }
    }
}
