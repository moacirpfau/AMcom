// Interface
using Dapper;
using Questao5.Domain.Entities;
using System.Data;

public interface ICommandStore
{
    Task SalvarMovimentoAsync(Movimento movimento);
}

// Implementação
public class CommandStore : ICommandStore
{
    private readonly IDbConnection _dbConnection;

    public CommandStore(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task SalvarMovimentoAsync(Movimento movimento)
    {
        var query = @"INSERT INTO movimento (datamovimento, valor, idmovimento, tipomovimento, idcontacorrente) VALUES (@DataMovimento, @Valor, @IdMovimento, @TipoMovimento, @IdContaCorrente);";
        await _dbConnection.ExecuteAsync(query, movimento);
    }
}

