using Microsoft.AspNetCore.Mvc;
using Moq;
using Ultimate_Mahjong_Connect.Controllers._1._0;
using UltimateMahjongConnect.Application.DTO;
using UltimateMahjongConnect.Application.Services;
using FluentAssertions;
using Xunit;

namespace UltimateMahjongConnect.WebApi.Tests.Controllers;

public class GamerControllerTests
{
    private readonly Mock<IGamerService> _mockGamerService;
    private readonly GamerController _controller;

    public GamerControllerTests()
    {
        _mockGamerService = new Mock<IGamerService>();
        _controller = new GamerController(_mockGamerService.Object);
    }

    [Fact]
    public async Task GetAllGamers_ShouldReturnOkResult_WithListOfGamers()
    {
        // Arrange
        var expectedGamers = new List<GamerDTO>
        {
            new() { Id = 1, Pseudonyme = "Player1", Email = "player1@test.com", Age = 25, Score = 100 },
            new() { Id = 2, Pseudonyme = "Player2", Email = "player2@test.com", Age = 30, Score = 200 }
        };

        _mockGamerService.Setup(s => s.GetAllGamerAsync())
            .ReturnsAsync(expectedGamers);

        // Act
        var result = await _controller.GetAllGamers();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(expectedGamers);
        
        _mockGamerService.Verify(s => s.GetAllGamerAsync(), Times.Once);
    }

    [Fact]
    public async Task GetGamerByPseudonyme_WithValidPseudonyme_ShouldReturnOkResult()
    {
        // Arrange
        var pseudonyme = "TestPlayer";
        var expectedGamer = new GamerDTO 
        { 
            Id = 1, 
            Pseudonyme = pseudonyme, 
            Email = "test@test.com", 
            Age = 25, 
            Score = 150 
        };

        _mockGamerService.Setup(s => s.GetGamerByPseudonymeAsync(pseudonyme))
            .ReturnsAsync(expectedGamer);

        // Act
        var result = await _controller.GetGamerByPseudonyme(pseudonyme);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(expectedGamer);
    }

    [Fact]
    public async Task GetGamerByPseudonyme_WithInvalidPseudonyme_ShouldReturnNotFound()
    {
        // Arrange
        var pseudonyme = "NonExistentPlayer";
        _mockGamerService.Setup(s => s.GetGamerByPseudonymeAsync(pseudonyme))
            .ReturnsAsync((GamerDTO?)null);

        // Act
        var result = await _controller.GetGamerByPseudonyme(pseudonyme);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task CreateGamer_WithValidGamer_ShouldReturnCreatedResult()
    {
        // Arrange
        var gamerDto = new GamerDTO
        {
            Pseudonyme = "NewPlayer",
            Email = "newplayer@test.com",
            Age = 22,
            Score = 0
        };

        var expectedId = 123;
        _mockGamerService.Setup(s => s.CreateGamerAsync(gamerDto))
            .ReturnsAsync(expectedId);

        // Act
        var result = await _controller.CreateGamer(gamerDto);

        // Assert
        result.Should().BeOfType<CreatedResult>();
        var createdResult = result as CreatedResult;
        createdResult!.Value.Should().BeEquivalentTo(gamerDto);
        createdResult.StatusCode.Should().Be(201);
    }

    [Fact]
    public async Task GetGamerById_WithValidId_ShouldReturnOkResult()
    {
        // Arrange
        var gamerId = 1;
        var expectedGamer = new GamerDTO 
        { 
            Id = gamerId, 
            Pseudonyme = "TestPlayer", 
            Email = "test@test.com", 
            Age = 25, 
            Score = 300 
        };

        _mockGamerService.Setup(s => s.GetGamerByIdAsync(gamerId))
            .ReturnsAsync(expectedGamer);

        // Act
        var result = await _controller.GetGamerById(gamerId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(expectedGamer);
    }

    [Fact]
    public async Task GetGamerById_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var gamerId = 999;
        _mockGamerService.Setup(s => s.GetGamerByIdAsync(gamerId))
            .ReturnsAsync((GamerDTO?)null);

        // Act
        var result = await _controller.GetGamerById(gamerId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task UpdateGamer_WithValidGamer_ShouldReturnNoContent()
    {
        // Arrange
        var gamerDto = new GamerDTO
        {
            Id = 1,
            Pseudonyme = "UpdatedPlayer",
            Email = "updated@test.com",
            Age = 26,
            Score = 400
        };

        _mockGamerService.Setup(s => s.GetGamerByIdAsync(gamerDto.Id))
            .ReturnsAsync(gamerDto);
        _mockGamerService.Setup(s => s.UpdateGamerAsync(gamerDto))
            .ReturnsAsync(gamerDto);

        // Act
        var result = await _controller.UpdateGamer(gamerDto);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        _mockGamerService.Verify(s => s.UpdateGamerAsync(gamerDto), Times.Once);
    }

    [Fact]
    public async Task UpdateGamer_WithNullGamer_ShouldReturnBadRequest()
    {
        // Act
        var result = await _controller.UpdateGamer(null!);

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }

    [Fact]
    public async Task UpdateGamer_WithNonExistentGamer_ShouldReturnNotFound()
    {
        // Arrange
        var gamerDto = new GamerDTO { Id = 999, Pseudonyme = "NonExistent" };
        _mockGamerService.Setup(s => s.GetGamerByIdAsync(gamerDto.Id))
            .ReturnsAsync((GamerDTO?)null);

        // Act
        var result = await _controller.UpdateGamer(gamerDto);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task DeleteGamer_WithValidId_ShouldReturnNoContent()
    {
        // Arrange
        var gamerId = 1;
        var gamerDto = new GamerDTO { Id = gamerId, Pseudonyme = "ToDelete" };
        
        _mockGamerService.Setup(s => s.GetGamerByIdAsync(gamerId))
            .ReturnsAsync(gamerDto);
        _mockGamerService.Setup(s => s.DeleteGamerAsync(gamerDto))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteGamer(gamerId);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        _mockGamerService.Verify(s => s.DeleteGamerAsync(gamerDto), Times.Once);
    }

    [Fact]
    public async Task DeleteGamer_WithInvalidId_ShouldReturnBadRequest()
    {
        // Arrange
        var gamerId = 999;
        _mockGamerService.Setup(s => s.GetGamerByIdAsync(gamerId))
            .ReturnsAsync((GamerDTO?)null);

        // Act
        var result = await _controller.DeleteGamer(gamerId);

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }

    [Fact]
    public async Task DeleteGamer_WhenServiceThrows_ShouldReturnBadRequest()
    {
        // Arrange
        var gamerId = 1;
        var gamerDto = new GamerDTO { Id = gamerId };
        var expectedException = new Exception("Database error");

        _mockGamerService.Setup(s => s.GetGamerByIdAsync(gamerId))
            .ReturnsAsync(gamerDto);
        _mockGamerService.Setup(s => s.DeleteGamerAsync(gamerDto))
            .ThrowsAsync(expectedException);

        // Act
        var result = await _controller.DeleteGamer(gamerId);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.Value.Should().Be(expectedException.Message);
    }
}
