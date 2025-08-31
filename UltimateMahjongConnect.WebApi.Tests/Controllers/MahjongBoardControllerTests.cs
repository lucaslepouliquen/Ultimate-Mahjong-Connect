using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Ultimate_Mahjong_Connect.Controllers._1._0;
using UltimateMahjongConnect.Application.Services;
using UltimateMahjongConnect.Domain.Interfaces;
using UltimateMahjongConnect.Domain.Models;
using FluentAssertions;
using Xunit;

namespace UltimateMahjongConnect.WebApi.Tests.Controllers;

public class MahjongBoardControllerTests
{
    private readonly Mock<IBoardService> _mockBoardService;
    private readonly Mock<ILogger<MahjongBoardController>> _mockLogger;
    private readonly MahjongBoardController _controller;
    private readonly Mock<IMahjongBoard> _mockBoard;

    public MahjongBoardControllerTests()
    {
        _mockBoardService = new Mock<IBoardService>();
        _mockLogger = new Mock<ILogger<MahjongBoardController>>();
        _mockBoard = new Mock<IMahjongBoard>();
        _controller = new MahjongBoardController(_mockBoardService.Object, _mockLogger.Object);
    }

    [Fact]
    public void GetBoard_WithDeterministicMode_ShouldReturnOkResult()
    {
        // Arrange
        var expectedBoard = CreateMockBoard();
        _mockBoard.Setup(b => b.GetBoard()).Returns(expectedBoard);
        _mockBoardService.Setup(s => s.GetOrCreateBoard()).Returns(_mockBoard.Object);

        // Act
        var result = _controller.GetBoard("deterministic");

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().Be(expectedBoard);

        _mockBoardService.Verify(s => s.GetOrCreateBoard(), Times.Once);
    }

    [Fact]
    public void GetBoard_WithRandomMode_ShouldReturnOkResult()
    {
        // Arrange
        var expectedBoard = CreateMockBoard();
        _mockBoard.Setup(b => b.GetBoard()).Returns(expectedBoard);
        _mockBoardService.Setup(s => s.GetOrCreateBoard()).Returns(_mockBoard.Object);

        // Act
        var result = _controller.GetBoard("random");

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        _mockBoardService.Verify(s => s.GetOrCreateBoard(), Times.Once);
    }

    [Fact]
    public void GetBoard_WithDefaultMode_ShouldUseDeterministicMode()
    {
        // Arrange
        var expectedBoard = CreateMockBoard();
        _mockBoard.Setup(b => b.GetBoard()).Returns(expectedBoard);
        _mockBoardService.Setup(s => s.GetOrCreateBoard()).Returns(_mockBoard.Object);

        // Act
        var result = _controller.GetBoard();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        _mockBoardService.Verify(s => s.GetOrCreateBoard(), Times.Once);
    }

    [Fact]
    public void GetBoard_WhenServiceThrows_ShouldReturnBadRequest()
    {
        // Arrange
        var expectedException = new Exception("Service error");
        _mockBoardService.Setup(s => s.GetOrCreateBoard())
            .Throws(expectedException);

        // Act
        var result = _controller.GetBoard();

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.Value.Should().Be(expectedException.Message);
    }

    [Fact]
    public void ResetBoard_ShouldClearAndCreateNewBoard()
    {
        // Arrange
        var expectedBoard = CreateMockBoard();
        _mockBoard.Setup(b => b.GetBoard()).Returns(expectedBoard);
        _mockBoardService.Setup(s => s.GetOrCreateBoard()).Returns(_mockBoard.Object);

        // Act
        var result = _controller.ResetBoard("deterministic");

        // Assert
        result.Should().BeOfType<OkObjectResult>();

        _mockBoardService.Verify(s => s.ClearBoard(), Times.Once);
        _mockBoardService.Verify(s => s.GetOrCreateBoard(), Times.Once);

        var okResult = result as OkObjectResult;
        var response = okResult!.Value;
        response.Should().NotBeNull();
    }

    [Fact]
    public void ValidateAndExecuteMove_WithValidMove_ShouldReturnSuccessResult()
    {
        // Arrange
        var row1 = 1; var col1 = 1; var row2 = 1; var col2 = 2;
        var mockTile1 = new Mock<IMahjongTile>();
        var mockTile2 = new Mock<IMahjongTile>();
        var mockPath = new MahjongPath { IsValid = true };
        var expectedBoard = CreateMockBoard();

        // Configuration des tuiles dans le tableau pour l'indexeur
        expectedBoard[row1, col1] = mockTile1.Object;
        expectedBoard[row2, col2] = mockTile2.Object;

        _mockBoard.Setup(b => b.GetValidatedPath(row1, col1, row2, col2)).Returns(mockPath);
        _mockBoard.Setup(b => b.GetBoard()).Returns(expectedBoard);
        _mockBoardService.Setup(s => s.GetOrCreateBoard()).Returns(_mockBoard.Object);

        // Act
        var result = _controller.ValidateAndExecuteMove(row1, col1, row2, col2);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        
        _mockBoard.Verify(b => b.MatchAndRemoveTiles(It.IsAny<IMahjongTile>(), It.IsAny<IMahjongTile>()), Times.Once);
        _mockBoardService.Verify(s => s.SaveBoard(_mockBoard.Object), Times.Once);
    }

    [Fact]
    public void ValidateAndExecuteMove_WithInvalidMove_ShouldReturnInvalidResult()
    {
        // Arrange
        var row1 = 1; var col1 = 1; var row2 = 5; var col2 = 5;
        var mockPath = new MahjongPath { IsValid = false };

        _mockBoard.Setup(b => b.GetValidatedPath(row1, col1, row2, col2)).Returns(mockPath);
        _mockBoardService.Setup(s => s.GetOrCreateBoard()).Returns(_mockBoard.Object);

        // Act
        var result = _controller.ValidateAndExecuteMove(row1, col1, row2, col2);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        
        var okResult = result as OkObjectResult;
        var response = okResult!.Value;
        response.Should().NotBeNull();
        
        // Vérifier que les tuiles ne sont pas supprimées pour un mouvement invalide
        _mockBoard.Verify(b => b.MatchAndRemoveTiles(It.IsAny<IMahjongTile>(), It.IsAny<IMahjongTile>()), Times.Never);
        _mockBoardService.Verify(s => s.SaveBoard(It.IsAny<IMahjongBoard>()), Times.Never);
    }

    [Fact]
    public void ValidateAndExecuteMove_WhenServiceThrows_ShouldReturnBadRequest()
    {
        // Arrange
        var expectedException = new Exception("Board service error");
        _mockBoardService.Setup(s => s.GetOrCreateBoard())
            .Throws(expectedException);

        // Act
        var result = _controller.ValidateAndExecuteMove(1, 1, 1, 2);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.Value.Should().Be(expectedException.Message);
    }

    [Fact]
    public void GetPlayableBoard_ShouldInitializePlayableBoard()
    {
        // Arrange
        var expectedBoard = CreateMockBoard();
        _mockBoard.Setup(b => b.GetBoard()).Returns(expectedBoard);
        _mockBoardService.Setup(s => s.GetOrCreateBoard()).Returns(_mockBoard.Object);

        // Act
        var result = _controller.GetPlayableBoard();

        // Assert   
        result.Should().BeOfType<OkObjectResult>();
        
        _mockBoard.Verify(b => b.InitializeBoardPlayable(), Times.Once);
        _mockBoardService.Verify(s => s.SaveBoard(_mockBoard.Object), Times.Once);
        
        var okResult = result as OkObjectResult;
        var response = okResult!.Value;
        response.Should().NotBeNull();
    }

    [Theory]
    [InlineData("deterministic")]
    [InlineData("random")]
    [InlineData("playable")]
    public void ResetBoard_WithDifferentModes_ShouldWork(string mode)
    {
        // Arrange
        var expectedBoard = CreateMockBoard();
        _mockBoard.Setup(b => b.GetBoard()).Returns(expectedBoard);
        _mockBoardService.Setup(s => s.GetOrCreateBoard()).Returns(_mockBoard.Object);

        // Act
        var result = _controller.ResetBoard(mode);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        _mockBoardService.Verify(s => s.ClearBoard(), Times.Once);
        _mockBoardService.Verify(s => s.GetOrCreateBoard(), Times.Once);
    }

    private IMahjongTile[,] CreateMockBoard()
    {
        var board = new IMahjongTile[14, 14];
        for (int i = 0; i < 14; i++)
        {
            for (int j = 0; j < 14; j++)
            {
                var mockTile = new Mock<IMahjongTile>();
                board[i, j] = mockTile.Object;
            }
        }
        return board;
    }
}
