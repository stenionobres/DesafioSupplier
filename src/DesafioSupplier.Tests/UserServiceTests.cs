using Moq;
using Xunit;
using DesafioSupplier.Domain.Entities;
using DesafioSupplier.Application.Services;
using DesafioSupplier.Domain.Interfaces.Auth;
using DesafioSupplier.Domain.Interfaces.Repositories;

namespace DesafioSupplier.Tests;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher>();

        _userService = new UserService(
            _userRepositoryMock.Object,
            _passwordHasherMock.Object
        );
    }

    [Fact]
    public async Task SaveUserAsync_ShouldGenerateId_HashPassword_AndSaveUser()
    {
        var user = new User
        {
            Id = string.Empty,
            Email = "test@email.com",
            Password = "123456"
        };

        var hashedPassword = "hashed_password";

        _passwordHasherMock
            .Setup(p => p.HashPasswordAsync(user.Password))
            .ReturnsAsync(hashedPassword);

        _userRepositoryMock
            .Setup(r => r.SaveUserAsync(It.IsAny<User>()))
            .Returns(Task.CompletedTask);

        var result = await _userService.SaveUserAsync(user);

        Assert.False(string.IsNullOrEmpty(result));
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(hashedPassword, user.Password);

        _passwordHasherMock.Verify(p => p.HashPasswordAsync("123456"), Times.Once);

        _userRepositoryMock.Verify(r => r.SaveUserAsync(It.Is<User>(u =>
            u.Id == result &&
            u.Password == hashedPassword &&
            u.Email == user.Email
        )), Times.Once);
    }

    [Fact]
    public async Task GetUserAsync_ShouldReturnUser_WhenUserExists()
    {
        var email = "test@email.com";

        var expectedUser = new User
        {
            Id = Guid.NewGuid().ToString(),
            Email = email,
            Password = "hashed"
        };

        _userRepositoryMock
            .Setup(r => r.GetUserAsync(email))
            .ReturnsAsync(expectedUser);

        var result = await _userService.GetUserAsync(email);

        Assert.NotNull(result);
        Assert.Equal(expectedUser, result);

        _userRepositoryMock.Verify(r => r.GetUserAsync(email), Times.Once);
    }

    [Fact]
    public async Task GetUserAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        var email = "notfound@email.com";

        _userRepositoryMock
            .Setup(r => r.GetUserAsync(email))
            .ReturnsAsync((User)null);

        var result = await _userService.GetUserAsync(email);

        Assert.Null(result);

        _userRepositoryMock.Verify(r => r.GetUserAsync(email), Times.Once);
    }
}
