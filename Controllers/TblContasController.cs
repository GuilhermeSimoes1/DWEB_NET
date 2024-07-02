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
    public class TblContasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TblContasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TblContas
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Contas.Include(t => t.UserID);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TblContas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblContas = await _context.Contas
                .Include(t => t.UserID)
                .FirstOrDefaultAsync(m => m.ContaID == id);
            if (tblContas == null)
            {
                return NotFound();
            }

            return View(tblContas);
        }

        // GET: TblContas/Create
        public IActionResult Create()
        {
            ViewData["UserFK"] = new SelectList(_context.Utilizadores, "UserID", "Email");
            return View();
        }

        // POST: TblContas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContaID,NomeConta,Saldo,UserFK")] TblContas tblContas)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tblContas);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserFK"] = new SelectList(_context.Utilizadores, "UserID", "Email", tblContas.UserFK);
            return View(tblContas);
        }

        // GET: TblContas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblContas = await _context.Contas.FindAsync(id);
            if (tblContas == null)
            {
                return NotFound();
            }
            ViewData["UserFK"] = new SelectList(_context.Utilizadores, "UserID", "Email", tblContas.UserFK);
            return View(tblContas);
        }

        // POST: TblContas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ContaID,NomeConta,Saldo,UserFK")] TblContas tblContas)
        {
            if (id != tblContas.ContaID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
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
            }
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
                .Include(t => t.UserID)
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
