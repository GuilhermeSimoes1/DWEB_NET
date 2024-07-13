using DWEB_NET.Data;
using DWEB_NET.Models;
using DWEB_NET.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
﻿using Azure.Core;
using DWEB_NET.Data;
using DWEB_NET.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Policy;
using System.Text.Encodings.Web;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;


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

                if (user.IsAdmin)
                {

                    var orcamentosAdmin = _context.Orcamentos.Select(o => new
                    {
                        o.OrcamentoID,
                        o.NomeOrcamento,
                        o.ValorNecessario,
                        o.DataInicial,
                        o.DataFinal,
                        o.ValorAtual
                    }).ToList();
                    return Ok(orcamentosAdmin);
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
     


        private readonly Dictionary<TblTransacoes.Tipo, List<int>> categoriasPorTipo = new Dictionary<TblTransacoes.Tipo, List<int>>
    {
        { TblTransacoes.Tipo.Ganho, new List<int> { 6, 7, 8 } },   // IDs das categorias para transações de Ganho
        { TblTransacoes.Tipo.Gasto, new List<int> { 1, 2, 3, 4, 5, 6 } }  // IDs das categorias para transações de Gasto
    };

        private readonly Dictionary<int, string> categoriaNomes = new Dictionary<int, string>
    {
        { 1, "Saúde" },
        { 2, "Lazer" },
        { 3, "Casa" },
        { 4, "Educação" },
        { 5, "Alimentação" },
        { 6, "Outros" },
        { 7, "Salário" },
        { 8, "Investimentos" }
    };


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromQuery] string Email, [FromQuery] string Password, [FromQuery] bool rememberMe)
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                return BadRequest("Invalid login request.");
            }

            try
            {
                var resultUser = await _userManager.FindByEmailAsync(Email);

                if (resultUser != null)
                {
                    var passWorks = new PasswordHasher<IdentityUser>().VerifyHashedPassword(resultUser, resultUser.PasswordHash, Password);

                    if (passWorks == PasswordVerificationResult.Success)
                    {
                        await _signInManager.SignInAsync(resultUser, rememberMe); // 'rememberMe' determines if the user should be remembered for 14 days.

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
                return BadRequest(ex.Message); // Return exception message
            }
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] TblUtilizadores user)
        {
            if (user == null)
            {
                return BadRequest("Tente Novamente");
            }

            // Cria um novo IdentityUser com os dados do TblUtilizadores
            var userIdentity = new IdentityUser
            {
                UserName = user.Email,
                Email = user.Email,
                EmailConfirmed = true
            };

            // Cria um novo IdentityUser com um Id gerado
            IdentityUser newUser = new IdentityUser
            {
                UserName = user.Email,
                Email = user.Email,
                Id = Guid.NewGuid().ToString(),
                EmailConfirmed = true
            };

            // Cria o IdentityUser com a senha fornecida
            var result = await _userManager.CreateAsync(newUser, user.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                // Define o UserAutent como o Id do novo usuário IdentityUser
                var userId = newUser.Id;

                var utilizador = new TblUtilizadores
                {
                    UserAutent = userId,
                    UserName = user.UserName,
                    Email = user.Email,
                    Password = newUser.PasswordHash,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Descricao = user.Descricao,
                    IsAdmin = false
                };

                // Adiciona o novo TblUtilizadores ao contexto e salva
                _context.Utilizadores.Add(utilizador);
                await _context.SaveChangesAsync();

                // Cria a conta principal associada ao novo usuário TblUtilizadores
                var contaPrincipal = new TblContas
                {
                    NomeConta = "Conta Principal",
                    Saldo = 0,
                    UserFK = utilizador.UserID
                };

                _context.Contas.Add(contaPrincipal);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Registration successful." });
            }

            // Se houver erros ao criar o IdentityUser, adiciona-os ao ModelState
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            _logger.LogError("ModelState is invalid: " + string.Join("; ", errors));

            return BadRequest(ModelState);
        }



        [HttpGet("GetCategorias")]
        public IActionResult GetCategorias()
        {
            var result = new
            {
                categoriasPorTipo,
                categoriaNomes
            };
            return Ok(result);
        }


        [HttpPost("CreateTransacao")]
        public async Task<IActionResult> CreateTransacao([FromBody] TransacoesDTO transacao)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Adiciona a transação ao contexto da base de dados
                TblTransacoes newTransacao = new TblTransacoes
                {
                    UserFK = transacao.UserFK,
                    ContaFK = transacao.ContaFK,
                    DataTime = DateTime.Now, // Usar a data atual para a transação
                    ValorTransacao = transacao.ValorTransacao,
                    Descricao = transacao.Descricao,
                    TipoTransacao = (TblTransacoes.Tipo)transacao.TipoTransacao
                };

                _context.Transacoes.Add(newTransacao);
                await _context.SaveChangesAsync(); 

                // Associa categorias à transação
                foreach (var categoria in transacao.Categorias)
                {
                    TblTransacoesCategorias transacaoCategoria = new TblTransacoesCategorias
                    {
                        TransacaoFK = newTransacao.TransacaoID, // Usar o ID da transação recém-criada
                        CategoriaFK = categoria.CategoriaFK,
                        Valor = categoria.Valor
                    };
                    _context.TransacoesCategorias.Add(transacaoCategoria);
                }

                // Atualiza o saldo da conta
                var conta = await _context.Contas.FindAsync(transacao.ContaFK);
                if (conta != null)
                {
                    if (newTransacao.TipoTransacao == TblTransacoes.Tipo.Ganho)
                    {
                        conta.Saldo += transacao.ValorTransacao;
                    }
                    else if (newTransacao.TipoTransacao == TblTransacoes.Tipo.Gasto)
                    {
                        conta.Saldo -= transacao.ValorTransacao;
                    }
                    _context.Contas.Update(conta);
                }

                // Salva as alterações na base de dados
                await _context.SaveChangesAsync();

                return Ok(new { message = "Transação criada com sucesso!", saldoConta = conta?.Saldo });
            }
            catch (DbUpdateException ex)
            {
                // Em caso de erro ao salvar no banco de dados
                return StatusCode(500, new { message = "Erro ao salvar transação no banco de dados." });
            }
        }




        [HttpGet]
        [Route("GetUserAccounts")]
        public async Task<IActionResult> GetUserAccounts([FromQuery] int userId)
        {
            var user = await _context.Utilizadores.FirstOrDefaultAsync(u => u.UserID == userId);

            if (user == null)
            {
                return NotFound("User not found.");
            }



            var accounts = await _context.Contas
                .Where(c => c.UserFK == user.UserID)
                .Select(c => new
                {
                    c.ContaID,
                    c.NomeConta,
                    c.Saldo
                }).ToListAsync();

            return Ok(new { accounts });
        }


        [HttpGet("GetHistoricoTransacoes")]
        public async Task<IActionResult> GetHistoricoTransacoes([FromQuery] int userId)
        {

            var user = await _context.Utilizadores.FirstOrDefaultAsync(u => u.UserID == userId);


            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (user.IsAdmin)
            {


                var transacoesAdmin = await _context.Transacoes.Select(t => new
                {
                    t.TransacaoID,
                    t.ValorTransacao,
                    t.TipoTransacao,
                    t.Descricao,
                    t.DataTime,
                    Conta = t.Conta.NomeConta,
                    Email = t.User.Email
                }).ToListAsync();

                if (!transacoesAdmin.Any())
                {
                    return NotFound("No transactions found for the given user ID");
                }

                return Ok(transacoesAdmin);
            }

            var transacoes = await _context.Transacoes
                    .Where(t => t.UserFK == userId)
                    .Select(t => new
                    {
                        t.TransacaoID,
                        t.ValorTransacao,
                        t.TipoTransacao,
                        t.Descricao,
                        t.DataTime,
                        Conta = t.Conta.NomeConta,
                        Email = t.User.Email
                    }).ToListAsync();

            if (!transacoes.Any())
            {
                return NotFound("No transactions found for the given user ID");
            }

            return Ok(transacoes);
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

       
        [HttpGet]
        [Route("Contas")]
        public IActionResult GetContas(int userFK)
        {
            try
            {
                if (_context.Utilizadores.Any(u => u.UserID == userFK && u.IsAdmin))
                {
                    var contasAdmin = _context.Contas.Select(c => new
                    {
                        c.ContaID,
                        c.NomeConta,
                        c.Saldo,
                        c.UserFK
                    }).ToList();
                    return Ok(contasAdmin);
                }
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

                resultUser.UserName = user.UserName;
                resultUser.Email = user.Email;
                resultUser.NormalizedEmail = user.Email.ToUpper();
                resultUser.NormalizedUserName = user.UserName.ToUpper();

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


