using MediatR;
using Questao5.Application.Commands.Responses;

namespace Questao5.Application.Commands.Requests
{
    public class MovimentarContaCommand : IRequest<MovimentarContaResponse>
    {
        public string? IdContaCorrente { get; set; }
        public decimal Valor { get; set; }
        public string? TipoMovimento { get; set; } // 'C' = Crédito, 'D' = Débito
        public string? IdempotencyKey { get; set; }
    }


}
