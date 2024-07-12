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
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Security.Cryptography;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;


namespace DWEB_NET.Controllers
{
    public class TblUtilizadoresController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public TblUtilizadoresController(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
        }

        // GET: TblUtilizadores
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userAutent = await _context.Utilizadores.FirstOrDefaultAsync(u => u.UserAutent == userId);

            IQueryable<TblUtilizadores> utilizadoresQuery;

            if (userAutent != null && userAutent.IsAdmin)
            {
                // Admin pode ver todos os utilizadores
                utilizadoresQuery = _context.Utilizadores;
            }
            else
            {
                // Utilizador normal só pode ver o seu próprio utilizador
                utilizadoresQuery = _context.Utilizadores.Where(u => u.UserAutent == userId);
            }
            ViewBag.IsAdmin = userAutent?.IsAdmin ?? false;
            var utilizadores = await utilizadoresQuery.ToListAsync();
            return View(utilizadores);
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
        public async Task<IActionResult> Create([Bind("UserID,UserName,Email,FirstName,LastName,Descricao,UserAutent,Password")] TblUtilizadores tblUtilizadores)
        {
            {

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                tblUtilizadores.UserAutent = userId;


                _context.Add(tblUtilizadores);
                await _context.SaveChangesAsync();

                
                var user = new IdentityUser
                {
                    UserName = tblUtilizadores.UserName,
                    Email = tblUtilizadores.Email,
                    PasswordHash = HashPassword(tblUtilizadores.Password),
                    EmailConfirmed = true,
                };
                var result = await _userManager.CreateAsync(user);

                if (result.Succeeded)
                {
                    
                    tblUtilizadores.UserAutent = user.Id; 

                    
                    _context.Update(tblUtilizadores);
                    await _context.SaveChangesAsync();
                  
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(tblUtilizadores);
        }


    private string HashPassword(string password)
    {
        var passwordHasher = new PasswordHasher<IdentityUser>();
        return passwordHasher.HashPassword(null, password);
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
        public async Task<IActionResult> Edit(int id, [Bind("UserID,UserName,Email,FirstName,LastName,Descricao,UserAutent,Password")] TblUtilizadores tblUtilizadores)
        {
            if (id != tblUtilizadores.UserID)
            {
                return NotFound();
            }

            try
            {
                // Encontra o usuário correspondente em AspNetUsers
                var user = await _userManager.FindByIdAsync(tblUtilizadores.UserAutent);
                if (user == null)
                {
                    return NotFound(); // Se não encontrar o usuário, retorna NotFound
                }

                // Atualiza os dados do usuário em AspNetUsers com os dados da tabela TblUtilizadores
                user.UserName = tblUtilizadores.UserName;
                user.Email = tblUtilizadores.Email;

                // Se uma nova senha foi fornecida, atualiza também a senha em AspNetUsers
                if (!string.IsNullOrEmpty(tblUtilizadores.Password))
                {
                    user.PasswordHash = HashPassword(tblUtilizadores.Password);
                }

                // Salva as mudanças no usuário em AspNetUsers
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    foreach (var error in updateResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(tblUtilizadores); // Retorna a view de edição com os erros de validação
                }

                // Atualiza os dados na tabela TblUtilizadores
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
                    return RedirectToAction(nameof(Index));
                }
            }

            return RedirectToAction(nameof(Index));
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
          
            var tblUtilizadores = await _context.Utilizadores.FirstOrDefaultAsync(u => u.UserID == id);
            
            if (tblUtilizadores != null)
            {
                
                var user = await _userManager.FindByIdAsync(tblUtilizadores.UserAutent);
                _context.Utilizadores.Remove(tblUtilizadores);

                
                if (user != null)
                {
                    var result = await _userManager.DeleteAsync(user);
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", "Failed to delete the user.");
                        return View();
                    }
                }
            }
            else
            {
                
                ModelState.AddModelError("", "Utilizadores record not found.");
                return View();
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
