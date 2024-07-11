
using DWEB_NET.Data;
using DWEB_NET.Models;
using DWEB_NET.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class V1Controller : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ILogger<V1Controller> _logger;

    public V1Controller(ApplicationDbContext context, SignInManager<IdentityUser> signInManager, ILogger<V1Controller> logger, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _signInManager = signInManager;
        _logger = logger;
        _userManager = userManager;
    }

    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login([FromBody] LoginModel ola)
    {
        if (string.IsNullOrEmpty(ola.Email) || string.IsNullOrEmpty(ola.Password))
        {
            return BadRequest("Invalid login request.");
        }

        try
        {
            var resultUser = await _userManager.FindByEmailAsync(ola.Email);

            if (resultUser != null)
            {
                var passWorks = new PasswordHasher<IdentityUser>().VerifyHashedPassword(resultUser, resultUser.PasswordHash, ola.Password);

                if (passWorks == PasswordVerificationResult.Success)
                {
                    await _signInManager.SignInAsync(resultUser, ola.RememberMe); 

                    var user = _context.Utilizadores.FirstOrDefault(u => u.UserAutent == resultUser.Id);

                    if (user != null)
                    {
                        return Ok(user);
                    }
                    else
                    {
                        return BadRequest("User details not found");
                    }
                }
                else
                {
                    return BadRequest("Invalid password");
                }
            }
            else
            {
                return BadRequest("User not found");
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message); 
        }
    }

    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> Register([FromBody] TblUtilizadores model)
    {
        try
        {
            var user = new IdentityUser { 
                UserName = model.UserName, 
                Email = model.Email,
                Id = Guid.NewGuid().ToString(),
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password); 

            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                var tblUtilizadores = new TblUtilizadores
                {
                    UserAutent = user.Id,
                    UserName = model.UserName,
                    Email = model.Email,
                    Password = user.PasswordHash,
                    FirstName = model.UserName,
                    LastName = model.UserName,
                    IsAdmin = false,
                    Descricao = model.Descricao
                };

                _context.Utilizadores.Add(tblUtilizadores);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Registration successful. Please confirm your email." });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            _logger.LogError("Registration failed: " + string.Join("; ", errors));
            return BadRequest(ModelState);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred during registration: {ex.Message}");
            return StatusCode(500, new { message = "An error occurred. Please try again." });
        }
    }
}
