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
    public class TblTransacoesCategoriasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TblTransacoesCategoriasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TblTransacoesCategorias
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TransacoesCategorias.Include(t => t.CategoriaID).Include(t => t.TransacaoID);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TblTransacoesCategorias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblTransacoesCategorias = await _context.TransacoesCategorias
                .Include(t => t.CategoriaID)
                .Include(t => t.TransacaoID)
                .FirstOrDefaultAsync(m => m.TransacaoFK == id);
            if (tblTransacoesCategorias == null)
            {
                return NotFound();
            }

            return View(tblTransacoesCategorias);
        }

        // GET: TblTransacoesCategorias/Create
        public IActionResult Create()
        {
            ViewData["CategoriaFK"] = new SelectList(_context.Categorias, "CategoriaID", "CategoriaID");
            ViewData["TransacaoFK"] = new SelectList(_context.Transacoes, "TransacaoID", "TransacaoID");
            return View();
        }

        // POST: TblTransacoesCategorias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TransacaoFK,CategoriaFK")] TblTransacoesCategorias tblTransacoesCategorias)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tblTransacoesCategorias);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaFK"] = new SelectList(_context.Categorias, "CategoriaID", "CategoriaID", tblTransacoesCategorias.CategoriaFK);
            ViewData["TransacaoFK"] = new SelectList(_context.Transacoes, "TransacaoID", "TransacaoID", tblTransacoesCategorias.TransacaoFK);
            return View(tblTransacoesCategorias);
        }

        // GET: TblTransacoesCategorias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblTransacoesCategorias = await _context.TransacoesCategorias.FindAsync(id);
            if (tblTransacoesCategorias == null)
            {
                return NotFound();
            }
            ViewData["CategoriaFK"] = new SelectList(_context.Categorias, "CategoriaID", "CategoriaID", tblTransacoesCategorias.CategoriaFK);
            ViewData["TransacaoFK"] = new SelectList(_context.Transacoes, "TransacaoID", "TransacaoID", tblTransacoesCategorias.TransacaoFK);
            return View(tblTransacoesCategorias);
        }

        // POST: TblTransacoesCategorias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TransacaoFK,CategoriaFK")] TblTransacoesCategorias tblTransacoesCategorias)
        {
            if (id != tblTransacoesCategorias.TransacaoFK)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblTransacoesCategorias);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblTransacoesCategoriasExists(tblTransacoesCategorias.TransacaoFK))
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
            ViewData["CategoriaFK"] = new SelectList(_context.Categorias, "CategoriaID", "CategoriaID", tblTransacoesCategorias.CategoriaFK);
            ViewData["TransacaoFK"] = new SelectList(_context.Transacoes, "TransacaoID", "TransacaoID", tblTransacoesCategorias.TransacaoFK);
            return View(tblTransacoesCategorias);
        }

        // GET: TblTransacoesCategorias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblTransacoesCategorias = await _context.TransacoesCategorias
                .Include(t => t.CategoriaID)
                .Include(t => t.TransacaoID)
                .FirstOrDefaultAsync(m => m.TransacaoFK == id);
            if (tblTransacoesCategorias == null)
            {
                return NotFound();
            }

            return View(tblTransacoesCategorias);
        }

        // POST: TblTransacoesCategorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tblTransacoesCategorias = await _context.TransacoesCategorias.FindAsync(id);
            if (tblTransacoesCategorias != null)
            {
                _context.TransacoesCategorias.Remove(tblTransacoesCategorias);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblTransacoesCategoriasExists(int id)
        {
            return _context.TransacoesCategorias.Any(e => e.TransacaoFK == id);
        }
    }
}
