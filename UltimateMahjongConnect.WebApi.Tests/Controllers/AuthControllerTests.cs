using Microsoft.AspNetCore.Mvc;
using Moq;
using Ultimate_Mahjong_Connect.Controllers.V1;
using UltimateMahjongConnect.Application.DTO;
using UltimateMahjongConnect.Application.Services;
using FluentAssertions;
using Xunit;

namespace UltimateMahjongConnect.WebApi.Tests.Controllers;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _mockAuthService;
    private readonly Mock<IAnonymousSessionService> _mockAnonymousSessionService;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _mockAuthService = new Mock<IAuthService>();
        _mockAnonymousSessionService = new Mock<IAnonymousSessionService>();
        _controller = new AuthController(_mockAuthService.Object, _mockAnonymousSessionService.Object);
    }

    [Fact]
    public async Task CreateAnonymousSession_ShouldReturnOkResult_WithSessionData()
    {
        // Arrange
        var expectedSession = new AnonymousSessionDTO
        {
            SessionId = "test-session-123",
            AnonymousId = "anonymous-123",
            Token = "test-jwt-token",
            CreatedAt = DateTime.UtcNow,
            LastActivity = DateTime.UtcNow,
            IsActive = true
        };

        _mockAnonymousSessionService.Setup(s => s.CreateAnonymousSessionAsync())
            .ReturnsAsync(expectedSession);

        // Act
        var result = await _controller.CreateAnonymousSession();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        
        var response = okResult!.Value;
        response.Should().NotBeNull();
        
        _mockAnonymousSessionService.Verify(s => s.CreateAnonymousSessionAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateAnonymousSession_WhenServiceThrows_ShouldReturnBadRequest()
    {
        // Arrange
        var expectedException = new Exception("Session creation failed");
        _mockAnonymousSessionService.Setup(s => s.CreateAnonymousSessionAsync())
            .ThrowsAsync(expectedException);

        // Act
        var result = await _controller.CreateAnonymousSession();

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        var response = badRequestResult!.Value;
        response.Should().NotBeNull();
    }

    [Fact]
    public async Task ValidateAnonymousSession_WithValidSession_ShouldReturnTrue()
    {
        // Arrange
        var sessionId = "valid-session-123";
        _mockAnonymousSessionService.Setup(s => s.ValidateAnonymousSessionAsync(sessionId))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.ValidateAnonymousSession(sessionId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        _mockAnonymousSessionService.Verify(s => s.ValidateAnonymousSessionAsync(sessionId), Times.Once);
    }

    [Fact]
    public async Task ValidateAnonymousSession_WithInvalidSession_ShouldReturnFalse()
    {
        // Arrange
        var sessionId = "invalid-session-123";
        _mockAnonymousSessionService.Setup(s => s.ValidateAnonymousSessionAsync(sessionId))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.ValidateAnonymousSession(sessionId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        var response = okResult!.Value;
        response.Should().NotBeNull();
    }

    [Fact]
    public async Task GetAnonymousSession_WithValidSessionId_ShouldReturnSession()
    {
        // Arrange
        var sessionId = "test-session-123";
        var expectedSession = new AnonymousSessionDTO
        {
            SessionId = sessionId,
            AnonymousId = "anonymous-123",
            Token = "test-token",
            CreatedAt = DateTime.UtcNow,
            LastActivity = DateTime.UtcNow,
            IsActive = true
        };

        _mockAnonymousSessionService.Setup(s => s.GetAnonymousSessionAsync(sessionId))
            .ReturnsAsync(expectedSession);

        // Act
        var result = await _controller.GetAnonymousSession(sessionId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        _mockAnonymousSessionService.Verify(s => s.GetAnonymousSessionAsync(sessionId), Times.Once);
    }

    [Fact]
    public async Task GetAnonymousSession_WithInvalidSessionId_ShouldReturnNotFound()
    {
        // Arrange
        var sessionId = "invalid-session";
        _mockAnonymousSessionService.Setup(s => s.GetAnonymousSessionAsync(sessionId))
            .ReturnsAsync((AnonymousSessionDTO?)null);

        // Act
        var result = await _controller.GetAnonymousSession(sessionId);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task DeleteAnonymousSession_ShouldReturnOkResult()
    {
        // Arrange
        var sessionId = "test-session-123";
        _mockAnonymousSessionService.Setup(s => s.DeleteAnonymousSessionAsync(sessionId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteAnonymousSession(sessionId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        _mockAnonymousSessionService.Verify(s => s.DeleteAnonymousSessionAsync(sessionId), Times.Once);
    }

    [Fact]
    public async Task Register_WithValidRequest_ShouldReturnOkResult()
    {
        // Arrange
        var registerRequest = new RegisterRequestDTO
        {
            Email = "test@example.com",
            Pseudonyme = "TestPlayer",
            Password = "SecurePassword123",
            Age = 25
        };

        var authResult = new AuthResultDTO
        {
            IsSuccess = true,
            Data = new AuthResponseDTO
            {
                Token = "test-token",
                Pseudonyme = "TestPlayer",
                Email = "test@example.com", 
                Score = 0 
            }
        };

        _mockAuthService.Setup(s => s.RegisterAsync(registerRequest))
            .ReturnsAsync(authResult);

        // Act
        var result = await _controller.Register(registerRequest);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        _mockAuthService.Verify(s => s.RegisterAsync(registerRequest), Times.Once);
    }

    [Fact]
    public async Task Register_WithInvalidRequest_ShouldReturnBadRequest()
    {
        // Arrange
        var registerRequest = new RegisterRequestDTO
        {
            Email = "invalid-email",
            Pseudonyme = "Te",
            Password = "123",
            Age = 10
        };

        var authResult = new AuthResultDTO
        {
            IsSuccess = false,
            ErrorMessage = "Validation failed"
        };

        _mockAuthService.Setup(s => s.RegisterAsync(registerRequest))
            .ReturnsAsync(authResult);

        // Act
        var result = await _controller.Register(registerRequest);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task Login_WithValidCredentials_ShouldReturnOkResult()
    {
        // Arrange
        var loginRequest = new LoginRequestDTO
        {
            UsernameOrEmail = "testplayer@example.com",
            Password = "SecurePassword123"
        };

        var authResult = new AuthResultDTO
        {
            IsSuccess = true,
            Data = new AuthResponseDTO
            {
                Token = "test-token", 
                Pseudonyme = "TestPlayer", 
                Email = "testplayer@example.com",
                Score = 100 
            }
        };

        _mockAuthService.Setup(s => s.LoginAsync(loginRequest))
            .ReturnsAsync(authResult);

        // Act
        var result = await _controller.Login(loginRequest);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        _mockAuthService.Verify(s => s.LoginAsync(loginRequest), Times.Once);
    }

    [Fact]
    public async Task Login_WithInvalidCredentials_ShouldReturnUnauthorized()
    {
        // Arrange
        var loginRequest = new LoginRequestDTO
        {
            UsernameOrEmail = "wrong@example.com", // ✅ Nom de propriété corrigé
            Password = "WrongPassword"
        };

        var authResult = new AuthResultDTO
        {
            IsSuccess = false,
            ErrorMessage = "Invalid credentials"
        };

        _mockAuthService.Setup(s => s.LoginAsync(loginRequest))
            .ReturnsAsync(authResult);

        // Act
        var result = await _controller.Login(loginRequest);

        // Assert
        result.Should().BeOfType<UnauthorizedObjectResult>();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task ValidateAnonymousSession_WithInvalidSessionId_ShouldHandleGracefully(string? sessionId)
    {
        // Arrange
        _mockAnonymousSessionService.Setup(s => s.ValidateAnonymousSessionAsync(It.IsAny<string>()))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.ValidateAnonymousSession(sessionId!);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }
}
