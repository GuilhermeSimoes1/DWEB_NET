using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DWEB_NET.Data;
using DWEB_NET.Models;

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
            var applicationDbContext = _context.Orcamentos.Include(t => t.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TblOrcamentos/Details/5
        public async Task<IActionResult> Details(int? id)
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
        public async Task<IActionResult> Create([Bind("OrcamentoID,NomeOrcamento,ValorNecessario,DataInicial,DataFinal,ValorAtual,UserFK")] TblOrcamentos tblOrcamentos)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tblOrcamentos);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserFK"] = new SelectList(_context.Utilizadores, "UserID", "Email", tblOrcamentos.UserFK);
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

            if (ModelState.IsValid)
            {
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
                return RedirectToAction(nameof(Index));
            }
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
            return RedirectToAction(nameof(Index));
        }

        private bool TblOrcamentosExists(int id)
        {
            return _context.Orcamentos.Any(e => e.OrcamentoID == id);
        }
    }
}
