namespace Questao5.Infrastructure.Database.CommandStore.Requests
{
    public class SalvarMovimentoRequest
    {
        public string? IdRequisicao { get; set; }
        public string? IdContaCorrente { get; set; }
        public decimal Valor { get; set; }
        public char TipoMovimento { get; set; }
    }

}
