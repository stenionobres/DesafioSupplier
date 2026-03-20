using Microsoft.AspNetCore.Mvc;
using DesafioSupplier.Api.Shared;
using DesafioSupplier.Domain.Entities;
using DesafioSupplier.Api.ModelRequests;
using DesafioSupplier.Api.ModelResponses;
using DesafioSupplier.Domain.Interfaces.Services;

namespace DesafioSupplier.Api.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController(IUserService userService) : ControllerBase
{
    [HttpPost("signup")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(UserModelResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErroModelResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Signup(UserModelRequest userModelRequest)
    {
        if (string.IsNullOrEmpty(userModelRequest.Email))
        {
            return BadRequest(new ErroModelResponse() { DetalheErro = "Email é um campo obrigatório" });
        }

        if (string.IsNullOrEmpty(userModelRequest.Senha))
        {
            return BadRequest(new ErroModelResponse() { DetalheErro = "Senha é um campo obrigatório" });
        }
        
        var user = new User()
        {
            Email = userModelRequest.Email,
            Senha = userModelRequest.Senha
        };

        var userId = await userService.SaveUserAsync(user);

        return Ok(new UserModelResponse() { Id = userId });
    }

    [HttpPost("signin")]
    public void Signin()
    {

    }
}
