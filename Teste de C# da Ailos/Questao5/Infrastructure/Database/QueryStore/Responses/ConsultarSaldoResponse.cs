namespace Questao5.Infrastructure.Database.QueryStore.Responses
{
    public class ConsultarSaldoResponse
    {
        // O saldo atual da conta corrente
        public decimal Saldo { get; set; }

        // O número ou identificador da conta corrente
        public string? IdContaCorrente { get; set; }

        // A data da última movimentação na conta, se for relevante
        public DateTime DataUltimaMovimentacao { get; set; }

        // O nome do titular da conta, se necessário
        public string? NomeTitular { get; set; }
    }
}
