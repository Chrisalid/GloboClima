using GloboClima.API.Controllers;
using GloboClima.Core.DTOs;
using GloboClima.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace GloboClima.Tests.Controllers;

public class WeatherControllerTests
{
    private readonly Mock<IWeatherService> _weatherServiceMock;
    private readonly WeatherController _controller;

    public WeatherControllerTests()
    {
        _weatherServiceMock = new Mock<IWeatherService>();
        _controller = new WeatherController(_weatherServiceMock.Object);
    }

    [Fact]
    public async Task GetWeather_WithValidCity_ReturnsOkResult()
    {
        // Arrange
        var cityName = "London";
        var expectedWeather = new WeatherResponse
        {
            Name = "London",
            Main = new Main { Temp = 20.5 },
            Weather = new List<Weather> { new Weather { Main = "Clear", Description = "clear sky" } }
        };

        _weatherServiceMock
            .Setup(x => x.GetWeatherAsync(cityName))
            .ReturnsAsync(expectedWeather);

        // Act
        var result = await _controller.GetWeather(cityName);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var weather = Assert.IsType<WeatherResponse>(okResult.Value);
        Assert.Equal("London", weather.Name);
    }

    [Fact]
    public async Task GetWeather_WithInvalidCity_ReturnsNotFound()
    {
        // Arrange
        var cityName = "InvalidCity";
        _weatherServiceMock
            .Setup(x => x.GetWeatherAsync(cityName))
            .ReturnsAsync((WeatherResponse?)null);

        // Act
        var result = await _controller.GetWeather(cityName);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Contains("InvalidCity", notFoundResult.Value?.ToString());
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task GetWeather_WithEmptyCity_ReturnsBadRequest(string cityName)
    {
        // Act
        var result = await _controller.GetWeather(cityName);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetWeatherByCoordinates_WithValidCoordinates_ReturnsOkResult()
    {
        // Arrange
        var latitude = 51.5074;
        var longitude = -0.1278;
        var expectedWeather = new WeatherResponse
        {
            Name = "London",
            Coord = new Coord { Lat = latitude, Lon = longitude },
            Main = new Main { Temp = 18.2 }
        };

        _weatherServiceMock
            .Setup(x => x.GetWeatherAsync(latitude, longitude))
            .ReturnsAsync(expectedWeather);

        // Act
        var result = await _controller.GetWeatherByCoordinates(latitude, longitude);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var weather = Assert.IsType<WeatherResponse>(okResult.Value);
        Assert.Equal(latitude, weather.Coord.Lat);
        Assert.Equal(longitude, weather.Coord.Lon);
    }

    [Fact]
    public async Task GetWeatherByCoordinates_WithInvalidCoordinates_ReturnsNotFound()
    {
        // Arrange
        var latitude = 999.0;
        var longitude = 999.0;
        _weatherServiceMock
            .Setup(x => x.GetWeatherAsync(latitude, longitude))
            .ReturnsAsync((WeatherResponse?)null);

        // Act
        var result = await _controller.GetWeatherByCoordinates(latitude, longitude);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Contains($"{latitude}, {longitude}", notFoundResult.Value?.ToString());
    }
}
