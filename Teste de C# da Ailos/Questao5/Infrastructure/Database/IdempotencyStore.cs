using Dapper;
using Newtonsoft.Json;
using Questao5.Application.Commands.Responses;
using System.Data;

namespace Questao5.Infrastructure.Database
{
    public class IdempotencyStore : IIdempotencyStore
    {
        private readonly IDbConnection _dbConnection;

        public IdempotencyStore(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<MovimentarContaResponse?> GetResultByIdempotencyKeyAsync(string idempotencyKey)
        {
            var query = "SELECT resultado FROM idempotencia WHERE chave_idempotencia = @IdempotencyKey";
            var result = await _dbConnection.QueryFirstOrDefaultAsync<string>(query, new { IdempotencyKey = idempotencyKey });

            if (result != null)
            {
                return JsonConvert.DeserializeObject<MovimentarContaResponse>(result);
            }

            return null;
        }

        public async Task SaveResultAsync(string idempotencyKey, MovimentarContaResponse result)
        {
            var query = @"INSERT INTO idempotencia (chave_idempotencia, resultado) 
                      VALUES (@IdempotencyKey, @Result)";
            var resultJson = JsonConvert.SerializeObject(result);

            await _dbConnection.ExecuteAsync(query, new { IdempotencyKey = idempotencyKey, Result = resultJson });
        }
    }


    public interface IIdempotencyStore
    {
        Task<MovimentarContaResponse?> GetResultByIdempotencyKeyAsync(string idempotencyKey);
        Task SaveResultAsync(string idempotencyKey, MovimentarContaResponse result);
    }


}
