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
    public class TblTransacoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TblTransacoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TblTransacoes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Transacoes.Include(t => t.CategoriaID).Include(t => t.ContaID).Include(t => t.UserID);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TblTransacoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblTransacoes = await _context.Transacoes
                .Include(t => t.CategoriaID)
                .Include(t => t.ContaID)
                .Include(t => t.UserID)
                .FirstOrDefaultAsync(m => m.TransacaoID == id);
            if (tblTransacoes == null)
            {
                return NotFound();
            }

            return View(tblTransacoes);
        }

        // GET: TblTransacoes/Create
        public IActionResult Create()
        {
            ViewData["CategoriaFK"] = new SelectList(_context.Categorias, "CategoriaID", "CategoriaID");
            ViewData["ContaFK"] = new SelectList(_context.Contas, "ContaID", "ContaID");
            ViewData["UserFK"] = new SelectList(_context.Utilizadores, "UserID", "Email");
            return View();
        }

        // POST: TblTransacoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TransacaoID,DataTime,Descricao,ValorTransacao,ContaFK,CategoriaFK,UserFK")] TblTransacoes tblTransacoes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tblTransacoes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaFK"] = new SelectList(_context.Categorias, "CategoriaID", "CategoriaID", tblTransacoes.CategoriaFK);
            ViewData["ContaFK"] = new SelectList(_context.Contas, "ContaID", "ContaID", tblTransacoes.ContaFK);
            ViewData["UserFK"] = new SelectList(_context.Utilizadores, "UserID", "Email", tblTransacoes.UserFK);
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
            ViewData["CategoriaFK"] = new SelectList(_context.Categorias, "CategoriaID", "CategoriaID", tblTransacoes.CategoriaFK);
            ViewData["ContaFK"] = new SelectList(_context.Contas, "ContaID", "ContaID", tblTransacoes.ContaFK);
            ViewData["UserFK"] = new SelectList(_context.Utilizadores, "UserID", "Email", tblTransacoes.UserFK);
            return View(tblTransacoes);
        }

        // POST: TblTransacoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TransacaoID,DataTime,Descricao,ValorTransacao,ContaFK,CategoriaFK,UserFK")] TblTransacoes tblTransacoes)
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
            ViewData["CategoriaFK"] = new SelectList(_context.Categorias, "CategoriaID", "CategoriaID", tblTransacoes.CategoriaFK);
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
                .Include(t => t.CategoriaID)
                .Include(t => t.ContaID)
                .Include(t => t.UserID)
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
    }
}
