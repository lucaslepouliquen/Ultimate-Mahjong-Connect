using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using UltimateMahjongConnect.Application.DTO;
using UltimateMahjongConnect.Infrastructure.Models;
using Xunit;

namespace UltimateMahjongConnect.WebApi.Tests.Integration;

public class WebApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public WebApiIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Testing");
            
            builder.ConfigureServices(services =>
            {
                // Remplacer la base de données par une base en mémoire pour les tests
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationDbSQLContext>));
                
                if (descriptor != null)
                    services.Remove(descriptor);

                services.AddDbContext<ApplicationDbSQLContext>(options =>
                {
                    options.UseInMemoryDatabase($"TestDatabase_{Guid.NewGuid()}");
                });
            });
        });
        
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task Get_Ping_ShouldReturnSuccessAndCorrectMessage()
    {
        // Act
        var response = await _client.GetAsync("/api/v1.0/ping");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("Pong");
        content.Should().Contain("server is alive");
    }

    [Fact]
    public async Task Get_Board_ShouldReturnMahjongBoard()
    {
        // Act
        var response = await _client.GetAsync("/api/v1.0/board");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNullOrEmpty();
        
        // Vérifier que c'est un tableau 2D (format JSON)
        content.Should().Contain("[");
        content.Should().Contain("]");
    }

    [Fact]
    public async Task Post_ResetBoard_ShouldResetBoardSuccessfully()
    {
        // Act
        var response = await _client.PostAsync("/api/v1.0/board/reset?mode=deterministic", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("Board reset successfully");
        content.Should().Contain("board");
    }

    [Fact]
    public async Task Get_PlayableBoard_ShouldReturnPlayableBoard()
    {
        // Act
        var response = await _client.GetAsync("/api/v1.0/board/playable");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("Playable board created successfully");
        content.Should().Contain("board");
        content.Should().Contain("Solvable");
    }

    [Fact]
    public async Task Get_ValidateMove_WithValidParameters_ShouldReturnResult()
    {
        // Arrange - D'abord initialiser un plateau
        await _client.PostAsync("/api/v1.0/board/reset?mode=deterministic", null);

        // Act
        var response = await _client.GetAsync("/api/v1.0/board/path?row1=1&column1=1&row2=1&column2=2");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("isValid");
    }

    [Fact]
    public async Task Post_CreateAnonymousSession_ShouldReturnSessionData()
    {
        // Act
        var response = await _client.PostAsync("/api/v1.0/auth/anonymous", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("Anonymous session created successfully");
        content.Should().Contain("data");
    }

    [Fact]
    public async Task Get_AllGamers_ShouldReturnGamersList()
    {
        // Act
        var response = await _client.GetAsync("/api/v1.0/gamers");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNull();
        
        // Devrait être un tableau JSON (même vide)
        content.Should().StartWith("[");
        content.Should().EndWith("]");
    }

    [Fact]
    public async Task Post_CreateGamer_WithValidData_ShouldCreateGamer()
    {
        // Arrange
        var newGamer = new GamerDTO
        {
            Pseudonyme = $"TestPlayer_{Guid.NewGuid():N}",
            Email = $"test_{Guid.NewGuid():N}@example.com",
            Age = 25,
            Score = 0
        };

        var json = JsonConvert.SerializeObject(newGamer);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/v1.0/gamers", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Get_GamerByPseudonyme_WithExistingPseudonyme_ShouldReturnGamer()
    {
        // Arrange - Créer d'abord un joueur
        var newGamer = new GamerDTO
        {
            Pseudonyme = $"FindMePlayer_{Guid.NewGuid():N}",
            Email = $"findme_{Guid.NewGuid():N}@example.com",
            Age = 28,
            Score = 0
        };

        var json = JsonConvert.SerializeObject(newGamer);
        var createContent = new StringContent(json, Encoding.UTF8, "application/json");
        await _client.PostAsync("/api/v1.0/gamers", createContent);

        // Act
        var response = await _client.GetAsync($"/api/v1.0/gamers/byPseudonyme?pseudonyme={newGamer.Pseudonyme}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain(newGamer.Pseudonyme);
        content.Should().Contain(newGamer.Email);
    }

    [Fact]
    public async Task Get_GamerByPseudonyme_WithNonExistentPseudonyme_ShouldReturnNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/v1.0/gamers/byPseudonyme?pseudonyme=NonExistentPlayer123456789");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Theory]
    [InlineData("deterministic")]
    [InlineData("random")]
    [InlineData("playable")]
    public async Task Get_Board_WithDifferentModes_ShouldReturnBoard(string mode)
    {
        // Act
        var response = await _client.GetAsync($"/api/v1.0/board?mode={mode}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNullOrEmpty();
    }

    [Theory]
    [InlineData(0, 0, 0, 1)] // Bordure
    [InlineData(1, 1, 1, 1)] // Même position
    [InlineData(-1, 0, 1, 1)] // Position négative
    public async Task Get_ValidateMove_WithInvalidParameters_ShouldHandleGracefully(int row1, int col1, int row2, int col2)
    {
        // Arrange
        await _client.PostAsync("/api/v1.0/board/reset", null);

        // Act
        var response = await _client.GetAsync($"/api/v1.0/board/path?row1={row1}&column1={col1}&row2={row2}&column2={col2}");

        // Assert
        // L'API ne devrait pas crasher, même avec des paramètres invalides
        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Multiple_Concurrent_Requests_ShouldHandleCorrectly()
    {
        // Arrange
        var tasks = new List<Task<HttpResponseMessage>>();

        // Act - Faire plusieurs requêtes simultanées
        for (int i = 0; i < 10; i++)
        {
            tasks.Add(_client.GetAsync("/api/v1.0/ping"));
            tasks.Add(_client.GetAsync("/api/v1.0/board"));
            tasks.Add(_client.PostAsync("/api/v1.0/auth/anonymous", null));
        }

        var responses = await Task.WhenAll(tasks);

        // Assert
        responses.Should().HaveCount(30);
        responses.Should().AllSatisfy(response =>
        {
            response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.Created);
        });
    }

    [Fact]
    public async Task API_Endpoints_ShouldBeAccessible()
    {
        // Arrange
        var endpoints = new[]
        {
            "/api/v1.0/ping",
            "/api/v1.0/board",
            "/api/v1.0/gamers"
        };

        // Act & Assert
        foreach (var endpoint in endpoints)
        {
            var response = await _client.GetAsync(endpoint);
            response.StatusCode.Should().BeOneOf(
                HttpStatusCode.OK, 
                HttpStatusCode.Unauthorized, // Peut être protégé
                HttpStatusCode.NotFound // Peut ne pas exister
            );
        }
    }

    [Fact]
    public async Task API_ShouldHandleInvalidRoutes()
    {
        // Act
        var response = await _client.GetAsync("/api/v1.0/nonexistent");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task API_ShouldReturnCorrectContentType()
    {
        // Act
        var response = await _client.GetAsync("/api/v1.0/ping");

        // Assert
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/json");
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
