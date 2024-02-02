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
        public ActionResult AdicionarRegistro(DateTime data)
        {
            // Encontre o ponto correspondente à data informada ou crie um novo ponto
            Models.Ponto ponto = Funcionario.ListaDePontos.FirstOrDefault(p => p.DataRegistro.Date == data.Date);

            if (ponto == null)
            {
                ponto = new Models.Ponto { DataRegistro = data, Registros = new List<Registro>() };
                Funcionario.ListaDePontos.Add(ponto);
            }

            // Verifique se já existe um registro para a data informada
            Registro registroExistente = ponto.Registros.FirstOrDefault(r => r.Entrada.Date == data.Date);

            if (registroExistente != null)
            {
                // Já existe um registro para a data informada, atualize a saída
                registroExistente.Saida = DateTime.Now; // Você pode ajustar isso conforme necessário
            }
            else
            {
                // Não existe um registro para a data informada, crie um novo
                Registro novoRegistro = new Registro();

                // Determine se é uma entrada ou saída com base no horário e tempo do ponto
                if (DateTime.Now.TimeOfDay < Funcionario.PerfilDeTrabalho.InicioIntervalo.TimeOfDay)
                {
                    // Antes do início do intervalo, considera como entrada
                    novoRegistro.Entrada = DateTime.Now;
                    novoRegistro.Tipo = TipoDeRegistro.Normal;
                }
                else if (DateTime.Now.TimeOfDay > Funcionario.PerfilDeTrabalho.HoraFim.TimeOfDay)
                {
                    // Após o término do expediente, considera como saída
                    novoRegistro.Saida = DateTime.Now;
                    novoRegistro.Tipo = TipoDeRegistro.Noturno;
                }
                else
                {
                    // Durante o expediente, considera como novo (sem entrada ou saída específica)
                    novoRegistro.Tipo = TipoDeRegistro.HoraExtra;
                }

                // Adicione o novo registro ao ponto
                ponto.Registros.Add(novoRegistro);
            }

            // Salve as alterações no banco de dados ou na fonte de dados
            // SalvarAlteracoesNoBancoDeDados(funcionario);

            // Redirecione de volta à página de detalhes do funcionário
            return RedirectToAction("Detalhes", new { id = Funcionario.Id });
        }
    }
}
