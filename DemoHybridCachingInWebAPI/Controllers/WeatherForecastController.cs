using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Hybrid;

namespace DemoHybridCachingInWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController(HybridCache hybridCache) : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IActionResult> Get()
        {
            string cacheKey = "weatherData";

           var data = await hybridCache
                .GetOrCreateAsync(cacheKey, async token => await GetCast(), default);
            return Ok(data);
        }

        private async Task<WeatherForecast[]> GetCast()
        {
            await Task.Delay(5000);
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
