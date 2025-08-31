using Microsoft.AspNetCore.Mvc;
using Ultimate_Mahjong_Connect.Controllers.V1;
using FluentAssertions;
using Xunit;

namespace UltimateMahjongConnect.WebApi.Tests.Controllers;

public class PingControllerTests
{
    private readonly PingController _controller;

    public PingControllerTests()
    {
        _controller = new PingController();
    }

    [Fact]
    public async Task Get_ShouldReturnOkResult_WithPongMessage()
    {
        // Act
        var result = await _controller.Get();

        // Assert
        result.Should().BeOfType<ActionResult<string>>();
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);
        
        var response = okResult.Value as string;
        response.Should().NotBeNull();
        response.Should().Contain("Pong");
        response.Should().Contain("server is alive");
    }

    [Fact]
    public async Task Get_ShouldReturnCorrectAssemblyName()
    {
        // Act
        var result = await _controller.Get();

        // Assert
        var okResult = result.Result as OkObjectResult;
        var response = okResult!.Value as string;
        
        response.Should().Contain("UltimateMahjongConnect.Server.WebApi.Net");
    }

    [Fact]
    public async Task Get_ShouldAlwaysReturnOk_NeverThrow()
    {
        // Arrange & Act
        var tasks = Enumerable.Range(0, 10)
            .Select(_ => _controller.Get())
            .ToArray();

        var results = await Task.WhenAll(tasks);

        // Assert
        results.Should().HaveCount(10);
        results.Should().AllSatisfy(result =>
        {
            result.Result.Should().BeOfType<OkObjectResult>();
        });
    }
}
