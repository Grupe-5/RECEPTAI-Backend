using Microsoft.AspNetCore.Mvc;
using receptai.data;

namespace receptai.api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly RecipePlatformDbContext _db;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, RecipePlatformDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        Image a = new()
        {
            ImageData = []
        };
        _db.Images.Add(a);
        _db.SaveChanges();
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
