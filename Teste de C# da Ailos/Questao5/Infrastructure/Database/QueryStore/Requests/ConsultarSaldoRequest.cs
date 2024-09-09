using MediatR;
using Questao5.Infrastructure.Database.QueryStore.Responses;

namespace Questao5.Infrastructure.Database.QueryStore.Requests
{
    public class ConsultarSaldoRequest : IRequest<ConsultarSaldoResponse>
    {
        public string? IdContaCorrente { get; set; }
    }
}
