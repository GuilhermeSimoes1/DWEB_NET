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
    public class TblUtilizadoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TblUtilizadoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TblUtilizadores
        public async Task<IActionResult> Index()
        {
            return View(await _context.Utilizadores.ToListAsync());
        }

        // GET: TblUtilizadores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblUtilizadores = await _context.Utilizadores
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (tblUtilizadores == null)
            {
                return NotFound();
            }

            return View(tblUtilizadores);
        }

        // GET: TblUtilizadores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TblUtilizadores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserID,UserName,Email,Passwd,FirstName,LastName,Descricao,UserAutent")] TblUtilizadores tblUtilizadores)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tblUtilizadores);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tblUtilizadores);
        }

        // GET: TblUtilizadores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblUtilizadores = await _context.Utilizadores.FindAsync(id);
            if (tblUtilizadores == null)
            {
                return NotFound();
            }
            return View(tblUtilizadores);
        }

        // POST: TblUtilizadores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserID,UserName,Email,Passwd,FirstName,LastName,Descricao,UserAutent")] TblUtilizadores tblUtilizadores)
        {
            if (id != tblUtilizadores.UserID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblUtilizadores);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblUtilizadoresExists(tblUtilizadores.UserID))
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
            return View(tblUtilizadores);
        }

        // GET: TblUtilizadores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblUtilizadores = await _context.Utilizadores
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (tblUtilizadores == null)
            {
                return NotFound();
            }

            return View(tblUtilizadores);
        }

        // POST: TblUtilizadores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tblUtilizadores = await _context.Utilizadores.FindAsync(id);
            if (tblUtilizadores != null)
            {
                _context.Utilizadores.Remove(tblUtilizadores);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblUtilizadoresExists(int id)
        {
            return _context.Utilizadores.Any(e => e.UserID == id);
        }
    }
}
