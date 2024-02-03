namespace Senai.Asp.Net.Core.Mvc.Ponto.Models
{
    public class Ponto
    {
        public Ponto()
        {
            DataRegistro = DateTime.Now;
            Registros = new List<Registro>();
        }
        public Ponto(DateTime dateTime)
        {
            DataRegistro = dateTime;
            Registros = new List<Registro>();
        }
        public DateTime DataRegistro { get; set; }
        public List<Registro> Registros { get; set; }
    }
    public class Registro
    {
        public DateTime Entrada { get; set; }
        public DateTime Saida { get; set; }
        public TimeSpan Tempo
        {
            get
            {
                if(Saida == null)
                {
                    return TimeSpan.Zero;
                }
                return Saida - Entrada;
            }
        }
        public TipoDeRegistro Tipo { get; set; }
    }
    public enum TipoDeRegistro
    {
        Normal = 0,
        Noturno = 1,
        Intervalo = 2,
        Desconhecido = 3,
        HoraExtra = 4,
        Diurno = 5
    }
    public class Funcionario
    {
        public Funcionario()
        {
            PerfilDeTrabalho = PerfilDeTrabalho.PerfilDeTrabalhoPadrao();
            ListaDePontos = new List<Ponto>()
            {
                new Ponto()
                {
                    Registros = new List<Registro>()
                    {
                        new Registro()
                        {
                            Entrada = DateTime.Now,
                            Saida = DateTime.Now.AddHours(1),
                            Tipo = TipoDeRegistro.Normal
                        }
                    }
                }
            };
        }
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Idade { get; set; }
        public string CPF { get; set; }
        public string RG { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public PerfilDeTrabalho PerfilDeTrabalho { get; set; }
        public List<Ponto> ListaDePontos { get; set; }
    }
    public class PerfilDeTrabalho
    {
        private static PerfilDeTrabalho Padrao { get; set; }

        public static PerfilDeTrabalho PerfilDeTrabalhoPadrao()
        {
            if (Padrao == null)
            {
                Padrao = new PerfilDeTrabalho()
                {
                    Fixo = true,
                    HoraFim = new DateTime(2024, 01, 01, 18, 00, 00),
                    HoraInicio = new DateTime(2024, 01, 01, 08, 00, 00),
                    InicioIntervalo = new DateTime(2024, 01, 01, 12, 00, 00),
                    TempoDeIntervalo = TimeSpan.FromHours(2),
                    Tolerancia = TimeSpan.FromMinutes(10)
                };
            }
            return Padrao;
        }

        public bool Fixo { get; set; }
        public DateTime HoraInicio { get; set; }
        public DateTime HoraFim { get; set; }
        public DateTime InicioIntervalo { get; set; }
        public TimeSpan TempoDeIntervalo { get; set; }
        public TimeSpan Tolerancia { get; set; }
    }
}
