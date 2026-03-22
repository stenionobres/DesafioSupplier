using Moq;
using Xunit;
using DesafioSupplier.Domain.Entities;
using DesafioSupplier.Application.Services;
using DesafioSupplier.Domain.Interfaces.Auth;
using DesafioSupplier.Domain.Interfaces.Services;

namespace DesafioSupplier.Tests;

public class SignInServiceTests
{
    private readonly Mock<IUserService> _userServiceMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly SignInService _signInService;

    public SignInServiceTests()
    {
        _userServiceMock = new Mock<IUserService>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _tokenServiceMock = new Mock<ITokenService>();

        _signInService = new SignInService(
            _userServiceMock.Object,
            _passwordHasherMock.Object,
            _tokenServiceMock.Object
        );
    }

    [Fact]
    public async Task SignIn_ShouldReturnToken_WhenCredentialsAreValid()
    {
        var email = "test@email.com";
        var password = "123456";

        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            Email = email,
            Password = "hashed_password"
        };

        var expectedToken = "jwt_token";

        _userServiceMock
            .Setup(s => s.GetUserAsync(email))
            .ReturnsAsync(user);

        _passwordHasherMock
            .Setup(p => p.VerifyPasswordAsync(password, user.Password))
            .ReturnsAsync(true);

        _tokenServiceMock
            .Setup(t => t.GetTokenAsync(user))
            .ReturnsAsync(expectedToken);

        var result = await _signInService.SignIn(email, password);

        Assert.Equal(expectedToken, result);

        _userServiceMock.Verify(s => s.GetUserAsync(email), Times.Once);

        _passwordHasherMock.Verify(p =>
            p.VerifyPasswordAsync(password, user.Password), Times.Once);

        _tokenServiceMock.Verify(t =>
            t.GetTokenAsync(user), Times.Once);
    }

    [Fact]
    public async Task SignIn_ShouldThrowException_WhenUserNotFound()
    {
        var email = "notfound@email.com";
        var password = "123456";

        _userServiceMock
            .Setup(s => s.GetUserAsync(email))
            .ReturnsAsync((User)null);

        var exception = await Assert.ThrowsAsync<ApplicationException>(() =>
            _signInService.SignIn(email, password));

        Assert.Equal("Usuário não encontrado", exception.Message);

        _userServiceMock.Verify(s => s.GetUserAsync(email), Times.Once);

        _passwordHasherMock.Verify(p =>
            p.VerifyPasswordAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);

        _tokenServiceMock.Verify(t =>
            t.GetTokenAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task SignIn_ShouldThrowException_WhenPasswordIsInvalid()
    {
        var email = "test@email.com";
        var password = "wrong_password";

        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            Email = email,
            Password = "hashed_password"
        };

        _userServiceMock
            .Setup(s => s.GetUserAsync(email))
            .ReturnsAsync(user);

        _passwordHasherMock
            .Setup(p => p.VerifyPasswordAsync(password, user.Password))
            .ReturnsAsync(false);

        var exception = await Assert.ThrowsAsync<ApplicationException>(() =>
            _signInService.SignIn(email, password));

        Assert.Equal("Senha inválida", exception.Message);

        _userServiceMock.Verify(s => s.GetUserAsync(email), Times.Once);

        _passwordHasherMock.Verify(p =>
            p.VerifyPasswordAsync(password, user.Password), Times.Once);

        _tokenServiceMock.Verify(t =>
            t.GetTokenAsync(It.IsAny<User>()), Times.Never);
    }
}
