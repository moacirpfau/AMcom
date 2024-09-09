namespace Questao5.Application.Queries.Responses
{
    public class SaldoResponse
    {
        public string? NumeroContaCorrente { get; set; }
        public string? NomeTitular { get; set; }
        public DateTime DataHoraConsulta { get; set; }
        public double SaldoAtual { get; set; }
    }

}
