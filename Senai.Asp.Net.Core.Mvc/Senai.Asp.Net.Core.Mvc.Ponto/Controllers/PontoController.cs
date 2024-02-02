using Microsoft.AspNetCore.Mvc;
using Senai.Asp.Net.Core.Mvc.Ponto.Models;

namespace Senai.Asp.Net.Core.Mvc.Ponto.Controllers
{
    public class PontoController : Controller
    {
        public static Funcionario Funcionario { get; set; }
        public PontoController()
        {
            if (Funcionario == null)
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
        [HttpPost]
        public ActionResult AdicionarRegistro(DateTime entrada, DateTime saida,TipoDeRegistro tipo)
        {
            // Encontre o ponto correspondente à data atual ou crie um novo ponto
            Models.Ponto ponto = Funcionario.ListaDePontos.FirstOrDefault(p => p.DataRegistro.Date.DayOfYear == DateTime.Now.DayOfYear);

            if (ponto == null)
            {
                ponto = new Models.Ponto { Registros = new List<Registro>() };
                Funcionario.ListaDePontos.Add(ponto);
            }

            // Crie um novo registro
            Registro novoRegistro = new Registro
            {
                Entrada = entrada,
                Saida = saida,
                Tipo = tipo
            };

            // Adicione o registro ao ponto
            ponto.Registros.Add(novoRegistro);

            // Salve as alterações no banco de dados ou na fonte de dados
            // SalvarAlteracoesNoBancoDeDados(funcionario);

            // Redirecione de volta à página de detalhes do funcionário


            // Se algo der errado, redirecione para a página inicial ou trate de acordo com sua lógica
            return RedirectToAction("Index");
        }
    }
}
