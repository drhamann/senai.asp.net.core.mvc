using Microsoft.AspNetCore.Mvc;
using Senai.Asp.Net.Core.Mvc.Ponto.Models;

namespace Senai.Asp.Net.Core.Mvc.Ponto.Controllers
{
    public class PontoController : Controller
    {
        public static Funcionario Funcionario { get; set; }
        public PontoController() 
        {
            if(Funcionario == null)
            {
                Funcionario = new Funcionario
                {
                    Id = 1,
                    Nome = "João da Silva",
                    Idade = 30,
                    CPF = "123.456.789-01",
                    RG = "987654321",
                    Telefone = "(11) 1234-5678",
                    Email = "joao.silva@example.com"
                };
            }
        }
        public IActionResult Index()
        {
            return View(Funcionario);
        }
    }
}
