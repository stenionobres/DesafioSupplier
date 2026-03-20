using Microsoft.AspNetCore.Mvc;
using DesafioSupplier.Api.Shared;
using DesafioSupplier.Domain.Entities;
using DesafioSupplier.Api.ModelRequests;
using DesafioSupplier.Api.ModelResponses;
using DesafioSupplier.Domain.Interfaces.Services;

namespace DesafioSupplier.Api.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController(IUserService userService, ISignInService signInService) : ControllerBase
{
    [HttpPost("signup")]
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
    [ProducesResponseType(typeof(SignInModelResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErroModelResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Signin(SignInModelRequest signInModelRequest)
    {
        if (string.IsNullOrEmpty(signInModelRequest.Email))
        {
            return BadRequest(new ErroModelResponse() { DetalheErro = "Email é um campo obrigatório" });
        }

        if (string.IsNullOrEmpty(signInModelRequest.Senha))
        {
            return BadRequest(new ErroModelResponse() { DetalheErro = "Senha é um campo obrigatório" });
        }

        var token = await signInService.SignIn(signInModelRequest.Email, signInModelRequest.Senha);

        return Ok(new SignInModelResponse() { Token = token });
    }
}
