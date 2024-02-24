using application.DTOs.infrastructure;
using application.infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("testing")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries =
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly IBookRepository _bookRepository;

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IBookRepository bookRepository)
    {
        _logger = logger;
        _bookRepository = bookRepository;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }

    [HttpGet("books", Name = "GetBooks")]
    public async Task<IActionResult> LoadBooks([FromQuery] LoadingBooksRequest request)
    {
        try
        {
            // if i Got a time I will implement search using elastic search because it is faster and more efficient 
            var books = await _bookRepository.SearchAsync(request);
            return Ok(books);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading books");
            return StatusCode(500, "Internal server error");
        }
    }
}