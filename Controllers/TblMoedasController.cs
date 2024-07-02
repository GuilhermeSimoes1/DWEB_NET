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
    public class TblMoedasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TblMoedasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TblMoedas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Moedas.ToListAsync());
        }

        // GET: TblMoedas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblMoedas = await _context.Moedas
                .FirstOrDefaultAsync(m => m.MoedaID == id);
            if (tblMoedas == null)
            {
                return NotFound();
            }

            return View(tblMoedas);
        }

        // GET: TblMoedas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TblMoedas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MoedaID,NomeMoeda")] TblMoedas tblMoedas)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tblMoedas);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tblMoedas);
        }

        // GET: TblMoedas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblMoedas = await _context.Moedas.FindAsync(id);
            if (tblMoedas == null)
            {
                return NotFound();
            }
            return View(tblMoedas);
        }

        // POST: TblMoedas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MoedaID,NomeMoeda")] TblMoedas tblMoedas)
        {
            if (id != tblMoedas.MoedaID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblMoedas);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblMoedasExists(tblMoedas.MoedaID))
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
            return View(tblMoedas);
        }

        // GET: TblMoedas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblMoedas = await _context.Moedas
                .FirstOrDefaultAsync(m => m.MoedaID == id);
            if (tblMoedas == null)
            {
                return NotFound();
            }

            return View(tblMoedas);
        }

        // POST: TblMoedas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tblMoedas = await _context.Moedas.FindAsync(id);
            if (tblMoedas != null)
            {
                _context.Moedas.Remove(tblMoedas);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblMoedasExists(int id)
        {
            return _context.Moedas.Any(e => e.MoedaID == id);
        }
    }
}
