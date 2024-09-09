using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Queries.Requests;
using Questao5.Domain;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("movimentar")]
        public async Task<IActionResult> MovimentarConta([FromBody] MovimentarContaCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { error = ex.Message, type = ex.ErrorType });
            }
        }

        [HttpGet("saldo/{idContaCorrente}")]
        public async Task<IActionResult> ConsultarSaldo(string idContaCorrente)
        {
            try
            {
                var query = new ConsultarSaldoQuery { IdContaCorrente = idContaCorrente };
                var response = await _mediator.Send(query);
                return Ok(response);
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }
    }

}
