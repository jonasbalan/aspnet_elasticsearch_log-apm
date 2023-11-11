using Common.MS.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace SampleApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        private const string api2UrlBase = "https://internal.api.sample-ms.server.com";



        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }


        [Route("Get")]
        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            _logger.LogInformation("Init Get WeatherForecast");
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
            var ex = new Exception("Test elasticsearch");
            //_logger.LogError(ex, "Get With Error");
            throw ex;

        }

        [Route("GetFromApi2")]
        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> GetFromApi2([FromServices] IHttpClientFactory httpClientFactory)
        {
            await Task.Delay(100);// simulate doing something
            using var client = httpClientFactory.CreateClient("internalApis");
            client.BaseAddress = new Uri(api2UrlBase);
            IEnumerable<WeatherForecast>? enumerable = await client.GetJsonWithAuthorizationAsync<IEnumerable<WeatherForecast>>(HttpContext,"api2/WeatherForecast/get");
            return enumerable ?? Enumerable.Empty<WeatherForecast>();
        }

        [Route("GetWithErrorFromApi2")]
        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> GetWithErrorFromApi2([FromServices] IHttpClientFactory httpClientFactory)
        {
            await Task.Delay(100);// simulate doing something
            using var client = httpClientFactory.CreateClient("internalApis");
            client.BaseAddress = new Uri(api2UrlBase);
            IEnumerable<WeatherForecast>? enumerable = await client.GetJsonWithAuthorizationAsync<IEnumerable<WeatherForecast>>(HttpContext, "api2/WeatherForecast/GetWithError");
            return enumerable ?? Enumerable.Empty<WeatherForecast>();
        }
    }
}