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
    public class TblOrcamentosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TblOrcamentosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TblOrcamentos
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
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
        // GET: TblOrcamentos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var orcamento = await _context.Orcamentos
              .FirstOrDefaultAsync(m => m.OrcamentoID == id);
            ViewBag.NomeOrcamento = await _context.Orcamentos.ToListAsync();
            return View(orcamento);
        }

        // GET: TblOrcamentos/Create
        public IActionResult Create()
        {
            ViewData["UserFK"] = new SelectList(_context.Utilizadores, "UserID", "Email");
            return View();
        }

        // POST: TblOrcamentos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NomeOrcamento,ValorNecessario,DataInicial,DataFinal,ValorAtual,UserFK")] TblOrcamentos tblOrcamentos)
        {
            
                _context.Add(tblOrcamentos);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details));
                ViewBag.UserFK = new SelectList(_context.Users, "UserId", "UserName", tblOrcamentos.UserFK);

                return View(tblOrcamentos);
        }
    
    // GET: TblOrcamentos/Edit/5
    public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblOrcamentos = await _context.Orcamentos.FindAsync(id);
            if (tblOrcamentos == null)
            {
                return NotFound();
            }
            ViewData["UserFK"] = new SelectList(_context.Utilizadores, "UserID", "Email", tblOrcamentos.UserFK);
            return View(tblOrcamentos);
        }

        // POST: TblOrcamentos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrcamentoID,NomeOrcamento,ValorNecessario,DataInicial,DataFinal,ValorAtual,UserFK")] TblOrcamentos tblOrcamentos)
        {
            if (id != tblOrcamentos.OrcamentoID)
            {
                return NotFound();
            }

            
                try
                {
                    _context.Update(tblOrcamentos);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblOrcamentosExists(tblOrcamentos.OrcamentoID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details));
            
            ViewData["UserFK"] = new SelectList(_context.Utilizadores, "UserID", "Email", tblOrcamentos.UserFK);
            return View(tblOrcamentos);
        }

        // GET: TblOrcamentos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblOrcamentos = await _context.Orcamentos
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.OrcamentoID == id);
            if (tblOrcamentos == null)
            {
                return NotFound();
            }

            return View(tblOrcamentos);
        }

        // POST: TblOrcamentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tblOrcamentos = await _context.Orcamentos.FindAsync(id);
            if (tblOrcamentos != null)
            {
                _context.Orcamentos.Remove(tblOrcamentos);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details));
        }

        private bool TblOrcamentosExists(int id)
        {
            return _context.Orcamentos.Any(e => e.OrcamentoID == id);
        }
    }
}
