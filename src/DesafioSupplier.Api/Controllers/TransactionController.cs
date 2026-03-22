using Microsoft.AspNetCore.Mvc;
using DesafioSupplier.Api.Shared;
using DesafioSupplier.Domain.Entities;
using DesafioSupplier.Api.ModelRequests;
using DesafioSupplier.Api.ModelResponses;
using Microsoft.AspNetCore.Authorization;
using DesafioSupplier.Domain.Interfaces.Services;

namespace DesafioSupplier.Api.Controllers;

[Route("api/transaction")]
[ApiController]
public class TransactionController(ITransactionService transactionService) : ControllerBase
{
    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TransactionModelResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(TransactionDeniedModelResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateTransaction(TransactionModelRequest transactionModelRequest)
    {
        try
        {
            if (string.IsNullOrEmpty(transactionModelRequest.IdCliente))
            {
                return BadRequest(new ErroModelResponse() { DetalheErro = "IdCliente é um campo obrigatório" });
            }

            if (transactionModelRequest.ValorSimulacao <= 0)
            {
                return BadRequest(new ErroModelResponse() { DetalheErro = "ValorSimulacao precisa ser maior que zero" });
            }

            var transaction = new Transaction() { Id = string.Empty, CustomerId = transactionModelRequest.IdCliente, Amount = transactionModelRequest.ValorSimulacao };
            var transactionId = await transactionService.PerformTransactionAsync(transaction);

            return Ok(new TransactionModelResponse() { IdTransacao = transactionId });
        }
        catch (ApplicationException)
        {
            return BadRequest(new TransactionDeniedModelResponse());
        }
    }
}
