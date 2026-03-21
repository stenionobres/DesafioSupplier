using Microsoft.AspNetCore.Mvc;
using DesafioSupplier.Api.Shared;
using DesafioSupplier.Api.ModelRequests;
using DesafioSupplier.Api.ModelResponses;
using DesafioSupplier.Domain.Interfaces.Services;

namespace DesafioSupplier.Api.Controllers;

[Route("api/transaction")]
[ApiController]
public class TransactionController(ITransactionService transactionService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TransactionModelResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TransactionDeniedModelResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateTransaction(TransactionModelRequest transactionModelRequest)
    {
        if (string.IsNullOrEmpty(transactionModelRequest.IdCliente))
        {
            return BadRequest(new ErroModelResponse() { DetalheErro = "IdCliente é um campo obrigatório" });
        }

        if (transactionModelRequest.ValorSimulacao <= 0)
        {
            return BadRequest(new ErroModelResponse() { DetalheErro = "ValorSimulacao precisa ser maior que zero" });
        }

        var transactionId = await transactionService.PerformTransaction(transactionModelRequest.IdCliente, transactionModelRequest.ValorSimulacao);

        return Ok(new TransactionModelResponse() { IdTransacao = transactionId });
    }
}
