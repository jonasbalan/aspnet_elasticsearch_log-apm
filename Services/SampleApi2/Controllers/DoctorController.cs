using Microsoft.AspNetCore.Mvc;
using Common.MS.Auth;
using Microsoft.AspNetCore.Authorization;

namespace SampleApi2.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class DoctorController : ControllerBase
    {

        private const string api3UrlBase = "https://internal.api.sample-ms.server.com";

        private readonly ILogger<DoctorController> _logger;

        public DoctorController(ILogger<DoctorController> logger)
        {
            _logger = logger;
        }


        [HttpGet]
        public async Task<IEnumerable<dynamic>?> GetFromApi3([FromServices] IHttpClientFactory httpClientFactory)
        {
            await Task.Delay(100);// simulate doing something
            using var client = httpClientFactory.CreateClient("internalApis");
            client.BaseAddress = new Uri(api3UrlBase);

            var enumerable = await client.GetJsonWithAuthorizationAsync<IEnumerable<dynamic>>(HttpContext, "api3/doctor");

            return enumerable;
        }
    }
}