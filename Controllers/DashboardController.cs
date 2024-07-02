using DWEB_NET.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DWEB_NET.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            var viewModel = new DashboardViewModel();

            // Simulação de dados iniciais
            viewModel.AccountValue = 1500.0m;
            viewModel.SelectedTransactionType = string.Empty; // Valor inicial vazio
            viewModel.Descricao = string.Empty; // Valor inicial vazio
            viewModel.InputValue = 0;

            // Dados simulados para transações
            viewModel.TransactionHistory = new List<Transaction>
            {
                new Transaction
                {
                    Value = 500.0m,
                    Type = "ganho",
                    Descricao = "Salário de Junho",
                    Categories = new List<string> { "Salário" },
                    CategoryValues = new Dictionary<string, string> { { "Salário", "500.0" } },
                    AccountValue = 2000.0m
                },
                new Transaction
                {
                    Value = 200.0m,
                    Type = "gasto",
                    Descricao = "Compra de alimentos",
                    Categories = new List<string> { "Alimentos" },
                    CategoryValues = new Dictionary<string, string> { { "Alimentos", "200.0" } },
                    AccountValue = 1800.0m
                }
            };

            // Configuração inicial das categorias
            viewModel.GanhoCategories = new List<string> { "Salário", "Presentes", "Investimentos" };
            viewModel.GastoCategories = new List<string> { "Saúde", "Lazer", "Casa", "Educação", "Presentes" };

            // Configuração inicial dos datasets de Doughnut
            viewModel.RendasDataGanho = new DoughnutData
            {
                Labels = viewModel.GanhoCategories,
                Datasets = new List<DoughnutDataset>()
            };

            viewModel.RendasDataGasto = new DoughnutData
            {
                Labels = viewModel.GastoCategories,
                Datasets = new List<DoughnutDataset>()
            };

            return View(viewModel); // Retorna a View Index com o viewModel
        }
    }
}
