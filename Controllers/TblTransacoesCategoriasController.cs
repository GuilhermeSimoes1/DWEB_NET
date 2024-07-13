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
            var applicationDbContext = _context.TransacoesCategorias.Include(t => t.Categoria).Include(t => t.Transacao);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TblTransacoesCategorias/Details
        public async Task<IActionResult> Details(int transacaoFK, int categoriaFK)
        {
            var tblTransacoesCategorias = await _context.TransacoesCategorias
                .Include(t => t.Categoria)
                .Include(t => t.Transacao)
                .FirstOrDefaultAsync(m => m.TransacaoFK == transacaoFK && m.CategoriaFK == categoriaFK);
            if (tblTransacoesCategorias == null)
            {
                return NotFound();
            }

            return View(tblTransacoesCategorias);
        }

        // GET: TblTransacoesCategorias/Create
        public IActionResult Create()
        {
            ViewData["CategoriaFK"] = new SelectList(_context.Categorias, "CategoriaID", "NomeCategoria");
            ViewData["TransacaoFK"] = new SelectList(_context.Transacoes, "TransacaoID", "Descricao");
            return View();
        }

        // POST: TblTransacoesCategorias/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TransacaoFK,CategoriaFK,Valor")] TblTransacoesCategorias tblTransacoesCategorias)
        {
            
            {
                _context.Add(tblTransacoesCategorias);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaFK"] = new SelectList(_context.Categorias, "CategoriaID", "NomeCategoria", tblTransacoesCategorias.CategoriaFK);
            ViewData["TransacaoFK"] = new SelectList(_context.Transacoes, "TransacaoID", "Descricao", tblTransacoesCategorias.TransacaoFK);
            return View(tblTransacoesCategorias);
        }

        // GET: TblTransacoesCategorias/Edit
        public async Task<IActionResult> Edit(int transacaoFK, int categoriaFK)
        {
            var tblTransacoesCategorias = await _context.TransacoesCategorias
                .Include(t => t.Categoria)
                .Include(t => t.Transacao)
                .FirstOrDefaultAsync(m => m.TransacaoFK == transacaoFK && m.CategoriaFK == categoriaFK);

            if (tblTransacoesCategorias == null)
            {
                return NotFound();
            }

            // Carrega as categorias disponíveis para o dropdown
            ViewBag.Categorias = new SelectList(_context.Categorias, "CategoriaID", "NomeCategoria", tblTransacoesCategorias.CategoriaFK);

            return View(tblTransacoesCategorias);
        }


        // POST: TblTransacoesCategorias/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int transacaoFK, int categoriaFK, [Bind("TransacaoFK,CategoriaFK,Valor")] TblTransacoesCategorias tblTransacoesCategorias)
        {
            if (transacaoFK != tblTransacoesCategorias.TransacaoFK || categoriaFK != tblTransacoesCategorias.CategoriaFK)
            {
                return NotFound();
            }

           
                try
                {
                    // Carrega a entidade existente do banco de dados
                    var existingEntity = await _context.TransacoesCategorias
                        .Include(t => t.Transacao)
                        .Include(t => t.Categoria)
                        .FirstOrDefaultAsync(m => m.TransacaoFK == transacaoFK && m.CategoriaFK == categoriaFK);

                    if (existingEntity == null)
                    {
                        return NotFound();
                    }

                    // Atualiza as propriedades da entidade existente
                    existingEntity.CategoriaFK = tblTransacoesCategorias.CategoriaFK;
                    existingEntity.Valor = tblTransacoesCategorias.Valor;

                    // Atualiza o valor da transação associada
                    var transacao = await _context.Transacoes.FindAsync(transacaoFK);
                    if (transacao != null)
                    {
                        transacao.ValorTransacao = tblTransacoesCategorias.Valor;
                        _context.Update(transacao);
                    }

                    // Salva as alterações
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblTransacoesCategoriasExists(tblTransacoesCategorias.TransacaoFK, tblTransacoesCategorias.CategoriaFK))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
        
            ViewData["CategoriaFK"] = new SelectList(_context.Categorias, "CategoriaID", "NomeCategoria", tblTransacoesCategorias.CategoriaFK);
            ViewData["TransacaoFK"] = new SelectList(_context.Transacoes, "TransacaoID", "Descricao", tblTransacoesCategorias.TransacaoFK);
            return View(tblTransacoesCategorias);
        }


        // GET: TblTransacoesCategorias/Delete
        public async Task<IActionResult> Delete(int transacaoFK, int categoriaFK)
        {
            var tblTransacoesCategorias = await _context.TransacoesCategorias
                .Include(t => t.Categoria)
                .Include(t => t.Transacao)
                .FirstOrDefaultAsync(m => m.TransacaoFK == transacaoFK && m.CategoriaFK == categoriaFK);
            if (tblTransacoesCategorias == null)
            {
                return NotFound();
            }

            return View(tblTransacoesCategorias);
        }

        // POST: TblTransacoesCategorias/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int transacaoFK, int categoriaFK)
        {
            var tblTransacoesCategorias = await _context.TransacoesCategorias
                .FirstOrDefaultAsync(m => m.TransacaoFK == transacaoFK && m.CategoriaFK == categoriaFK);
            if (tblTransacoesCategorias != null)
            {
                _context.TransacoesCategorias.Remove(tblTransacoesCategorias);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool TblTransacoesCategoriasExists(int transacaoFK, int categoriaFK)
        {
            return _context.TransacoesCategorias.Any(e => e.TransacaoFK == transacaoFK && e.CategoriaFK == categoriaFK);
        }

        //FUNÇÕES USADAS NO EDIT

    }
}
