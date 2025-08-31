using System.Diagnostics;
using FluentAssertions;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;

namespace UltimateMahjongConnect.WebApi.Tests.Performance;

public class ApiPerformanceTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public ApiPerformanceTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task Ping_Endpoint_ShouldRespondWithinAcceptableTime()
    {
        // Arrange
        var stopwatch = new Stopwatch();

        // Act
        stopwatch.Start();
        var response = await _client.GetAsync("/api/v1.0/ping");
        stopwatch.Stop();

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(1000, "Ping should respond within 1 second");
    }

    [Fact]
    public async Task Board_Creation_ShouldCompleteWithinReasonableTime()
    {
        // Arrange
        var stopwatch = new Stopwatch();

        // Act
        stopwatch.Start();
        var response = await _client.GetAsync("/api/v1.0/board");
        stopwatch.Stop();

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(3000, "Board creation should complete within 3 seconds");
    }

    [Fact]
    public async Task Playable_Board_Creation_ShouldCompleteWithinReasonableTime()
    {
        // Arrange
        var stopwatch = new Stopwatch();

        // Act
        stopwatch.Start();
        var response = await _client.GetAsync("/api/v1.0/board/playable");
        stopwatch.Stop();

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(5000, "Playable board creation should complete within 5 seconds");
    }

    [Fact]
    public async Task Concurrent_Ping_Requests_ShouldHandleLoad()
    {
        // Arrange
        const int numberOfRequests = 50;
        var tasks = new List<Task<HttpResponseMessage>>();
        var stopwatch = new Stopwatch();

        // Act
        stopwatch.Start();
        for (int i = 0; i < numberOfRequests; i++)
        {
            tasks.Add(_client.GetAsync("/api/v1.0/ping"));
        }

        var responses = await Task.WhenAll(tasks);
        stopwatch.Stop();

        // Assert
        responses.Should().HaveCount(numberOfRequests);
        responses.Should().AllSatisfy(response => 
            response.IsSuccessStatusCode.Should().BeTrue());
        
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(10000, 
            $"{numberOfRequests} concurrent requests should complete within 10 seconds");
        
        // Calculer le temps moyen par requête
        var averageTime = stopwatch.ElapsedMilliseconds / (double)numberOfRequests;
        averageTime.Should().BeLessThan(200, "Average response time should be less than 200ms");
    }

    [Fact]
    public async Task Board_Reset_Multiple_Times_ShouldMaintainPerformance()
    {
        // Arrange
        const int numberOfResets = 10;
        var responseTimes = new List<long>();

        // Act
        for (int i = 0; i < numberOfResets; i++)
        {
            var stopwatch = Stopwatch.StartNew();
            var response = await _client.PostAsync("/api/v1.0/board/reset", null);
            stopwatch.Stop();

            response.IsSuccessStatusCode.Should().BeTrue();
            responseTimes.Add(stopwatch.ElapsedMilliseconds);
        }

        // Assert
        responseTimes.Should().AllSatisfy(time => 
            time.Should().BeLessThan(3000, "Each board reset should complete within 3 seconds"));
        
        var averageTime = responseTimes.Average();
        averageTime.Should().BeLessThan(2000, "Average board reset time should be less than 2 seconds");
    }

    [Fact]
    public async Task Memory_Usage_ShouldNotGrowExcessively_DuringBoardOperations()
    {
        // Arrange
        var initialMemory = GC.GetTotalMemory(true);
        const int numberOfOperations = 20;

        // Act
        for (int i = 0; i < numberOfOperations; i++)
        {
            await _client.GetAsync("/api/v1.0/board");
            await _client.PostAsync("/api/v1.0/board/reset", null);
            await _client.GetAsync("/api/v1.0/board/playable");
        }

        // Force garbage collection
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        var finalMemory = GC.GetTotalMemory(false);

        // Assert
        var memoryIncrease = finalMemory - initialMemory;
        var memoryIncreaseInMB = memoryIncrease / (1024.0 * 1024.0);
        
        memoryIncreaseInMB.Should().BeLessThan(50, 
            $"Memory increase should be less than 50MB after {numberOfOperations} operations");
    }

    [Fact]
    public async Task API_ShouldHandleRapidSequentialRequests()
    {
        // Arrange
        var tasks = new List<Task<HttpResponseMessage>>();
        var stopwatch = new Stopwatch();

        // Act
        stopwatch.Start();
        
        // Ping requests
        for (int i = 0; i < 5; i++)
        {
            tasks.Add(_client.GetAsync("/api/v1.0/ping"));
        }
        
        // Board requests
        for (int i = 0; i < 5; i++)
        {
            tasks.Add(_client.GetAsync("/api/v1.0/board"));
        }
        
        // Gamers requests
        for (int i = 0; i < 5; i++)
        {
            tasks.Add(_client.GetAsync("/api/v1.0/gamers"));
        }
        
        // Auth requests
        for (int i = 0; i < 5; i++)
        {
            tasks.Add(_client.PostAsync("/api/v1.0/auth/anonymous", null));
        }

        var responses = await Task.WhenAll(tasks);
        stopwatch.Stop();

        // Assert
        responses.Should().HaveCount(20);
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(15000, 
            "Rapid sequential requests should complete within 15 seconds");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    public async Task Board_PathValidation_ShouldScaleWithComplexity(int numberOfValidations)
    {
        // Arrange
        await _client.PostAsync("/api/v1.0/board/reset", null);
        var stopwatch = new Stopwatch();

        // Act
        stopwatch.Start();
        for (int i = 0; i < numberOfValidations; i++)
        {
            var response = await _client.GetAsync($"/api/v1.0/board/path?row1=1&column1=1&row2=1&column2={2 + i}");
            response.IsSuccessStatusCode.Should().BeTrue();
        }
        stopwatch.Stop();

        // Assert
        var averageTime = stopwatch.ElapsedMilliseconds / (double)numberOfValidations;
        averageTime.Should().BeLessThan(500, 
            $"Average path validation time should be less than 500ms for {numberOfValidations} validations");
    }

    [Fact]
    public async Task Load_Test_ShouldHandleMultipleUsersSimulation()
    {
        // Arrange
        const int numberOfUsers = 20;
        var tasks = new List<Task>();
        var stopwatch = new Stopwatch();

        // Act
        stopwatch.Start();
        for (int userId = 0; userId < numberOfUsers; userId++)
        {
            tasks.Add(SimulateUserSession(userId));
        }

        await Task.WhenAll(tasks);
        stopwatch.Stop();

        // Assert
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(30000,
            $"Load test with {numberOfUsers} users should complete within 30 seconds");
    }

    private async Task SimulateUserSession(int userId)
    {
        // Simuler une session utilisateur typique
        try
        {
            // 1. Create anonymous session
            await _client.PostAsync("/api/v1.0/auth/anonymous", null);
            
            // 2. Get board
            await _client.GetAsync("/api/v1.0/board");
            
            // 3. Reset board (simulate game restart)
            await _client.PostAsync("/api/v1.0/board/reset", null);
            
            // 4. Validate some moves
            for (int move = 0; move < 3; move++)
            {
                await _client.GetAsync($"/api/v1.0/board/path?row1=1&column1=1&row2=1&column2={move + 2}");
            }
            
            // 5. Get gamers list
            await _client.GetAsync("/api/v1.0/gamers");
        }
        catch
        {
            // Ignorer les erreurs dans ce test de charge
            // L'important est de vérifier que le serveur ne crash pas
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _client?.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
