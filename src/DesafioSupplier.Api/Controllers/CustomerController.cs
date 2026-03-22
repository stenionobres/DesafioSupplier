using Microsoft.AspNetCore.Mvc;
using DesafioSupplier.Api.Shared;
using DesafioSupplier.Domain.Entities;
using DesafioSupplier.Api.ModelRequests;
using Microsoft.AspNetCore.Authorization;
using DesafioSupplier.Api.ModelResponses;
using DesafioSupplier.Domain.Interfaces.Services;

namespace DesafioSupplier.Api.Controllers;

[Route("api/customer")]
[ApiController]
public class CustomerController(ICustomerService customerService) : ControllerBase
{
    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(CustomerModelResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErroModelResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SaveCustomer(CustomerModelRequest customerModelRequest)
    {
        try
        {
            if (string.IsNullOrEmpty(customerModelRequest.Nome))
            {
                return BadRequest(new ErroModelResponse() { DetalheErro = "Nome é um campo obrigatório" });
            }

            if (string.IsNullOrEmpty(customerModelRequest.Cpf))
            {
                return BadRequest(new ErroModelResponse() { DetalheErro = "Cpf é um campo obrigatório" });
            }

            if (customerModelRequest.ValorLimite < 0)
            {
                return BadRequest(new ErroModelResponse() { DetalheErro = "ValorLimite não pode ser negativo" });
            }

            var customer = new Customer()
            {
                Id = string.Empty,
                Name = customerModelRequest.Nome,
                Cpf = customerModelRequest.Cpf,
                LimitValue = customerModelRequest.ValorLimite
            };
            var customerId = await customerService.SaveCustomerAsync(customer);

            return Ok(new CustomerModelResponse() { IdCliente = customerId });
        }
        catch (ApplicationException ex)
        {
            return BadRequest(new ErroModelResponse() { DetalheErro = ex.Message });
        }
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(GetAllCustomersModelResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllCustomers()
    {
        var customers = await customerService.GetAllCustomersAsync();

        return Ok(new GetAllCustomersModelResponse() { Customers = customers });
    }
}
