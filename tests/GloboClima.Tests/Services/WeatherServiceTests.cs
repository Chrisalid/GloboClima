using GloboClima.Core.DTOs;
using GloboClima.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;
using Xunit;

namespace GloboClima.Tests.Services;

public class WeatherServiceTests
{
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private readonly HttpClient _httpClient;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly WeatherService _weatherService;

    public WeatherServiceTests()
    {
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
        _configurationMock = new Mock<IConfiguration>();
        
        _configurationMock.Setup(x => x["OpenWeatherMap:ApiKey"]).Returns("test-api-key");
        
        _weatherService = new WeatherService(_httpClient, _configurationMock.Object);
    }

    [Fact]
    public async Task GetWeatherAsync_WithValidCity_ReturnsWeatherData()
    {
        // Arrange
        var cityName = "London";
        var expectedWeather = new WeatherResponse
        {
            Name = "London",
            Main = new Main { Temp = 20.5, Humidity = 65 },
            Weather = new List<Weather> { new Weather { Main = "Clear", Description = "clear sky" } }
        };

        var json = JsonSerializer.Serialize(expectedWeather);
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json)
        };

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        // Act
        var result = await _weatherService.GetWeatherAsync(cityName);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("London", result.Name);
        Assert.Equal(20.5, result.Main.Temp);
    }

    [Fact]
    public async Task GetWeatherAsync_WithInvalidCity_ReturnsNull()
    {
        // Arrange
        var cityName = "InvalidCity";
        var response = new HttpResponseMessage(HttpStatusCode.NotFound);

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        // Act
        var result = await _weatherService.GetWeatherAsync(cityName);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetWeatherAsync_WithCoordinates_ReturnsWeatherData()
    {
        // Arrange
        var latitude = 51.5074;
        var longitude = -0.1278;
        var expectedWeather = new WeatherResponse
        {
            Name = "London",
            Coord = new Coord { Lat = latitude, Lon = longitude },
            Main = new Main { Temp = 18.2, Humidity = 70 }
        };

        var json = JsonSerializer.Serialize(expectedWeather);
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json)
        };

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        // Act
        var result = await _weatherService.GetWeatherAsync(latitude, longitude);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(latitude, result.Coord.Lat);
        Assert.Equal(longitude, result.Coord.Lon);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Constructor_WithInvalidApiKey_ThrowsArgumentNullException(string apiKey)
    {
        // Arrange
        var configMock = new Mock<IConfiguration>();
        configMock.Setup(x => x["OpenWeatherMap:ApiKey"]).Returns(apiKey);

        var teste = configMock.Object["OpenWeatherMap:ApiKey"];

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new WeatherService(_httpClient, configMock.Object));
    }
}
