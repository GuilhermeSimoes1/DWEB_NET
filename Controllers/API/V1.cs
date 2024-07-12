using DWEB_NET.Data;
using DWEB_NET.Models;
using DWEB_NET.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DWEB_NET.Controllers
{
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

        [HttpGet]
        [Route("Orcamentos/{userId}")]
        public IActionResult GetOrcamentos(int userId)
        {
            try
            {
                var user = _context.Utilizadores.FirstOrDefault(u => u.UserID == userId);

                if (user == null)
                {
                    return NotFound("User not found.");
                }

                var orcamentos = _context.Orcamentos
                    .Where(o => o.UserFK == userId)
                    .Select(o => new
                    {
                        o.OrcamentoID,
                        o.NomeOrcamento,
                        o.ValorNecessario,
                        o.DataInicial,
                        o.DataFinal,
                        o.ValorAtual
                    })
                    .ToList();

                return Ok(orcamentos);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving budgets: {ex.Message}");
                return StatusCode(500, new { message = "Failed to retrieve budgets. Please try again." });
            }
        }

        [HttpPost]
        [Route("Orcamentos")]
        public IActionResult AdicionarOrcamento([FromBody] OrcamentoModel model)
        {
            try
            {
                var user = _context.Utilizadores.FirstOrDefault(u => u.UserID == model.UserFK);
                if (user == null)
                {
                    return NotFound("User not found.");
                }
                var user2 = new TblUtilizadores
                {
                    UserID = model.UserFK
                };

                var novoOrcamento = new TblOrcamentos
                {
                    NomeOrcamento = model.NomeOrcamento,
                    ValorNecessario = model.ValorNecessario,
                    DataInicial = model.DataInicial,
                    DataFinal = model.DataFinal,
                    ValorAtual = model.ValorAtual,
                    UserFK = user2.UserID
                };

                _context.Orcamentos.Add(novoOrcamento);
                _context.SaveChanges();

                return Ok(new { 
                    orcamentoId = novoOrcamento.OrcamentoID,
                    nomeOrcamento = novoOrcamento.NomeOrcamento,
                    valorNecessario = novoOrcamento.ValorNecessario, 
                    dataInicial = novoOrcamento.DataInicial,  
                    datafinal = novoOrcamento.DataFinal,
                    valorAtual = novoOrcamento.ValorAtual,
                    userFK = novoOrcamento.UserFK,
                    message = "Orçamento adicionado com sucesso." });

            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while adding budget: {ex.Message}");
                return StatusCode(500, new { message = "Failed to add budget. Please try again." });
            }
        }

        [HttpPut]
        [Route("Orcamentos/{orcamentoId}")]
        public IActionResult EditarOrcamento(int orcamentoId, [FromBody] OrcamentoModel model)
        {
            try
            {
                var orcamento = _context.Orcamentos.FirstOrDefault(o => o.OrcamentoID == orcamentoId);
                if (orcamento == null)
                {
                    return NotFound("Budget not found.");
                }

                orcamento.NomeOrcamento = model.NomeOrcamento;
                orcamento.ValorNecessario = model.ValorNecessario;
                orcamento.DataInicial = model.DataInicial;
                orcamento.DataFinal = model.DataFinal;
                orcamento.ValorAtual = model.ValorAtual;

                _context.SaveChanges();

                return Ok(new { message = "Orçamento atualizado com sucesso." });
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while updating budget: {ex.Message}");
                return StatusCode(500, new { message = "Failed to update budget. Please try again." });
            }
        }

        [HttpDelete]
        [Route("Orcamentos/{orcamentoId}")]
        public IActionResult ExcluirOrcamento(int orcamentoId)
        {
            try
            {
                var orcamento = _context.Orcamentos.FirstOrDefault(o => o.OrcamentoID == orcamentoId);
                if (orcamento == null)
                {
                    return NotFound("Budget not found.");
                }

                _context.Orcamentos.Remove(orcamento);
                _context.SaveChanges();

                return Ok(new { message = "Orçamento excluído com sucesso." });
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while deleting budget: {ex.Message}");
                return StatusCode(500, new { message = "Failed to delete budget. Please try again." });
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest("Invalid login request.");
            }

            try
            {
                var resultUser = await _userManager.FindByEmailAsync(model.Email);

                if (resultUser != null)
                {
                    var passWorks = new PasswordHasher<IdentityUser>().VerifyHashedPassword(resultUser, resultUser.PasswordHash, model.Password);

                    if (passWorks == PasswordVerificationResult.Success)
                    {
                        await _signInManager.SignInAsync(resultUser, model.RememberMe);

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
                var user = new IdentityUser
                {
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
                        FirstName = model.FirstName, // Assuming FirstName and LastName are available in model
                        LastName = model.LastName,
                        IsAdmin = false
                        
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


        [HttpGet]
        [Route("Contas")]
        public IActionResult GetContas(int userFK)
        {
            try
            {
                var contas = _context.Contas.Where(c => c.UserFK == userFK).ToList();
                return Ok(contas);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao buscar as contas: {ex.Message}");
                return StatusCode(500, "Erro interno ao buscar as contas. Por favor, tente novamente.");
            }
        }


        [HttpPut]
        [Route("Contas/{contaId}")]
        public IActionResult UpdateConta(int contaId, [FromBody] ContaModel model)
        {
            try
            {
                var conta = _context.Contas.FirstOrDefault(c => c.ContaID == contaId);
                if (conta == null)
                {
                    return NotFound("Conta não encontrada.");
                }

                conta.NomeConta = model.NomeConta;
                conta.Saldo = model.Saldo;

                _context.SaveChanges();

                return Ok(new { message = "Conta atualizada com sucesso." });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao atualizar a conta: {ex.Message}");
                return StatusCode(500, new { message = "Falha ao atualizar a conta. Por favor, tente novamente." });
            }
        }

        [HttpDelete]
        [Route("Contas/{contaId}")]
        public IActionResult DeleteConta(int contaId)
        {
            try
            {
                var conta = _context.Contas.FirstOrDefault(c => c.ContaID == contaId);
                if (conta == null)
                {
                    return NotFound("Conta não encontrada.");
                }

                _context.Contas.Remove(conta);
                _context.SaveChanges();

                return Ok(new { message = "Conta excluída com sucesso." });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao excluir a conta: {ex.Message}");
                return StatusCode(500, new { message = "Falha ao excluir a conta. Por favor, tente novamente." });
            }
        }

        [HttpPost]
        [Route("Contas")]
        public async Task<IActionResult> AdicionarConta([FromBody] ContaModel model, [FromQuery] string OldEmail)
        {
            try
            {
                // Obtém o usuário através do email
                var resultUser = await _userManager.FindByEmailAsync(OldEmail);
                if (resultUser == null)
                {
                    return NotFound("User not found.");
                }

                // Obtém o usuário no contexto da aplicação
                var user = _context.Utilizadores.FirstOrDefault(u => u.UserAutent == resultUser.Id);
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                // Usa o UserID do usuário autenticado
                var novaConta = new TblContas
                {
                    NomeConta = model.NomeConta,
                    Saldo = model.Saldo,
                    UserFK = user.UserID
                };

                // Adiciona a nova conta ao contexto e salva as alterações
                _context.Contas.Add(novaConta);
                await _context.SaveChangesAsync();

                // Retorna os detalhes da conta adicionada
                return Ok(new
                {
                    contaId = novaConta.ContaID,
                    nomeConta = novaConta.NomeConta,
                    saldo = novaConta.Saldo,
                    userFK = novaConta.UserFK,
                    message = "Conta adicionada com sucesso."
                });
            }
            catch (Exception ex)
            {
                // Loga o erro ocorrido
                _logger.LogError($"An error occurred while adding account: {ex.Message}");

                // Retorna um erro interno do servidor com uma mensagem apropriada
                return StatusCode(500, new { message = "Failed to add account. Please try again." });
            }
        }




        [HttpPut]
        [Route("Utilizadores")]
        public async Task<IActionResult> UpdateUtilizador([FromBody] TblUtilizadores model, [FromQuery] string UserFK, [FromQuery] string oldEmail)
        {
            try
            {
                var resultUser = await _userManager.FindByEmailAsync(oldEmail);

                if (resultUser == null)
                {
                    return NotFound("User not found.");
                }

                var user = _context.Utilizadores.FirstOrDefault(u => u.UserAutent == resultUser.Id);

                if (user == null)
                {
                    return NotFound("User not found.");
                }


                user.UserName = model.UserName;
                user.Email = model.Email;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.UserAutent = resultUser.Id;

                _context.Utilizadores.Update(user);
                await _context.SaveChangesAsync();

                return Ok(new { message = "User details updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to update user details. Please try again." });
            }
        }


    }
}
