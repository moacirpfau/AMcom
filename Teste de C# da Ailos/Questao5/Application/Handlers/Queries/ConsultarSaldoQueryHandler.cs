using Dapper;
using MediatR;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Responses;
using System.Data;

namespace Questao5.Application.Handlers.Queries
{
    public class ConsultarSaldoQueryHandler : IRequestHandler<ConsultarSaldoQuery, SaldoResponse>
    {
        private readonly IDbConnection _dbConnection;

        public ConsultarSaldoQueryHandler(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<SaldoResponse> Handle(ConsultarSaldoQuery request, CancellationToken cancellationToken)
        {
            var contaCorrente = await _dbConnection.QueryFirstOrDefaultAsync(
                "SELECT * FROM contacorrente WHERE idcontacorrente = @IdContaCorrente",
                new { IdContaCorrente = request.IdContaCorrente });

            if (contaCorrente == null)
            {
                throw new BusinessException("A conta corrente não está cadastrada.", "INVALID_ACCOUNT");
            }
            if (contaCorrente.Ativo == 0)
            {
                throw new BusinessException("A conta corrente está inativa.", "INACTIVE_ACCOUNT");
            }

            var query = @"
            SELECT
                c.numero AS NumeroConta,
                c.nome AS NomeConta,
                (SELECT IFNULL(SUM(valor), 0) FROM movimento WHERE idcontacorrente = c.idcontacorrente AND tipomovimento = 'C') AS SomaCreditos,
                (SELECT IFNULL(SUM(valor), 0) FROM movimento WHERE idcontacorrente = c.idcontacorrente AND tipomovimento = 'D') AS SomaDebitos
            FROM
                contacorrente c
            WHERE
                c.idcontacorrente = @IdContaCorrente";

            var result = await _dbConnection.QueryFirstOrDefaultAsync(query, new { IdContaCorrente = request.IdContaCorrente });

            if (result == null)
            {
                // Retornar resposta adequada caso a conta não seja encontrada
                return new SaldoResponse
                {
                    SaldoAtual = 0,
                    NumeroContaCorrente = "0",
                    NomeTitular = "Titular não encontrada"
                };
            }
            var saldo = result.SomaCreditos - result.SomaDebitos;
            return new SaldoResponse
            {
                SaldoAtual = saldo,
                DataHoraConsulta = DateTime.Now,
                NumeroContaCorrente = result.NumeroConta.ToString(),
                NomeTitular = result.NomeConta
            };
        }
    }

}
