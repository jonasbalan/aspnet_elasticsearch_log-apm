using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace SampleApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        private const string api2UrlBase = "http://sampleapi2:80";

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        
        [Route("Get")]
        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [Route("GetWithError")]        
        [HttpGet]
        public IEnumerable<WeatherForecast> GetWithError()
        {
            throw new Exception("Test elasticsearch");
        }

        [Route("GetFromApi2")]        
        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> GetFromApi2([FromServices] IHttpClientFactory httpClientFactory)
        {
            using var client = httpClientFactory.CreateClient("Api2");

            client.BaseAddress = new Uri(api2UrlBase);
            IEnumerable<WeatherForecast>? enumerable = await client.GetFromJsonAsync<IEnumerable<WeatherForecast>>("WeatherForecast/get");
            return enumerable ?? Enumerable.Empty<WeatherForecast>();
        }

        [Route("GetWithErrorFromApi2")]
        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> GetWithErrorFromApi2([FromServices] IHttpClientFactory httpClientFactory)
        {
            using var client = httpClientFactory.CreateClient("Api2");

            client.BaseAddress = new Uri(api2UrlBase);
            IEnumerable<WeatherForecast>? enumerable = await client.GetFromJsonAsync<IEnumerable<WeatherForecast>>("WeatherForecast/GetWithError");
            return enumerable ?? Enumerable.Empty<WeatherForecast>();
        }
    }
}