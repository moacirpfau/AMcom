using Dapper;
using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Domain;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database;
using System.Data;

public class MovimentarContaCommandHandler : IRequestHandler<MovimentarContaCommand, MovimentarContaResponse>
{
    private readonly ICommandStore _commandStore;
    private readonly IIdempotencyStore _idempotencyStore;
    private readonly IDbConnection _dbConnection;

    public MovimentarContaCommandHandler(ICommandStore commandStore, IIdempotencyStore idempotencyStore, IDbConnection dbConnection)
    {
        _commandStore = commandStore;
        _idempotencyStore = idempotencyStore;
        _dbConnection = dbConnection;
    }

    public async Task<MovimentarContaResponse> Handle(MovimentarContaCommand request, CancellationToken cancellationToken)
    {
        const int maxAttempts = 100;
        const int delayBetweenAttempts = 1000; // Em milissegundos

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            try
            {
                // 1. Verificar se já existe uma requisição processada com essa chave de idempotência
                var existingResult = await _idempotencyStore.GetResultByIdempotencyKeyAsync(request.IdempotencyKey);
                if (existingResult != null)
                {
                    return existingResult; // Retorna o resultado já existente
                }

                
                var contaCorrente = await _dbConnection.QueryFirstOrDefaultAsync(
                    "SELECT * FROM contacorrente WHERE idcontacorrente = @IdContaCorrente",
                    new { IdContaCorrente = request.IdContaCorrente });
                // 2. Verificar se a conta corrente existe 
                if (contaCorrente == null)
                {
                    throw new BusinessException("A conta corrente não está cadastrada.", "INVALID_ACCOUNT");
                }
                // 3. Verificar se a conta corrente está ativa
                if (contaCorrente.Ativo == 0)
                {
                    throw new BusinessException("A conta corrente está inativa.", "INACTIVE_ACCOUNT");
                }
                // 4. Verificar se o valor é maior que zero
                if (request.Valor <= 0)
                {
                    throw new BusinessException("O valor da movimentação deve ser positivo.", "INVALID_VALUE");
                }
                // 5. Verificar se o TipoMovimento esta correto
                if (request.TipoMovimento != "C" && request.TipoMovimento != "D")
                {
                    throw new BusinessException("O tipo de movimentação deve ser 'C' (crédito) ou 'D' (débito).", "INVALID_TYPE");
                }

                // Criar um objeto Movimento
                var movimento = new Movimento
                {
                    IdMovimento = Guid.NewGuid().ToString(),
                    IdContaCorrente = request.IdContaCorrente,
                    DataMovimento = DateTime.Now.ToString("yyyy-MM-dd"),
                    TipoMovimento = request.TipoMovimento,
                    Valor = request.Valor
                };

                // Salvar o movimento
                await _commandStore.SalvarMovimentoAsync(movimento);

                // Salvar o resultado no store de idempotência
                var response = new MovimentarContaResponse
                {
                    IdMovimento = movimento.IdMovimento
                };
                await _idempotencyStore.SaveResultAsync(request.IdempotencyKey, response);

                return response; // Retorna o resultado gerado
            }
            catch (Exception ex)
            {
                if (attempt == maxAttempts - 1)
                {
                    throw new Exception("Não foi possível processar a requisição após várias tentativas.", ex);
                }
                await Task.Delay(delayBetweenAttempts, cancellationToken);
            }
        }

        throw new Exception("Não foi possível processar a requisição.");
    }

}
